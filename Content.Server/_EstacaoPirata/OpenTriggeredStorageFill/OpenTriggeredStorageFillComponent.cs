// SPDX-FileCopyrightText: 2025 RadsammyT <32146976+RadsammyT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server._EstacaoPirata.OpenTriggeredStorageFill;

/// <summary>
/// This is used for storing an item prototype to be inserted into a container when the trigger is activated. This is deleted from the entity after the item is inserted.
/// </summary>
[RegisterComponent]
public sealed partial class OpenTriggeredStorageFillComponent : Component
{
    [DataField]
    public List<EntitySpawnEntry> Contents = new();
}
