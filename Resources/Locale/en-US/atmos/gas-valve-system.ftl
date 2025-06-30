# SPDX-FileCopyrightText: 2021 E F R <602406+Efruit@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

# Examine Text
gas-valve-system-examined = The valve is [color={$statusColor}]{$open ->
    [true]  open
   *[false] closed
}[/color].
