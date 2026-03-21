# SPDX-FileCopyrightText: 2023 Doru991
# SPDX-FileCopyrightText: 2024 Remuchi
# SPDX-FileCopyrightText: 2025 sleepyyapril
# SPDX-FileCopyrightText: 2026 pocl.v
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

health-examinable-silicon-none = [color=green]There is no obvious damage to be seen.[/color]

# wizden edit, better health examine, changed 25 -> 15, 75 -> 100
health-examinable-silicon-Blunt-15 = [color=red]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } minor dents on { POSS-ADJ($target) } chassis.[/color]
health-examinable-silicon-Blunt-50 = [color=crimson]{ CAPITALIZE(POSS-ADJ($target)) } chassis is severely dented![/color]
health-examinable-silicon-Blunt-100 = [color=crimson]{ CAPITALIZE(POSS-ADJ($target)) } chassis is almost completely caved in![/color]

# wizden edit, better health examine, changed 10 -> 8, 25 -> 30, 50 -> 75, 75 -> 100
health-examinable-silicon-Slash-8 = [color=red]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } some minor scratches.[/color]
health-examinable-silicon-Slash-30 = [color=red]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } significant scratches on { POSS-ADJ($target) } chassis.[/color]
health-examinable-silicon-Slash-75 = [color=crimson]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } major gashes across { POSS-ADJ($target) } plating![/color]
health-examinable-silicon-Slash-100 = [color=crimson]{ CAPITALIZE(POSS-ADJ($target)) } chassis is torn up![/color]

health-examinable-silicon-Piercing-50 = [color=crimson]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } large holes all over { POSS-ADJ($target) } chassis![/color]

# wizden edit, better health examine, changed 25 - > 15
health-examinable-silicon-Heat-15 = [color=orange]{ CAPITALIZE(SUBJECT($target)) } { CONJUGATE-HAVE($target) } superficial burns across { POSS-ADJ($target) } chassis.[/color]
health-examinable-silicon-Heat-50 = [color=orange]{ CAPITALIZE(POSS-ADJ($target)) } chassis is significantly charred.[/color]
health-examinable-silicon-Heat-75 = [color=orange]{ CAPITALIZE(POSS-ADJ($target)) } chassis is partially melted![/color]

health-examinable-silicon-Shock-50 = [color=lightgoldenrodyellow]{ CAPITALIZE(POSS-ADJ($target)) } circuits seem partially fried![/color]
