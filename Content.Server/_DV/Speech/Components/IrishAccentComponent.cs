// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server._DV.Speech.EntitySystems;

namespace Content.Server._DV.Speech.Components;

// Takes the ES and assigns the system and component to each other
[RegisterComponent]
[Access(typeof(IrishAccentSystem))]
public sealed partial class IrishAccentComponent : Component
{ }
