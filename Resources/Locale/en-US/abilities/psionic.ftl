# SPDX-FileCopyrightText: 2023 PHCodes
# SPDX-FileCopyrightText: 2024 ShatteredSwords
# SPDX-FileCopyrightText: 2025 VMSolidus
# SPDX-FileCopyrightText: 2025 sleepyyapril
#
# SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

# TheDen - Standardizing "psion" to mean people, and "psionic" to be an adjective

cage-resist-second-person = You start removing your {$cage}.
cage-resist-third-person = {CAPITALIZE(THE($user))} starts removing {POSS-ADJ($user)} {$cage}.

cage-uncage-verb = Uncage

action-name-metapsionic = Metapsionic Pulse
action-description-metapsionic = Send a mental pulse through the area to see if there are any psions nearby.

metapsionic-pulse-success = You detect psionic presence nearby.
metapsionic-pulse-failure = You don't detect any psionic presence nearby.
metapsionic-pulse-power = You detect that {$power} was used nearby.

action-name-dispel = Dispel
action-description-dispel = Dispel summoned entities such as familiars or forcewalls.

action-name-mass-sleep = Mass Sleep
action-description-mass-sleep = Put targets in a small area to sleep.

accept-psionics-window-title = Psionic!
accept-psionics-window-prompt-text-part = You've gained a psionic ability!
                                          Your mind opens to the noosphere.
                                          Thoughts hum like distant stars.
                                          The air feels charged with meaning.

action-name-psionic-invisibility = Psionic Invisibility
action-description-psionic-invisibility = Render yourself invisible to any entity that could potentially be psionic. Borgs, animals, and so on are not affected.

action-name-psionic-invisibility-off = Disable Psionic Invisibility
action-description-psionic-invisibility-off = Make yourself visible again, though it will stun you.

action-name-mind-swap = Mind Swap
action-description-mind-swap = Swap minds with the target. Either of you can reverse the effects after twenty seconds.

action-name-mind-swap-return = Reverse Mind Swap
action-description-mind-swap-return = Return to your original body.

action-name-telegnosis = Telegnosis
action-description-telegnosis = Create a telegnostic projection to remotely observe things.

action-name-psionic-regeneration = Psionic Regeneration
action-description-psionic-regeneration = Push your natural metabolism to the limit to power your body's regenerative capability.

glimmer-report = Current Glimmer Level: {$level}Ψ.
glimmer-event-report-generic = Noospheric discharge detected. Glimmer level has decreased by {$decrease} to {$level}Ψ.
glimmer-event-report-signatures = New psionic signatures manifested. Glimmer level has decreased by {$decrease} to {$level}Ψ.
glimmer-event-awakened-prefix = awakened {$entity}

noospheric-zap-seize = You seize up!
noospheric-zap-seize-potential-regained = You seize up! Some mental block seems to have cleared, too.

mindswap-trapped = Seems you're trapped in this vessel.

telegnostic-trapped-entity-name = severed telegnostic projection
telegnostic-trapped-entity-desc = Its many eyes betray sadness.

psionic-burns-up = {CAPITALIZE(THE($item))} burns up with arcs of strange energy!
psionic-burn-resist = Strange arcs dance across {THE($item)}!

action-name-noospheric-zap = Noospheric Zap
action-description-noospheric-zap = Shocks the conciousness of the target and leaves them stunned and stuttering.

action-name-pyrokinesis = Pyrokinesis
action-description-pyrokinesis = Light a flammable target on fire.
pyrokinesis-power-used = A wisp of flame engulfs {THE($target)}, igniting {OBJECT($target)}!

action-name-psychokinesis = Psychokinesis
action-description-psychokinesis = Bend the fabric of space to instantly move across it.

action-name-rf-sensitivity = Toggle RF Sensitivity
action-desc-rf-sensitivity = Toggle your ability to interpret radio waves on and off.

action-name-assay = Assay
action-description-assay = Probe an entity at close range to glean enigmatic information about any powers they may have.
