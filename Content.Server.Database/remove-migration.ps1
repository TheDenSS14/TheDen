# SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

#!/usr/bin/env pwsh

param([String]$name)

if ($name -eq "")
{
    Write-Error "must specify migration name"
    exit
}

dotnet ef migrations remove --context SqliteServerDbContext $name
dotnet ef migrations remove --context PostgresServerDbContext $name
