<!--
SPDX-FileCopyrightText: 2017 PJB3005 <pieterjan.briers@gmail.com>
SPDX-FileCopyrightText: 2018 Pieter-Jan Briers <pieterjan.briers@gmail.com>
SPDX-FileCopyrightText: 2019 Ivan <silvertorch5@gmail.com>
SPDX-FileCopyrightText: 2019 Silver <silvertorch5@gmail.com>
SPDX-FileCopyrightText: 2020 Injazz <43905364+Injazz@users.noreply.github.com>
SPDX-FileCopyrightText: 2020 RedlineTriad <39059512+RedlineTriad@users.noreply.github.com>
SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <zddm@outlook.es>
SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
SPDX-FileCopyrightText: 2021 Swept <sweptwastaken@protonmail.com>
SPDX-FileCopyrightText: 2021 mirrorcult <lunarautomaton6@gmail.com>
SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
SPDX-FileCopyrightText: 2022 ike709 <ike709@users.noreply.github.com>
SPDX-FileCopyrightText: 2023 iglov <iglov@avalon.land>
SPDX-FileCopyrightText: 2024 BasedPugilist <125258730+BasedPugilist@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 Debug <49997488+DebugOk@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 Fansana <116083121+Fansana@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 Pspritechologist <81725545+Pspritechologist@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
SPDX-FileCopyrightText: 2024 stellar-novas <158120145+stellar-novas@users.noreply.github.com>
SPDX-FileCopyrightText: 2024 stellar-novas <stellar_novas@riseup.net>
SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>

SPDX-License-Identifier: AGPL-3.0-or-later AND MIT
-->

# The Den

<p align="center"><img src="https://raw.githubusercontent.com/TheDenSS14/TheDen/master/Resources/Textures/Logo/logo.png" width="512px" /></p>

---

<!-- Put Description Text Here -->

## Links
[Discord](https://denstation.net/discord) | [Wiki](https://wiki.denstation.net) | [Steam](https://store.steampowered.com/app/1255460/Space_Station_14/) | [Standalone](https://spacestationmultiverse.com/downloads/)

## Contributing

We are happy to accept contributions from anybody, come join our Discord if you want to help.
We've got a list of issues [here](https://github.com/TheDenSS14/TheDen/issues) and a list in our discord! Don't be afraid to ask for help either!

We are currently accepting translations of the game on our main repository.
If you would like to translate the game into another language check the #contributor-general channel in our Discord.

## Building

Refer to [the Space Wizards' guide](https://docs.spacestation14.com/en/general-development/setup/setting-up-a-development-environment.html) on setting up a development environment for general information, but keep in mind that Einstein Engines is not the same and many things may not apply.
We provide some scripts shown below to make the job easier.

### Build dependencies

> - Git
> - .NET SDK 9.0.101


### Windows

> 1. Clone this repository
> 2. Run `git submodule update --init --recursive` in a terminal to download the engine
> 3. Run `Scripts/bat/buildAllDebug.bat` after making any changes to the source
> 4. Run `Scripts/bat/runQuickAll.bat` to launch the client and the server
> 5. Connect to localhost in the client and play

### Linux

> 1. Clone this repository
> 2. Run `git submodule update --init --recursive` in a terminal to download the engine
> 3. Run `Scripts/sh/buildAllDebug.sh` after making any changes to the source
> 4. Run `Scripts/sh/runQuickAll.sh` to launch the client and the server
> 5. Connect to localhost in the client and play

### MacOS

> I don't know anybody using MacOS to test this, but it's probably roughly the same steps as Linux

## License

All code in this codebase is released under the AGPL-3.0-or-later license. Each file includes REUSE Specification headers or separate .license files that specify a dual license option. This dual licensing is provided to simplify the process for projects that are not using AGPL, allowing them to adopt the relevant portions of the code under an alternative license. You can review the complete texts of these licenses in the LICENSES/ directory.

Most media assets are licensed under [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless stated otherwise. Assets have their license and the copyright in the metadata file. [Example](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

Note that some assets are licensed under the non-commercial [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) or similar non-commercial licenses and will need to be removed if you wish to use this project commercially.
