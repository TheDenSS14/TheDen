// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <zachcaffee@outlook.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Collections.Generic;
using System.Linq;
using Content.Shared.Announcements.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Announcers;

/// <summary>
///     Checks if every announcer has a fallback announcement
/// </summary>
[TestFixture]
[TestOf(typeof(AnnouncerPrototype))]
public sealed class AnnouncerPrototypeTest
{
    /// <inheritdoc cref="AnnouncerPrototypeTest"/>
    [Test]
    public async Task TestAnnouncerFallbacks()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var prototype = server.ResolveDependency<IPrototypeManager>();

        await server.WaitAssertion(() =>
        {
            var success = true;
            var why = new List<string>();

            foreach (var announcer in prototype.EnumeratePrototypes<AnnouncerPrototype>().OrderBy(a => a.ID))
            {
                if (announcer.Announcements.Any(a => a.ID.ToLower() == "fallback"))
                    continue;

                success = false;
                why.Add(announcer.ID);
            }

            Assert.That(success, Is.True, $"The following announcers do not have a fallback announcement:\n  {string.Join("\n  ", why)}");
        });

        await pair.CleanReturnAsync();
    }
}
