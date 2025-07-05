// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Server.Ipc;

[RegisterComponent]
public sealed partial class IpcIonAlertComponent : Component
{
    [DataField]
    public float AlertChance = 0.9f; // sneaky chance to miss an ion storm
}
