// SPDX-FileCopyrightText: 2024 Fansana <116083121+Fansana@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Security;

namespace Content.Server.CartridgeLoader.Cartridges;

[RegisterComponent, Access(typeof(SecWatchCartridgeSystem))]
public sealed partial class SecWatchCartridgeComponent : Component
{
    /// <summary>
    /// Only show people with these statuses.
    /// </summary>
    [DataField]
    public List<SecurityStatus> Statuses = new()
    {
        SecurityStatus.Suspected,
        SecurityStatus.Wanted
    };

    /// <summary>
    /// Station entity thats getting its records checked.
    /// </summary>
    [DataField]
    public EntityUid? Station;
}
