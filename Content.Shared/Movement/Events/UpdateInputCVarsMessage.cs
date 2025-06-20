// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Movement.Events;

/// <summary>
///     Raised from the client to the server to require the server to update the client's input CVars.
/// </summary>
[Serializable, NetSerializable]
public sealed class UpdateInputCVarsMessage : EntityEventArgs { }
