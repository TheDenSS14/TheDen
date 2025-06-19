# SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 2021 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
# SPDX-FileCopyrightText: 2021 mirrorcult <lunarautomaton6@gmail.com>
# SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

### Solution transfer component

comp-solution-transfer-fill-normal = You fill {THE($target)} with {$amount}u from {THE($owner)}.
comp-solution-transfer-fill-fully = You fill {THE($target)} to the brim with {$amount}u from {THE($owner)}.
comp-solution-transfer-transfer-solution = You transfer {$amount}u to {THE($target)}.

## Displayed when trying to transfer to a solution, but either the giver is empty or the taker is full
comp-solution-transfer-is-empty = {THE($target)} is empty!
comp-solution-transfer-is-full = {THE($target)} is full!

## Displayed in change transfer amount verb's name
comp-solution-transfer-verb-custom-amount = Custom
comp-solution-transfer-verb-amount = {$amount}u
comp-solution-transfer-verb-toggle = Toggle to {$amount}u

## Displayed after you successfully change a solution's amount using the BUI
comp-solution-transfer-set-amount = Transfer amount set to {$amount}u.
