// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._DEN.GameTicking.Rules;


/// <summary>
/// This is used for handling the high danger preset.
/// </summary>
[RegisterComponent]
public sealed partial class LowDangerRuleComponent : Component, IFakePreset
{
    [DataField]
    public HashSet<EntityUid> AdditionalGameRules { get; set; } = new();
}
