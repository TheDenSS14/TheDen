// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2024 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Falcon <falcon@zigtag.dev>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Collections.Generic;
using System.Reflection;
using Content.Server.Administration.Managers;
using Robust.Shared.Toolshed;

namespace Content.IntegrationTests.Tests.Toolshed;

[TestFixture]
[Ignore("Broken in 160.x for unknown reasons")]
public sealed class AdminTest : ToolshedTest
{
    [Test]
    public async Task AllCommandsHavePermissions()
    {
        var toolMan = Server.ResolveDependency<ToolshedManager>();
        var admin = Server.ResolveDependency<IAdminManager>();
        var ignored = new HashSet<Assembly>()
            {typeof(LocTest).Assembly, typeof(Robust.UnitTesting.Shared.Toolshed.LocTest).Assembly};

        await Server.WaitAssertion(() =>
        {
            Assert.Multiple(() =>
            {
                foreach (var cmd in toolMan.DefaultEnvironment.AllCommands())
                {
                    if (ignored.Contains(cmd.Cmd.GetType().Assembly))
                        continue;

                    Assert.That(admin.TryGetCommandFlags(cmd, out _), $"Command does not have admin permissions set up: {cmd.FullName()}");
                }
            });
        });
    }
}
