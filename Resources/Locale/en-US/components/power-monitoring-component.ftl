# SPDX-FileCopyrightText: 2022 20kdc <asdd2808@gmail.com>
# SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

power-monitoring-window-title = Power Monitoring Console

power-monitoring-window-label-sources = Sources
power-monitoring-window-label-smes = SMES
power-monitoring-window-label-substation = Substation
power-monitoring-window-label-apc = APC
power-monitoring-window-label-misc = Misc

power-monitoring-window-object-array = {$name} array [{$count}]

power-monitoring-window-station-name = [color=white][font size=14]{$stationName}[/font][/color]
power-monitoring-window-unknown-location = Unknown location
power-monitoring-window-total-sources = Total generator output
power-monitoring-window-total-battery-usage = Total battery usage
power-monitoring-window-total-loads = Total network loads
power-monitoring-window-value = { POWERWATTS($value) }
power-monitoring-window-button-value = {$value} W
power-monitoring-window-show-inactive-consumers = Show Inactive Consumers

power-monitoring-window-show-cable-networks = Toggle cable networks
power-monitoring-window-show-hv-cable = High voltage
power-monitoring-window-show-mv-cable = Medium voltage
power-monitoring-window-show-lv-cable = Low voltage

power-monitoring-window-flavor-left = [user@nanotrasen] $run power_net_query
power-monitoring-window-flavor-right = v1.3
power-monitoring-window-rogue-power-consumer = [color=white][font size=14][bold]! WARNING - ROGUE POWER CONSUMING DEVICE DETECTED ![/bold][/font][/color]
power-monitoring-window-power-net-abnormalities = [color=white][font size=14][bold]CAUTION - ABNORMAL ACTIVITY IN POWER NET[/bold][/font][/color]
