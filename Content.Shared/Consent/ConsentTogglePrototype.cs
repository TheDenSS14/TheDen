// SPDX-FileCopyrightText: 2024 Pierson Arnold <greyalphawolf7@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Consent;

using Robust.Shared.Prototypes;

/// <summary>
/// TODO
/// </summary>
[Prototype("consentToggle")]
public sealed partial class ConsentTogglePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;
}
