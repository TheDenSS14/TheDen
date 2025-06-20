# SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

multi-handed-item-pick-up-fail = {$number -> 
    [one] You need one more free hand to pick up { THE($item) }.
    *[other] You need { $number } more free hands to pick up { THE($item) }.
}
