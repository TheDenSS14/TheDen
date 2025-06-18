// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server._DEN.GameTicking.Rules;

public interface IFakePreset
{
    public HashSet<EntityUid> AdditionalGameRules { get; set; }
}

