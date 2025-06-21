# SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

altar-examine = [color=purple]This altar can be used to sacrifice Psionics.[/color]
altar-sacrifice-verb = Sacrifice

altar-failure-reason-self = You can't sacrifice yourself!
altar-failure-reason-user = You are not psionic or clerically trained!
altar-failure-reason-user-humanoid = You are not a humanoid!
altar-failure-reason-target = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} not psionic!
altar-failure-reason-target-humanoid = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} not a humanoid!
altar-failure-reason-target-catatonic = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} braindead!

altar-sacrifice-popup = {$user} starts to sacrifice {$target}!
