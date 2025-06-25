// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Paint;

/// Component applied to target entity when painted
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PaintedComponent : Component
{
    [DataField, AutoNetworkedField]
    public Color Color = Color.FromHex("#2cdbd5");

    /// Used to remove the color when the component is removed
    [DataField, AutoNetworkedField]
    public Color BeforeColor;

    [DataField, AutoNetworkedField]
    public bool Enabled;

    // Not using ProtoId because ShaderPrototype is in Robust.Client
    [DataField, AutoNetworkedField]
    public string ShaderName = "Greyscale";
}

[Serializable, NetSerializable]
public enum PaintVisuals : byte
{
    Painted,
}
