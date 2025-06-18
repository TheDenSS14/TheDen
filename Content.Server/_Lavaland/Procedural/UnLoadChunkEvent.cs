// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._Lavaland.Procedural;

/// <summary>
/// Lavaland: Raised when biome chunk is about to unload.
/// </summary>
public sealed class UnLoadChunkEvent : CancellableEntityEventArgs
{
    public Vector2i Chunk { get; set; }

    public UnLoadChunkEvent(Vector2i chunk)
    {
        Chunk = chunk;
    }
}
