// SPDX-FileCopyrightText: 2024 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 2024 portfiend <109661617+portfiend@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared._DV.QuickPhrase;

[Prototype]
public sealed partial class QuickPhrasePrototype : IPrototype, IInheritingPrototype
{
    /// <summary>
    /// The "in code name" of the object. Must be unique.
    /// </summary>
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The prototype we inherit from.
    /// </summary>
    [ViewVariables]
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<QuickPhrasePrototype>))]
    public string[]? Parents { get; }

    [ViewVariables]
    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; }

    /// <summary>
    /// The phrase that this prototype represents.
    /// </summary>
    [DataField]
    public LocId Text = string.Empty;

    /// <summary>
    /// Determines how the phrase is sorted in the UI.
    /// </summary>
    [DataField]
    public string Group = string.Empty;

    /// <summary>
    /// The tab in the UI that this phrase falls under.
    /// </summary>
    [DataField]
    public string Tab = string.Empty;

    /// <summary>
    /// Color of button in UI.
    /// </summary>
    [DataField]
    public string StyleClass = string.Empty;
}

/// <summary>
/// Imp. Added this to enable custom AAC vocabularies
/// </summary>
[Prototype]
public sealed partial class QuickPhraseGroupPrototype : IPrototype, IInheritingPrototype
{
    /// <summary>
    /// The "in code name" of the object. Must be unique.
    /// </summary>
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The prototype we inherit from.
    /// </summary>
    [ViewVariables]
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<QuickPhraseGroupPrototype>))]
    public string[]? Parents { get; }

    [ViewVariables]
    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; }

    /// <summary>
    /// List of prototype IDs in this group.
    /// </summary>
    [DataField]
    public List<ProtoId<QuickPhrasePrototype>> Prototypes { get; set; } = default!;
}
