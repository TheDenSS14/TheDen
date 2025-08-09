# SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

command-glimmershow-description = Show the current glimmer level.
command-glimmershow-help = No arguments.

command-glimmerset-description = Set glimmer to a number.
command-glimmerset-help = glimmerset (integer)

cmd-lspsionics-desc = List psionics.
cmd-lspsionics-help = No arguments.

cmds-psionicpower-entityuid = <EntityUid>

cmd-addpsionicpower-desc = Initialize an entity as Psionic with a given PowerPrototype
cmd-addpsionicpower-help = Argument 1 must be an EntityUid, and argument 2 must be a string matching the PrototypeId of a PsionicPower.
addpsionicpower-args-one-error = Argument 1 must be an EntityUid
addpsionicpower-args-two-error = Argument 2 must match the PrototypeId of a PsionicPower

cmd-addrandompsionicpower-desc = Initialize an entity as Psionic with a random PowerPrototype that is available for that entity to roll.
cmd-addrandompsionicpower-help = Argument 1 must be an EntityUid.
addrandompsionicpower-args-one-error = Argument 1 must be an EntityUid

cmd-removepsionicpower-desc = Remove a Psionic power from an entity.
cmd-removepsionicpower-help = Argument 1 must be an EntityUid, and argument 2 must be a string matching the PrototypeId of a PsionicPower.
removepsionicpower-args-one-error = Argument 1 must be an EntityUid
removepsionicpower-args-two-error = Argument 2 must match the PrototypeId of a PsionicPower.
removepsionicpower-not-psionic-error = The target entity is not Psionic.
removepsionicpower-not-contains-error = The target entity does not have this PsionicPower.

cmd-removeallpsionicpowers-desc = Remove all Psionic powers from an entity.
cmd-removeallpsionicpowers-help = Argument 1 must be an EntityUid.
removeallpsionicpowers-args-one-error = Argument 1 must be an EntityUid.
removeallpsionicpowers-not-psionic-error = The target entity is not Psionic.
