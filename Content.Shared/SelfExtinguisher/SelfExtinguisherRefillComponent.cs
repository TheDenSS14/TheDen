// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.SelfExtinguisher;

/// <summary>
///     Used to refill the charges of self-extinguishers.
/// </summary>
[RegisterComponent]
public sealed partial class SelfExtinguisherRefillComponent : Component
{
    // <summary>
    //   The amount of charges to refill.
    // </summary>
    [DataField(required: true)]
    public int RefillAmount;
}
