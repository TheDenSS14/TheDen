# SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

cage-resist-second-person = You start removing your {$cage}.
cage-resist-third-person = {CAPITALIZE(THE($user))} starts removing {POSS-ADJ($user)} {$cage}.

cage-uncage-verb = Uncage

action-name-metapsionic = Metapsionic Pulse
action-description-metapsionic = Send a mental pulse through the area to see if there are any psychics nearby.

metapsionic-pulse-success = You detect psychic presence nearby.
metapsionic-pulse-failure = You don't detect any psychic presence nearby.
metapsionic-pulse-power = You detect that {$power} was used nearby.

action-name-dispel = Dispel
action-description-dispel = Dispel summoned entities such as familiars or forcewalls.

action-name-mass-sleep = Mass Sleep
action-description-mass-sleep = Put targets in a small area to sleep.

accept-psionics-window-title = Psionic!
accept-psionics-window-prompt-text-part = You rolled a psionic power!
                                         It's possible that certain anti-psychic forces may hunt you,
                                         so you should consider keeping it secret.
                                         Do you still wish to be psionic?

action-name-psionic-invisibility = Psionic Invisibility
action-description-psionic-invisibility = Render yourself invisible to any entity that could potentially be psychic. Borgs, animals, and so on are not affected.

action-name-psionic-invisibility-off = Turn Off Psionic Invisibility
action-description-psionic-invisibility-off = Return to visibility, and receive a stun.

action-name-mind-swap = Mind Swap
action-description-mind-swap = Swap minds with the target. Either can change back after 20 seconds.

action-name-mind-swap-return = Reverse Mind Swap
action-description-mind-swap-return = Return to your original body.

action-name-telegnosis = Telegnosis
action-description-telegnosis = Create a telegnostic projection to remotely observe things.

action-name-psionic-regeneration = Psionic Regeneration
action-description-psionic-regeneration = Push your natural metabolism to the limit to power your body's regenerative capability.

glimmer-report = Current Glimmer Level: {$level}Ψ.
glimmer-event-report-generic = Noöspheric discharge detected. Glimmer level has decreased by {$decrease} to {$level}Ψ.
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
action-description-assay = Probe an entity at close range to glean metaphorical information about any powers they may have

#DeltaV Precognition text
psionic-power-precognition-failure-by-damage = Your concentration was broken! You fail to decipher anything of use.
psionic-power-precognition-no-event-result-message = You see a vision of an undisturbed lake.

psionic-power-precognition-xeno-vents-result-message = You see a vision, peering around a corner you see a strange, squelching beast tear at the insides of one of your coworkers as they let out a bloodcurdling scream.
psionic-power-precognition-mothroach-spawn-result-message = You see a hungering mass of newborns, chittering and screeching as they sink their mandibles into the station's knitwork.
psionic-power-precognition-listening-post-result-message = you hear a cacophony of foreign voices speaking over radio static; you wake with a creeping sense of paranoia.
psionic-power-precognition-paradox-anomaly-result-message = You see your coworker splitting in two. Where there was one, there are now two. You are unsure which of the beings is truly your coworker.
# psionic-power-precognition-fugitive-result-message = You see hounds around every corner all hunting for someone who does not belong.
# psionic-power-precognition-syndicate-recruiter-result-message = You see someone cutting ties on a chain-link fence and reforging its now disparate parts under a new oath of blood.
# psionic-power-precognition-synthesis-specialist-result-message = You smell a dangerous mixture of chemicals in the air; the distant sound of a small plasma engine roars to life.
psionic-power-precognition-cargo-gifts-base-result-message = You see a vision of yourself, gathered for a time-old tradition of receiving gifts. You didn't ask for these, but you must pretend to appreciate nonetheless.
psionic-power-precognition-anomoly-spawn-result-message = You attempt to look forward, but the future is distorted by a blast of noöspheric energies converging on a single point.
psionic-power-precognition-bluespace-artifact-result-message = You attempt to look forward, but are blinded by noöspheric energies coalescing into an object beyond comprehension.
psionic-power-precognition-bluespace-locker-result-message = You attempt to look forward, but the noösphere seems distorted by a constantly shifting bluespace energy.
psionic-power-precognition-breaker-flip-result-message = You see torches snuff around you and keepers rekindling the lost flames.
psionic-power-precognition-bureaucratic-error-result-message = You see a vision of yourself trapped in a room, trying to solve a puzzle with both missing and duplicate pieces.
psionic-power-precognition-clerical-error-result-message = You see faces you once knew being obscured in a fog of static, identities lost.
psionic-power-precognition-closet-skeleton-result-message = You hear a crackling laugh echo and clinking bones in the dusty recesses of the station.
psionic-power-precognition-dragon-spawn-result-message = Reality around you bulges and breaks as a great beast cries for war. The smell of salty sea and blood fills the air.
psionic-power-precognition-ninja-spawn-result-message = You see a vision of shadows brought to life, hounds of war howling their cries as they chase it through dark corners of the station.
psionic-power-precognition-revenant-spawn-result-message = The shadows around you grow threefold taller and threefold darker. Something lurks within them, a predator stalking you in the darkness. 
psionic-power-precognition-gas-leak-result-message = For but a moment, it feels as if you cannot breathe. With a blink, everything returns to normal.
psionic-power-precognition-kudzu-growth-result-message = Leaves and vines pierce through the dusty tiles of the station, crawling about your ankles, trying to drag you down with them.
psionic-power-precognition-power-grid-check-result-message = You see torches snuff around you only to spontaneously ignite moments later.
psionic-power-precognition-solar-flare-result-message = The stars look beautiful tonight, shrinking and growing and shooting great bolts like fireworks into the sky.
psionic-power-precognition-vent-clog-result-message = You smell something horrific on the artificial breeze of the station, for a moment your eyes fill with fog. When you blink it away, the smell is gone.
psionic-power-precognition-slimes-spawn-result-message = Something lurks deep within the darkest corners of the station, crying for blood. Soft squelches and bubbling howls accompany the call.
psionic-power-precognition-snake-spawn-result-message = Something lurks deep within the darkest corners of the station, crying for blood. The sounds of hissing growls accompany the call. 
psionic-power-precognition-spider-spawn-result-message = Something lurks deep within the darkest corners of the station, crying for blood. A symphony of clicks and chitters accompanies the call.
psionic-power-precognition-spider-clown-spawn-result-message = Something lurks deep within the darkest corners of the station, crying for blood. An unholy mass of honks accompanies the call.
psionic-power-precognition-zombie-outbreak-result-message = Your coworker lies on the cold ground before you; skull ripped open, eyes blank. You think you see the body twitch.
psionic-power-precognition-lone-ops-spawn-result-message = You see a vision of a beast with a blood-red carapace, laughing as it eats through the station, bite by bite.
psionic-power-precognition-sleeper-agents-result-message = You see a vision of life through the eyes of a non-descript coworker, a soft but dangerous buzzing accompanies you at the base of their skull. It sounds like radio static.
psionic-power-precognition-mass-hallucinations-result-message = You attempt to see a vision of the future, but all you see is a phantasmagoria of chaotic shapes.
psionic-power-precognition-ion-storm-result-message = You see a vision of the rigid being destroyed and reshaped into something new and wrong.
psionic-power-precognition-meteor-swarm-result-message = You see a fiery vision of shooting stars falling from the sky, colorful trails shooting through the station you call home.
# psionic-power-precognition-game-rule-urist-swarm-result-message = You see a fiery vision of... PEOPLE falling from the sky! colorful trails shooting through the station you call home.
# psionic-power-precognition-immovable-rod-spawn-result-message = You see a fiery vision of a MASSIVE star falling from the sky! colorful trails shooting through the station you call home.
psionic-power-precognition-mouse-migration-result-message = You see a vision of living as a simplistic creature, scurrying underfoot of creatures beyond your comprehension.
psionic-power-precognition-king-rat-migration-result-message = You see a vision of living as a simplistic creature, scurrying underfoot of creatures beyond your comprehension.
psionic-power-precognition-cockroach-migration-result-message = You see a vision of living as a simplistic creature, scurrying underfoot of creatures beyond your comprehension.
psionic-power-precognition-snail-migration-result-message = You see a vision of living as a simplistic creature, scurrying underfoot of creatures beyond your comprehension.
psionic-power-precognition-random-sentience-result-message = Something bright and beautiful sparks to life within your third eye. Nothing brings wonder quite like new life.
psionic-power-precognition-unknown-shuttle-cargo-lost-result-message = You see a vision of a simple ship of old Terra, adrift of the sea, far away from home.
psionic-power-precognition-unknown-shuttle-traveling-cuisine-result-message = You see a vision of peace, a cozy meal sizzling on a warm stove. A delicious smells wafts through the air.
psionic-power-precognition-unknown-shuttle-disaster-evac-pod-result-message = You see a vision of death and blood, of a destruction so complete only few survive, drifting through the coldness of space.

#TheDen Addition
psionic-power-precognition-mimic-vendor-result-message = Your eyes are blinded by blinking lights, ears scorned by robotic voices. Yet, oddly, you feel refreshed.
psionic-power-precognition-derelict-cyborg-result-message = You see a vision of rusted steel, long abandoned by it's creator.