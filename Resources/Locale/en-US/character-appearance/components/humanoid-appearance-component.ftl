# SPDX-FileCopyrightText: 2022 och-och <80923370+och-och@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

humanoid-appearance-component-unknown-species = Person
humanoid-appearance-component-examine = { CAPITALIZE(SUBJECT($user)) } { CONJUGATE-BE($user) } { INDEFINITE($age) } { $age } { $species }.
humanoid-appearance-component-examine-pronouns = { CAPITALIZE(SUBJECT($user)) } also accepts {$pronouns} as pronouns.