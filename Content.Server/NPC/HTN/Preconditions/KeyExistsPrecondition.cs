// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.HTN.Preconditions;

public sealed partial class KeyExistsPrecondition : HTNPrecondition
{
    [DataField("key", required: true)] public string Key = string.Empty;

    public override bool IsMet(NPCBlackboard blackboard)
    {
        return blackboard.ContainsKey(Key);
    }
}
