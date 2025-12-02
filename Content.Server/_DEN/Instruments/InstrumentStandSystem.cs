using Content.Server.Instruments;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Instruments;
using Content.Shared.UserInterface;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;

namespace Content.Shared._DEN.Instruments;

public sealed partial class InstrumentStandSystem : EntitySystem
{
    [Dependency] private readonly ActivatableUISystem _activatableUI = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<InstrumentStandComponent, ComponentInit>(OnInstrumentStandInit);
        SubscribeLocalEvent<InstrumentStandComponent, EntInsertedIntoContainerMessage>(OnInstrumentStandItemInserted);
        SubscribeLocalEvent<InstrumentStandComponent, EntRemovedFromContainerMessage>(OnInstrumentStandItemRemoved);
    }

    private void OnInstrumentStandInit(Entity<InstrumentStandComponent> ent, ref ComponentInit args)
    {
        RefreshInstrumentComponent(ent);
    }

    private void OnInstrumentStandItemInserted(Entity<InstrumentStandComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (!ent.Comp.Initialized || args.Container.ID != ent.Comp.SlotId)
            return;

        RefreshInstrumentComponent(ent);
    }

    private void OnInstrumentStandItemRemoved(Entity<InstrumentStandComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (!ent.Comp.Initialized || args.Container.ID != ent.Comp.SlotId)
            return;

        RefreshInstrumentComponent(ent);
    }

    private void RefreshInstrumentComponent(Entity<InstrumentStandComponent> ent)
    {
        if (!_itemSlots.TryGetSlot(ent, ent.Comp.SlotId, out var itemSlot))
            return;

        var hasInstrument = itemSlot.Item != null;

        if (hasInstrument && ent.Comp.Instrument == null)
            AddInstrumentComponent(ent, itemSlot);
        else if (!hasInstrument && ent.Comp.Instrument != null)
            RemoveInstrumentComponent(ent);

        UpdateInstrumentStandInterface(ent);
    }

    private void AddInstrumentComponent(Entity<InstrumentStandComponent> ent, ItemSlot slot)
    {
        if (slot.Item == null || !TryComp<InstrumentComponent>(slot.Item, out var instrument))
            return;

        var item = slot.Item.Value;
        ent.Comp.Instrument = CopyComp(item, ent.Owner, instrument);

        if (TryComp<SwappableInstrumentComponent>(item, out var swappable))
            CopyComp(item, ent.Owner, swappable);
    }

    private void RemoveInstrumentComponent(Entity<InstrumentStandComponent> ent)
    {
        if (ent.Comp.Instrument == null)
            return;

        RemComp(ent, ent.Comp.Instrument);
        ent.Comp.Instrument = null;

        if (TryComp<SwappableInstrumentComponent>(ent, out var swappable))
            RemComp(ent, swappable);
    }

    private void UpdateInstrumentStandInterface(Entity<InstrumentStandComponent> ent)
    {
        if (!TryComp<ActivatableUIComponent>(ent, out var aui)
            || !_itemSlots.TryGetSlot(ent, ent.Comp.SlotId, out var itemSlot))
            return;

        var currentlyEnabled = aui.RequiredItems == null;
        var enabled = itemSlot.Item != null;

        if (currentlyEnabled != enabled)
        {
            // if the stand's state changes, close all UIs
            _activatableUI.CloseAll(ent, aui);

            // sorry, this is kind of a dirty way of doing it
            aui.RequiredItems = enabled ? null : new EntityWhitelist() { Tags = [] };
        }
    }
}
