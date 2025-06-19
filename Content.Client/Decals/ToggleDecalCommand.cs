// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Console;
using Robust.Shared.GameObjects;

namespace Content.Client.Decals;

public sealed class ToggleDecalCommand : IConsoleCommand
{
    public string Command => "toggledecals";
    public string Description => "Toggles decaloverlay";
    public string Help => $"{Command}";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        EntitySystem.Get<DecalSystem>().ToggleOverlay();
    }
}
