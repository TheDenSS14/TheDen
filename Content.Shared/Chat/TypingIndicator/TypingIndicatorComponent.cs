// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Morb
// SPDX-FileCopyrightText: 2024 LankLTE
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2026 MajorMoth
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     Show typing indicator icon when player typing text in chat box.
///     Added automatically when player poses entity.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TypingIndicatorComponent : Component
{
    /// <summary>
    ///     Prototype ID of the original typing indicator the entity is using set in yml.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("proto", customTypeSerializer: typeof(PrototypeIdSerializer<TypingIndicatorPrototype>)), AutoNetworkedField]
    public string StartingPrototype = SharedTypingIndicatorSystem.InitialIndicatorId;

    /// <summary>
    ///     Prototype id that store all visual info about typing indicator, set to the value of StartingPrototype on component init.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public string Prototype = SharedTypingIndicatorSystem.InitialIndicatorId;
}
