# SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
# SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 2023 metalgearsloth <comedian_vs_clown@hotmail.com>
# SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

action-name-wake = Wake up

sleep-onomatopoeia = Zzz...
sleep-examined = [color=lightblue]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} asleep.[/color]

wake-other-success = You shake {THE($target)} awake.
wake-other-failure = You shake {THE($target)}, but {SUBJECT($target)} {CONJUGATE-BE($target)} not waking up.

popup-sleep-in-bag = {THE($entity)} curls up and falls asleep.
