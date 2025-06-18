// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 fox <daytimer253@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Language.Events;

/// <summary>
///     Raised on an entity when its list of languages changes.
/// </summary>
/// <remarks>
///     This is raised both on the server and on the client.
///     The client raises it broadcast after receiving a new language comp state from the server.
/// </remarks>
public sealed class LanguagesUpdateEvent : EntityEventArgs
{
}
