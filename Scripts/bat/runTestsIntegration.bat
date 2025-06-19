REM SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
REM SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
REM
REM SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

cd ..\..\

mkdir Scripts\logs

del Scripts\logs\Content.IntegrationTests.log
dotnet test Content.IntegrationTests/Content.IntegrationTests.csproj -c DebugOpt -- NUnit.ConsoleOut=0 NUnit.MapWarningTo=Failed > Scripts\logs\Content.IntegrationTests.log

pause
