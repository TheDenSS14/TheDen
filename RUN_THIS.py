# SPDX-FileCopyrightText: 2017 PJB3005
# SPDX-FileCopyrightText: 2017 Pieter-Jan Briers
# SPDX-FileCopyrightText: 2021 Paul Ritter
# SPDX-FileCopyrightText: 2021 Swept
# SPDX-FileCopyrightText: 2024 SimpleStation14
# SPDX-FileCopyrightText: 2024 stellar-novas
# SPDX-FileCopyrightText: 2025 AvianMaiden
#
# SPDX-License-Identifier: AGPL-3.0-or-later

#!/usr/bin/env python3

# Import future so people on py2 still get the clear error that they need to upgrade.
from __future__ import print_function
import sys
import subprocess

version = sys.version_info
if version.major < 3 or (version.major == 3 and version.minor < 5):
    print("ERROR: You need at least Python 3.5 to build SS14.")
    sys.exit(1)

subprocess.run([sys.executable, "git_helper.py"], cwd="BuildChecker")
