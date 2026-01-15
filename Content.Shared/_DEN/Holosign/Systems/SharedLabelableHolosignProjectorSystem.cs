using System.Linq;
using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using Content.Shared._Floof.Consent;
using Content.Shared.Administration.Logs;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.Holosign.Systems;

public abstract class SharedLabelableHolosignProjectorSystem : EntitySystem
{
    [Dependency] protected readonly SharedUserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedChargesSystem _charges = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly SharedConsentSystem _consent = default!;

    private readonly ProtoId<ConsentTogglePrototype> _nsfwDescriptionsConsent = "NSFWDescriptions";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LabelableHolosignProjectorComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, ComponentHandleState>(OnHandleState);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, LabelableHolosignChangedMessage>(OnHolosignDescriptionChanged);

        SubscribeLocalEvent<LabeledHolosignComponent, ExaminedEvent>(OnExamine);
    }

    private void OnExamine(EntityUid uid, LabeledHolosignComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        if (component.IsNSFW)
        {
            if(_consent.HasConsent(args.Examiner, _nsfwDescriptionsConsent))
                args.PushMarkup(component.Description);
            else
            {
                args.PushMarkup(Loc.GetString("labelable-holoprojector-consent-not-available"));
            }
        }
        else
        {
            args.PushMarkup(component.Description);
        }
    }

    private void OnBeforeInteract(Entity<LabelableHolosignProjectorComponent> ent, ref BeforeRangedInteractEvent args)
    {
        if (args.Handled || !args.CanReach ||
            HasComp<StorageComponent>(args.Target))
            return;

        var coords = args.ClickLocation.SnapToGrid(EntityManager);
        var mapCoords = _transform.ToMapCoordinates(coords);

        var matches = _lookup.GetEntitiesInRange(mapCoords, 0.25f);
        matches.RemoveWhere(match => _whitelist.IsWhitelistFail(ent.Comp.SignWhitelist, match));

        if (matches.Count == 0)
            args.Handled = TryPlaceSign(ent, args, args.User);
        else
            args.Handled = TryRemoveSign(ent, matches.First(), args.User);
    }

    private bool TryPlaceSign(Entity<LabelableHolosignProjectorComponent> ent, BeforeRangedInteractEvent args, EntityUid user)
    {
        if (ent.Comp.BarrierDescription.Length == 0)
        {
            if (!_uiSystem.HasUi(ent, LabelableHolosignUIKey.Key))
                return false;
            _uiSystem.OpenUi(ent.AsType(), LabelableHolosignUIKey.Key, user);
            UpdateUI(ent);
            return true;
        }

        if (ent.Comp.UsesCharges)
        {
            if (!TryComp<LimitedChargesComponent>(ent, out var charges) || !_charges.TryUseCharge((ent, charges)))
            {
                _popup.PopupClient(Loc.GetString("labelable-holoprojector-no-charges", ("item", ent)), ent, args.User);
                return false;
            }
        }

        var holoUid = EntityManager.PredictedSpawnAtPosition(
            ent.Comp.SignProto,
            args.ClickLocation.SnapToGrid(EntityManager));

        var labelComp = EnsureComp<LabeledHolosignComponent>(holoUid);
        labelComp.Description = ent.Comp.BarrierDescription;
        labelComp.IsNSFW = ent.Comp.IsNSFW;
        Dirty(holoUid, labelComp);

        var xform = Transform(holoUid);
        if (!xform.Anchored)
            _transform.AnchorEntity(holoUid, xform);

        return true;
    }

    private bool TryRemoveSign(Entity<LabelableHolosignProjectorComponent> ent, EntityUid sign, EntityUid user)
    {
        if (ent.Comp.UsesCharges && TryComp<LimitedChargesComponent>(ent, out var charges))
            _charges.AddCharges(ent, 1, charges);

        var userIdentity = Identity.Name(user, EntityManager);
        _popup.PopupPredicted(
            Loc.GetString("labelable-holoprojector-reclaim", ("sign", sign)),
            Loc.GetString("labelable-holoprojector-reclaim-others", ("sign", sign), ("user", userIdentity)),
            ent,
            user);

        EntityManager.PredictedDeleteEntity(sign);

        return true;
    }

    private void OnGetState(Entity<LabelableHolosignProjectorComponent> entity, ref ComponentGetState args)
    {
        args.State = new LabelableHolosignProjectorComponentState(entity.Comp.BarrierDescription)
        {
            MaxDescriptionChars = entity.Comp.MaxDescriptionChars,
        };
    }

    private void OnHandleState(Entity<LabelableHolosignProjectorComponent> entity, ref ComponentHandleState args)
    {
        if (args.Current is not LabelableHolosignProjectorComponentState state)
            return;

        entity.Comp.MaxDescriptionChars = state.MaxDescriptionChars;

        if (entity.Comp.BarrierDescription == state.BarrierDescription)
            return;

        entity.Comp.BarrierDescription = state.BarrierDescription;
        UpdateUI(entity);
    }

    protected virtual void UpdateUI(Entity<LabelableHolosignProjectorComponent> entity) { }

    private void OnHolosignDescriptionChanged(
        EntityUid uid,
        LabelableHolosignProjectorComponent component,
        LabelableHolosignChangedMessage args
    )
    {
        var description = args.Description.Trim();
        component.BarrierDescription = description[..Math.Min(component.MaxDescriptionChars, description.Length)];
        UpdateUI((uid, component));
        Dirty(uid, component);
    }
}
