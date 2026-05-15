// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using Content.Shared._Floof.Consent;
using Content.Shared.Administration.Logs;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;


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
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    private readonly ProtoId<ConsentTogglePrototype> _nsfwDescriptionsConsent = "NSFWDescriptions";

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, AfterAutoHandleStateEvent>(OnHandleState);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, BeforeRangedInteractEvent>(OnBeforeInteract);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, LabelableHolosignDescriptionMessage>(OnHolosignDescriptionChanged);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, LabelableHolosignSignChosen>(OnSignChosen);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, LabelableHolosignOpenOtherUI>(OnOpenOtherUI);
        SubscribeLocalEvent<LabelableHolosignProjectorComponent, ExaminedEvent>(OnProjectorExamined);

        SubscribeLocalEvent<LabeledHolosignComponent, ExaminedEvent>(OnSignExamine);
    }

    private void OnSignExamine(EntityUid uid, LabeledHolosignComponent component, ExaminedEvent args)
    {
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

    private void OnProjectorExamined(Entity<LabelableHolosignProjectorComponent> entity, ref ExaminedEvent evt)
    {
        if (entity.Comp.SelectedSignProto is { } signProto)
        {
            evt.PushMarkup(Loc.GetString("labelable-holoprojector-selected-sign", ("sign", signProto)));
        }
    }

    private void OnBeforeInteract(Entity<LabelableHolosignProjectorComponent> ent, ref BeforeRangedInteractEvent args)
    {
        if (args.Handled || !args.CanReach ||
            HasComp<StorageComponent>(args.Target) ||
            HasComp<ItemSlotsComponent>(args.Target))
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
        if (ent.Comp.SelectedSignProto == null)
        {
            if (!_uiSystem.HasUi(ent, LabelableHolosignUIKey.Signs))
                return false;
            _uiSystem.OpenUi(ent.Owner, LabelableHolosignUIKey.Signs, user);
            UpdateUI(ent);
            return true;
        }
        
        if (ent.Comp.BarrierDescription.Length == 0)
        {
            if (!_uiSystem.HasUi(ent, LabelableHolosignUIKey.Description))
                return false;
            _uiSystem.OpenUi(ent.Owner, LabelableHolosignUIKey.Description, user);
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
            ent.Comp.SelectedSignProto,
            args.ClickLocation.SnapToGrid(EntityManager));

        var labelComp = EnsureComp<LabeledHolosignComponent>(holoUid);
        labelComp.Description = ent.Comp.BarrierDescription;
        labelComp.IsNSFW = ent.Comp.IsNsfw;
        Dirty(holoUid, labelComp);

        var xform = Transform(holoUid);
        if (!xform.Anchored)
            _transform.AnchorEntity(holoUid, xform);

        var nsfwStr = labelComp.IsNSFW ? "nsfw" : "";
        _adminLogger.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(user):user} placed a {ToPrettyString(holoUid):holosign} with {nsfwStr} description {labelComp.Description}");
        
        return true;
    }

    private void OnHandleState(Entity<LabelableHolosignProjectorComponent> ent, ref AfterAutoHandleStateEvent evt)
    {
        UpdateUI(ent);
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

    protected virtual void UpdateUI(Entity<LabelableHolosignProjectorComponent> entity) { }

    private void OnHolosignDescriptionChanged(
        EntityUid uid,
        LabelableHolosignProjectorComponent component,
        LabelableHolosignDescriptionMessage args
    )
    {
        var description = args.Description.Trim();
        component.BarrierDescription = description[..Math.Min(component.MaxDescriptionChars, description.Length)];
        component.IsNsfw = args.IsNsfw;
        UpdateUI((uid, component));
        Dirty(uid, component);
    }

    private void OnSignChosen(Entity<LabelableHolosignProjectorComponent> entity, ref LabelableHolosignSignChosen args)
    {
        // Use an index because trusting the client to send an arbitrary ProtoId seems bad.
        if (entity.Comp.SignProtos.TryGetValue(args.Selection, out var signProtoId) &&
            _prototypeManager.TryIndex(signProtoId, out var proto))
        {
            entity.Comp.SelectedSignProto = proto;
            UpdateUI(entity);
            Dirty(entity);
        }
    }

    private void OnOpenOtherUI(Entity<LabelableHolosignProjectorComponent> entity, 
        ref LabelableHolosignOpenOtherUI args)
    {
        // Have to send this over here to open the BUI since I can't seem to do it from inside the UI.
        var user = args.Actor;
        if (!_uiSystem.HasUi(entity, LabelableHolosignUIKey.Description))
            return;
        _uiSystem.OpenUi(entity.Owner, LabelableHolosignUIKey.Description, user);
        UpdateUI(entity);
    }
}
