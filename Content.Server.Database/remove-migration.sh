# SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

#!/usr/bin/env bash

if [ -z "$1" ] ; then
    echo "Must specify migration name"
    exit 1
fi

dotnet ef migrations remove --context SqliteServerDbContext -o Migrations/Sqlite "$1"
dotnet ef migrations remove --context PostgresServerDbContext -o Migrations/Postgres "$1"
