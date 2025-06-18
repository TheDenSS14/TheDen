// SPDX-FileCopyrightText: 2024 Verm <32827189+Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Labels.Components;

/// <summary>                                                                                                                                                 
/// Specifies the paper type (see textures/storage/crates/labels.rsi to see currently supported paper types)  to show on crates this label is attached to.                                                                     
/// </summary>                                                                                                                                                
[RegisterComponent, NetworkedComponent]
public sealed partial class PaperLabelTypeComponent : Component
{
    /// <summary>                                                                                                                                             
    /// The type of label to show.                                                                                                                                    
    /// </summary>
    [DataField]
    public string PaperType = "Paper";
}
