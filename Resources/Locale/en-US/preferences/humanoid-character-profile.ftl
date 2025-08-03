# SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 2021 Remie Richards <remierichards@gmail.com>
# SPDX-FileCopyrightText: 2025 Azzy <azzy@azzy.info>
# SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

### UI

# Displayed in the Character prefs window
humanoid-character-profile-summary = 
    This is {$name}. {$gender ->
    [male] He is
    [female] She is
    [neuter] It is
    *[other] They are
} {$age} years old.
