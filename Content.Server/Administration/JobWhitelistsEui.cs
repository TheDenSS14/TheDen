// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Server.Players.JobWhitelist;
using Content.Shared.Administration;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Content.Shared.Roles;
using Robust.Shared.Log;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration;

public sealed class JobWhitelistsEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly JobWhitelistManager _jobWhitelist = default!;

    private readonly ISawmill _sawmill;

    public NetUserId PlayerId;
    public string PlayerName;

    public HashSet<ProtoId<JobPrototype>> Whitelists = new();

    public JobWhitelistsEui(NetUserId playerId, string playerName)
    {
        IoCManager.InjectDependencies(this);

        _sawmill = _log.GetSawmill("admin.job_whitelists_eui");

        PlayerId = playerId;
        PlayerName = playerName;
    }

    public async void LoadWhitelists()
    {
        var jobs = await _db.GetJobWhitelists(PlayerId.UserId);
        foreach (var id in jobs)
        {
            if (_proto.HasIndex<JobPrototype>(id))
                Whitelists.Add(id);
        }

        StateDirty();
    }

    public override EuiStateBase GetNewState()
    {
        return new JobWhitelistsEuiState(PlayerName, Whitelists);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (msg is not SetJobWhitelistedMessage args)
            return;

        if (!_admin.HasAdminFlag(Player, AdminFlags.Whitelist))
        {
            _sawmill.Warning($"{Player.Name} ({Player.UserId}) tried to change role whitelists for {PlayerName} without whitelists flag");
            return;
        }

        if (!_proto.HasIndex<JobPrototype>(args.Job))
            return;

        if (args.Whitelisting)
        {
            _jobWhitelist.AddWhitelist(PlayerId, args.Job);
            Whitelists.Add(args.Job);
        }
        else
        {
            _jobWhitelist.RemoveWhitelist(PlayerId, args.Job);
            Whitelists.Remove(args.Job);
        }

        var verb = args.Whitelisting ? "added" : "removed";
        _sawmill.Info($"{Player.Name} ({Player.UserId}) {verb} whitelist for {args.Job} to player {PlayerName} ({PlayerId.UserId})");

        StateDirty();
    }
}
