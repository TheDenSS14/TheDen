// SPDX-FileCopyrightText: 2024 12rabbits <53499656+12rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.EUI;
using Content.Shared.ReadyManifest;

namespace Content.Server.ReadyManifest;

public sealed class ReadyManifestEui : BaseEui
{
    private readonly ReadyManifestSystem _readyManifest;

    /// <summary>
    ///     Current owner of this UI, if it has one. This is
    ///     to ensure that if a BUI is closed, the EUIs related
    ///     to the BUI are closed as well.
    /// </summary>
    public readonly EntityUid? Owner;

    public ReadyManifestEui(EntityUid? owner, ReadyManifestSystem readyManifestSystem)
    {
        Owner = owner;
        _readyManifest = readyManifestSystem;
    }

    public override ReadyManifestEuiState GetNewState()
    {
        var entries = _readyManifest.GetReadyManifest();
        return new(entries);
    }

    public override void Closed()
    {
        base.Closed();

        _readyManifest.CloseEui(Player, Owner);
    }
}