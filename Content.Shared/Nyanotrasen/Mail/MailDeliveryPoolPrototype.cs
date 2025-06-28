// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Mail;

/// <summary>
/// Generic random weighting dataset to use.
/// </summary>
[Prototype("mailDeliveryPool")]
public sealed partial class MailDeliveryPoolPrototype : IPrototype
{
    [IdDataFieldAttribute] public string ID { get; } = default!;

    /// <summary>
    /// Mail that can be sent to everyone.
    /// </summary>
    [DataField("everyone")]
    public Dictionary<string, float> Everyone = new();

    /// <summary>
    /// Mail that can be sent only to specific jobs.
    /// </summary>
    [DataField("jobs")]
    public Dictionary<string, Dictionary<string, float>> Jobs = new();

    /// <summary>
    /// Mail that can be sent only to specific departments.
    /// </summary>
    [DataField("departments")]
    public Dictionary<string, Dictionary<string, float>> Departments = new();
}
