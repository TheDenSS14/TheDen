// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2025 Milon
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.DetailExaminable;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DetailExaminableComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public string Content = string.Empty;

    // DEN - NSFW examine text
    [DataField(required: true), AutoNetworkedField]
    public string NsfwContent = string.Empty;

    // DEN - Self-examination text
    [DataField, AutoNetworkedField]
    public string SelfContent = string.Empty;
}
