# SPDX-FileCopyrightText: 2025 Whatstone
#
# SPDX-License-Identifier: AGPL-3.0-or-later

crate-storage-rack-examine = { $count ->
    [0] The rack is empty.
    [1] The rack contains {$count} item:
    *[others] The rack contains {$count} items:
}
