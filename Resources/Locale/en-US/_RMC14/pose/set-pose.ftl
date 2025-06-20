# SPDX-FileCopyrightText: 2025 MajorMoth <61519600+MajorMoth@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

rmc-set-pose-examined = [color=lightblue][bold]{ CAPITALIZE(SUBJECT($ent)) } { CONJUGATE-BE($ent) } {$pose}[/bold][/color]
rmc-set-pose-dialog = This is {$ent}. { CAPITALIZE(SUBJECT($ent)) } { CONJUGATE-BE($ent) }...
rmc-set-pose-title = Set Pose
