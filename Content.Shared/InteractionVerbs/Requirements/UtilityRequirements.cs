// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Random;
using Robust.Shared.Serialization;

namespace Content.Shared.InteractionVerbs.Requirements;

[Serializable, NetSerializable]
public sealed partial class ChanceRequirement : InteractionRequirement
{
    [DataField(required: true)]
    public float Chance;

    public override bool IsMet(InteractionArgs args, InteractionVerbPrototype proto, InteractionAction.VerbDependencies deps)
    {
        return Chance > 0f && (Chance > 1f || deps.Random.Prob(Chance));
    }
}
