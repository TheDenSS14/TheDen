// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Silicons.Bots;

[RegisterComponent]
[Access(typeof(FillbotSystem))]
public sealed partial class FillbotComponent : Component
{
    [ViewVariables]
    public EntityUid? LinkedSinkEntity { get; set; }
}
