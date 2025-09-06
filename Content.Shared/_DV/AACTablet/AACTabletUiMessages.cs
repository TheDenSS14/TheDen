// SPDX-FileCopyrightText: 2024 Milon
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 little-meow-meow
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.QuickPhrase;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.AACTablet;

[Serializable, NetSerializable]
public enum AACTabletKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class AACTabletSendPhraseMessage(List<ProtoId<QuickPhrasePrototype>> phraseIds, string prefix) : BoundUserInterfaceMessage // starcup: added prefix
{
    public List<ProtoId<QuickPhrasePrototype>> PhraseIds = phraseIds;
    public string Prefix = prefix; // starcup: radio-enabled aac
}
