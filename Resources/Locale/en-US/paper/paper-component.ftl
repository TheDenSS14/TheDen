# SPDX-FileCopyrightText: 2021 20kdc
# SPDX-FileCopyrightText: 2021 DrSmugleaf
# SPDX-FileCopyrightText: 2021 Galactic Chimp
# SPDX-FileCopyrightText: 2022 Fishfish458
# SPDX-FileCopyrightText: 2022 fishfish458
# SPDX-FileCopyrightText: 2023 Eoin Mcloughlin
# SPDX-FileCopyrightText: 2023 eoineoineoin
# SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
# SPDX-FileCopyrightText: 2025 sleepyyapril
#
# SPDX-License-Identifier: MIT


### UI

paper-ui-blank-page-message = This page intentionally left blank

# Shown when paper with words examined details
paper-component-examine-detail-has-words = {CAPITALIZE(THE($paper))} has something written on it.
# Shown when paper with stamps examined
paper-component-examine-detail-stamped-by = {CAPITALIZE(THE($paper))} {CONJUGATE-HAVE($paper)} been stamped by: {$stamps}.

paper-component-action-stamp-paper-other = {CAPITALIZE(THE($user))} stamps {THE($target)} with {THE($stamp)}.
paper-component-action-stamp-paper-self = You stamp {THE($target)} with {THE($stamp)}.

# Indicator to show how full a paper is
paper-ui-fill-level = {$currentLength}/{$maxLength}

paper-ui-save-button = Save ({$keybind})
