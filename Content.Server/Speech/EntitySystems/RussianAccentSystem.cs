// SPDX-FileCopyrightText: 2022 Pancake <Pangogie@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Peptide90 <78795277+Peptide90@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Text;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class RussianAccentSystem : EntitySystem
{
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<RussianAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string Accentuate(string message)
    {
        var accentedMessage = new StringBuilder(_replacement.ApplyReplacements(message, "russian"));

        for (var i = 0; i < accentedMessage.Length; i++)
        {
            var c = accentedMessage[i];

            accentedMessage[i] = c switch
            {
                'A' => 'Д',
                'b' => 'в',
                'N' => 'И',
                'n' => 'и',
                'K' => 'К',
                'k' => 'к',
                'm' => 'м',
                'h' => 'н',
                't' => 'т',
                'R' => 'Я',
                'r' => 'я',
                'Y' => 'У',
                'W' => 'Ш',
                'w' => 'ш',
                _ => accentedMessage[i]
            };
        }

        return accentedMessage.ToString();
    }

    private void OnAccent(EntityUid uid, RussianAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message);
    }
}
