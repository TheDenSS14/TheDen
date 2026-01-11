# SPDX-FileCopyrightText: 2024 VMSolidus
# SPDX-FileCopyrightText: 2025 Shaman
# SPDX-FileCopyrightText: 2025 sleepyyapril
#
# SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

# TheDen - What the fuck man
altar-examine = [color=purple]This altar can be used to sacrifice psions.[/color]
altar-sacrifice-verb = Sacrifice

altar-failure-reason-self = You can't sacrifice yourself!
altar-failure-reason-user = You are not psionic or clerically trained!
altar-failure-reason-user-humanoid = You are not a humanoid!
altar-failure-reason-target = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} not psionic!
altar-failure-reason-target-humanoid = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} not a humanoid!
altar-failure-reason-target-catatonic = {CAPITALIZE(THE($target))} {CONJUGATE-BE($target)} braindead!

altar-sacrifice-popup = {$user} starts to sacrifice {$target}!
