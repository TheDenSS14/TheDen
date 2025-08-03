// SPDX-FileCopyrightText: 2025 Sapphire
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Localization;
using Content.Shared.Humanoid.Markings;
using System.Linq;
using System.Collections.Generic;

namespace Content.IntegrationTests.Tests.Traits;

/// <summary>
///    Checks if every marking has a valid name localization string.
/// </summary>
[TestFixture]
[TestOf(typeof(MarkingPrototype))]
public sealed class MarkingLocalizationTest
{
    // These do not show layer locales, so layer locales do not matter.
    private static readonly HashSet<MarkingCategories> IgnoredLayerLocales =
    [
        MarkingCategories.Hair,
        MarkingCategories.FacialHair
    ];

    [Test]
    public async Task TestMarkingLocalization()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var locale = server.ResolveDependency<ILocalizationManager>();
        var proto = server.ResolveDependency<IPrototypeManager>();
        var marking = server.ResolveDependency<MarkingManager>();

        await server.WaitAssertion(() =>
        {
            var missingStrings = new List<string>();

            foreach (var markingProto in proto.EnumeratePrototypes<MarkingPrototype>().OrderBy(a => a.ID))
            {
                if (!locale.HasString($"marking-{markingProto.ID}") && string.IsNullOrEmpty(markingProto.Name))
                    missingStrings.Add($"\"{markingProto.ID}\", \"marking-{markingProto.ID}\"");

                // In these cases, layer names do not display anyway, so the layers do not need to be localized.
                if (markingProto.ForcedColoring
                    || IgnoredLayerLocales.Contains(markingProto.MarkingCategory))
                    continue;

                var layerStrings = marking.GetMarkingStateNames(markingProto, false);
                foreach (var layer in layerStrings)
                    if (!locale.HasString(layer) && !LayerIsLinked(markingProto, layer))
                        missingStrings.Add(layer);
            }

            Assert.That(!missingStrings.Any(), Is.True, $"The following markings are missing localization strings:\n  {string.Join("\n  ", missingStrings)}");
        });

        await pair.CleanReturnAsync();
    }

    // This function is a little gross but whatever
    private bool LayerIsLinked(MarkingPrototype marking, string layerKey)
    {
        if (marking.ColorLinks == null || marking.ColorLinks.Count == 0)
            return false;

        var markingPrefix = $"marking-{marking.ID}-";
        var layerId = layerKey[markingPrefix.Length..];
        return marking.ColorLinks.ContainsKey(layerId);
    }
}
