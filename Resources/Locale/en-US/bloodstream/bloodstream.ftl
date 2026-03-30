# SPDX-FileCopyrightText: 2022 Kara
# SPDX-FileCopyrightText: 2022 mirrorcult
# SPDX-FileCopyrightText: 2024 Angelo Fallaria
# SPDX-FileCopyrightText: 2025 sleepyyapril
# SPDX-FileCopyrightText: 2026 pocl.v
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

bloodstream-component-looks-pale = [color=bisque]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BASIC($target, "look", "looks")} pale.[/color]
# imp/den edit, better health examine
bloodstream-component-slight-bleeding = [color=#ffa8a8]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} bleeding a little.[/color]
bloodstream-component-bleeding = [color=red]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} bleeding.[/color]
bloodstream-component-profusely-bleeding = [color=crimson]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} profusely bleeding![/color]
# imp/den edit, better health examine
bloodstream-component-massive-bleeding = [color=#d4003c]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} hemorrhaging blood![/color]

bloodstream-component-wounds-cauterized = You feel your wounds painfully close!

bloodstream-component-selfaware-looks-pale = [color=bisque]You feel dizzy from blood loss.[/color]
# den edit, better health examine
bloodstream-component-selfaware-slight-bleeding = [color=#ffa8a8]You are bleeding a little.[/color]
bloodstream-component-selfaware-bleeding = [color=red]You are bleeding.[/color]
bloodstream-component-selfaware-profusely-bleeding = [color=crimson]You are profusely bleeding![/color]
# den edit, better health examine
bloodstream-component-selfaware-massive-bleeding = [color=#d4003c]{CAPITALIZE(SUBJECT($target))} {CONJUGATE-BE($target)} hemorrhaging blood![/color]
