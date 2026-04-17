// SPDX-FileCopyrightText: 2024 Nemanja
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 SpeltIncorrectyl
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 Dirius77
//
// SPDX-License-Identifier: MIT

using Content.Shared.Materials;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Construction.Components;

/// <summary>
/// This is used for a machine that creates flatpacks at the cost of materials
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedFlatpackSystem))]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class FlatpackCreatorComponent : Component
{
    /// <summary>
    /// Whether or not packing is occuring
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool Packing;

    /// <summary>
    /// The time at which packing ends
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan PackEndTime;

    /// <summary>
    /// How long packing lasts.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan PackDuration = TimeSpan.FromSeconds(3);

    /// <summary>
    /// The prototype used when spawning a flatpack.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId BaseFlatpackPrototype = "BaseFlatpack";

    /// <summary>
    /// A default cost applied to all flatpacks outside of the cost of constructing the machine.
    /// This one is applied to machines specifically.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<MaterialPrototype>, int> BaseMachineCost = new();

    /// <summary>
    /// A default cost applied to all flatpacks outside of the cost of constructing the machine.
    /// This one is applied to computers specifically.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<MaterialPrototype>, int> BaseComputerCost = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string SlotId = "board_slot";

    // DEN: Part upgrades.
    /// <summary>
    /// The part that influences the flatpacker's speed.
    /// </summary>
    [DataField]
    public string MachinePartPrintSpeed = "Manipulator";

    /// <summary>
    /// Controls how much of an impact an upgraded part has on the print time.
    /// </summary>
    [DataField] public float PartRatingPrintTimeMultiplier = 0.5f;
    
    /// <summary>
    /// The part that influences the flatpacker's material use.
    /// </summary>
    [DataField]
    public string MachinePartMaterialUse = "MatterBin";

    /// <summary>
    /// Controls how much of an impact an upgraded part has on the material cost.
    /// </summary>
    [DataField] public float PartRatingMaterialUseMultiplier = 1.0f;
    
    /// <summary>
    /// The multiplier for the amount of time the packing takes with upgrades.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float FinalTimeMultiplier = 1.0f;
    
    /// <summary>
    /// The multiplier for the amount of material the packing takes with upgrades.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float FinalMaterialUseMultiplier = 1.0f;
    // DEN end
}

[Serializable, NetSerializable]
public enum FlatpackCreatorUIKey : byte
{
    Key
}

[Serializable, NetSerializable]
public enum FlatpackCreatorVisuals : byte
{
    Packing
}

[Serializable, NetSerializable]
public sealed class FlatpackCreatorStartPackBuiMessage : BoundUserInterfaceMessage
{

}
