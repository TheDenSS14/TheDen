// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Bed.Cryostorage.Components;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Robust.Shared.Configuration;


namespace Content.Client._DEN.Customization.Systems;


/// <summary>
/// Handles the user toggling whether they would like to see Cryo messages or not.
/// </summary>
public sealed class IgnoreCryoMessagesSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public override void Initialize()
    {
        Subs.CVar(_cfg, CCVars.IgnoreCryoMessage, SetIgnoreCryoMessage, true);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnEvent);
    }

    private void OnPlayerSpawnEvent(PlayerSpawnCompleteEvent ev)
    {
        // We joined a match, make sure the server knows our preference so it can attach it to our character.
        RaiseNetworkEvent(new IgnoreCryoMessage(_cfg.GetCVar(CCVars.IgnoreCryoMessage)));
    }

    private void SetIgnoreCryoMessage(bool state)
    {
        // Server will ignore this if a round isn't started or we dont have a character
        // but we send it again at round start anyway.
        // If a round is in progress, then this serves to update the config live.
        RaiseNetworkEvent(new IgnoreCryoMessage(state));
    }
}
