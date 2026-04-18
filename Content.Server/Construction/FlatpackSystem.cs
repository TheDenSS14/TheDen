// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 SpeltIncorrectyl
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Server.Audio;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Construction;
using Content.Shared.Construction.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Power;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Construction;

/// <inheritdoc/>
public sealed class FlatpackSystem : SharedFlatpackSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly AmbientSoundSystem _ambientSound = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlatpackCreatorComponent, FlatpackCreatorStartPackBuiMessage>(OnStartPack);
        SubscribeLocalEvent<FlatpackCreatorComponent, PowerChangedEvent>(OnPowerChanged);

        SubscribeLocalEvent<FlatpackCreatorComponent, RefreshPartsEvent>(OnPartsRefresh); // DEN: Part upgrades
        SubscribeLocalEvent<FlatpackCreatorComponent, UpgradeExamineEvent>(OnUpgradeExamine); // DEN: Part upgrades
    }

    private void OnStartPack(Entity<FlatpackCreatorComponent> ent, ref FlatpackCreatorStartPackBuiMessage args)
    {
        var (uid, comp) = ent;
        if (!this.IsPowered(ent, EntityManager) || comp.Packing)
            return;

        if (!_itemSlots.TryGetSlot(uid, comp.SlotId, out var itemSlot) || itemSlot.Item is not { } machineBoard)
            return;

        Dictionary<string, int>? cost = null;
        if (TryComp<MachineBoardComponent>(machineBoard, out var machineBoardComponent))
            cost = GetFlatpackCreationCost(ent, (machineBoard, machineBoardComponent));
        if (HasComp<ComputerBoardComponent>(machineBoard))
            cost = GetFlatpackCreationCost(ent);

        if (cost is null)
            return;

        // DEN: Part Upgrades
        foreach (var (mat, amount) in cost)
        {
            var adjustedAmount = amount * ent.Comp.FinalMaterialUseMultiplier;
            cost[mat] = (int)adjustedAmount;
        }

        if (!MaterialStorage.CanChangeMaterialAmount(uid, cost))
            return;

        comp.Packing = true;
        comp.PackEndTime = _timing.CurTime + (comp.PackDuration * comp.FinalTimeMultiplier); // DEN: Part upgrades.
        Appearance.SetData(uid, FlatpackCreatorVisuals.Packing, true);
        _ambientSound.SetAmbience(uid, true);
        Dirty(uid, comp);
    }

    private void OnPowerChanged(Entity<FlatpackCreatorComponent> ent, ref PowerChangedEvent args)
    {
        if (args.Powered)
            return;
        FinishPacking(ent, true);
    }

    // DEN: Part upgrades
    private void OnPartsRefresh(Entity<FlatpackCreatorComponent> ent, ref RefreshPartsEvent args)
    {
        var printTimeRating = args.PartRatings[ent.Comp.MachinePartPrintSpeed];
        var materialUseRating = args.PartRatings[ent.Comp.MachinePartMaterialUse];

        ent.Comp.FinalTimeMultiplier= MathF.Pow(ent.Comp.PartRatingPrintTimeMultiplier, printTimeRating - 1);
        ent.Comp.FinalMaterialUseMultiplier = MathF.Pow(ent.Comp.PartRatingMaterialUseMultiplier, materialUseRating - 1);
        Dirty(ent);
    }

    // DEN: Part upgrades
    private void OnUpgradeExamine(Entity<FlatpackCreatorComponent> ent, ref UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade("lathe-component-upgrade-speed", 1 / ent.Comp.FinalTimeMultiplier);
        args.AddPercentageUpgrade("lathe-component-upgrade-material-use", ent.Comp.FinalMaterialUseMultiplier);
    }

    private void FinishPacking(Entity<FlatpackCreatorComponent> ent, bool interrupted)
    {
        var (uid, comp) = ent;

        comp.Packing = false;
        Appearance.SetData(uid, FlatpackCreatorVisuals.Packing, false);
        _ambientSound.SetAmbience(uid, false);
        Dirty(uid, comp);

        if (interrupted)
            return;

        if (!_itemSlots.TryGetSlot(uid, comp.SlotId, out var itemSlot) || itemSlot.Item is not { } machineBoard)
            return;

        Dictionary<string, int>? cost = null;
        if (TryComp<MachineBoardComponent>(machineBoard, out var machineBoardComponent))
            cost = GetFlatpackCreationCost(ent, (machineBoard, machineBoardComponent));
        if (HasComp<ComputerBoardComponent>(machineBoard))
            cost = GetFlatpackCreationCost(ent);

        if (cost is null)
            return;

        if (!MaterialStorage.TryChangeMaterialAmount((ent, null), cost))
            return;

        var flatpack = Spawn(comp.BaseFlatpackPrototype, Transform(ent).Coordinates);
        SetupFlatpack(flatpack, machineBoard);
        Del(machineBoard);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<FlatpackCreatorComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.Packing)
                continue;

            if (_timing.CurTime < comp.PackEndTime)
                continue;

            FinishPacking((uid, comp), false);
        }
    }
}
