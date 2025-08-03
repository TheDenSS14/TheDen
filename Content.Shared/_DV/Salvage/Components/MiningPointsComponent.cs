// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.Salvage.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._DV.Salvage.Components;

/// <summary>
/// Stores mining points for a holder, such as an ID card or ore processor.
/// Mining points are gained by smelting ore and redeeming them to your ID card.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(MiningPointsSystem))]
[AutoGenerateComponentState]
public sealed partial class MiningPointsComponent : Component
{
    /// <summary>
    /// The number of points stored.
    /// </summary>
    [DataField, AutoNetworkedField]
    public uint Points;

    /// <summary>
    /// Sound played when successfully transferring points to another holder.
    /// </summary>
    [DataField]
    public SoundSpecifier? TransferSound;
}
