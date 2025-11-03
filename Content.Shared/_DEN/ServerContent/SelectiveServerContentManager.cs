// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.CCVars;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Shared._DEN.ServerContent;

/// <summary>
/// This manager allows us to check prototypes against a certain server content ID.
/// This is used for selectively restricting prototypes on certain servers that share the same codebase.
/// </summary>
/// <remarks>
/// It primarily just adds the functionality to check the current server content ID; the mechanics to actually
/// selectively omit content is handled by prototypes and systems.
/// </remarks>
public sealed partial class ServerSelectiveContentManager
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public const string Salvation = "salvation";
    public const string Eternity = "eternity";

    public bool IsSalvation => IsServer(Salvation);
    public bool IsEternity => IsServer(Eternity);

    /// <summary>
    /// Checks if the current server matches a given string.
    /// </summary>
    public bool IsServer(string targetServer)
    {
        var serverId = _cfg.GetCVar(DenCCVars.ServerContentId);
        return serverId == targetServer;
    }

    /// <summary>
    /// Checks a prototype to see if it excludes the current server.
    /// </summary>
    /// <param name="id">The ID of the prototype to check.</param>
    /// <returns>Whether or not this prototype is allowed on this server.</returns>
    [PublicAPI]
    public bool IsServerContentAllowed<T>(ProtoId<T> id) where T : class, IPrototype, IServerSelectivePrototype
    {
        return _prototype.Resolve(id, out var proto)
            && IsServerContentAllowed(proto);
    }

    /// <summary>
    /// Checks a prototype to see if it excludes the current server.
    /// </summary>
    /// <param name="proto">The prototype to check.</param>
    /// <returns>Whether or not this prototype is allowed on this server.</returns>
    [PublicAPI]
    public bool IsServerContentAllowed<T>(T proto) where T : class, IPrototype, IServerSelectivePrototype
    {
        if (proto.ExcludedServers == null)
            return true;

        var serverId = _cfg.GetCVar(DenCCVars.ServerContentId);
        return string.IsNullOrEmpty(serverId)
            || !proto.ExcludedServers.Contains(serverId);
    }
}

public interface IServerSelectivePrototype
{
    HashSet<string>? ExcludedServers { get; }
}
