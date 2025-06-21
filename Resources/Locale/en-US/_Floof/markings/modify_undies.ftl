# SPDX-FileCopyrightText: 2025 Aikakakah <145503852+Aikakakah@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

verb-categories-undies = Modify Undies

modify-undies-verb-text = {$isVisible ->
*[false] Put on
[true] Take off
} {$isMine ->
*[false] {$target}'s
[true] your
} {$undies}

undies-removed-self-start = You start taking off your {$undie}.
undies-equipped-self-start = You start putting on your {$undie}.

undies-removed-user-start = You start taking their {$undie} off.
undies-equipped-user-start = You start putting their {$undie} on them.

undies-removed-target-start = {PROPER($user)} starts taking your {$undie} off.
undies-equipped-target-start = {PROPER($user)} starts putting your {$undie} on you.

undies-removed-self = You take off your {$undie}.
undies-equipped-self = You put on your {$undie}.

undies-removed-user = You take their {$undie} off.
undies-equipped-user = You put their {$undie} on them.

undies-removed-target = {PROPER($user)} takes your {$undie} off.
undies-equipped-target = {PROPER($user)} puts your {$undie} on you.
