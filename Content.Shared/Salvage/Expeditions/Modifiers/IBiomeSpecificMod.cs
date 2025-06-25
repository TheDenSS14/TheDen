// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Salvage.Expeditions.Modifiers;

public interface IBiomeSpecificMod : ISalvageMod
{
    /// <summary>
    /// Whitelist for biomes. If null then any biome is allowed.
    /// </summary>
    List<string>? Biomes { get; }
}
