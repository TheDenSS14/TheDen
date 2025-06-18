// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Mood;

/// <summary>
///     This component exists solely to network CurrentMoodLevel, so that clients can make use of its value for math Prediction.
///     All mood logic is otherwise handled by the Server, and the client is not allowed to know the identity of its mood events.
/// </summary>
[RegisterComponent, AutoGenerateComponentState]
public sealed partial class NetMoodComponent : Component
{
    [DataField, AutoNetworkedField]
    public float CurrentMoodLevel;

    [DataField, AutoNetworkedField]
    public float NeutralMoodThreshold;
}