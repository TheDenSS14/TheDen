// SPDX-FileCopyrightText: 2024 Milon
// SPDX-FileCopyrightText: 2024 portfiend
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 MajorMoth
// SPDX-FileCopyrightText: 2025 SleepyScarecrow
// SPDX-FileCopyrightText: 2025 little-meow-meow
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._DV.AACTablet;
using Content.Shared._DV.QuickPhrase;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._DV.AACTablet.UI;

public sealed partial class AACBoundUserInterface : BoundUserInterface // starcup: made partial
{
    [ViewVariables]
    private AACWindow? _window;

    public AACBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = new AACWindow(Owner);
        _window.OpenCentered();
        _window.OnClose += Close;
        _window.PhraseButtonPressed += OnPhraseButtonPressed;
    }

    private void OnPhraseButtonPressed(List<ProtoId<QuickPhrasePrototype>> phraseId, string prefix)
    {
        SendMessage(new AACTabletSendPhraseMessage(phraseId, prefix)); // starcup: prefix parameter
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;

        _window?.Parent?.RemoveChild(_window);
    }
}
