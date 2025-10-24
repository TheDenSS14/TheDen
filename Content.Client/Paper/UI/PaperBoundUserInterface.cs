// SPDX-FileCopyrightText: 2020 adrian
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 Fishfish458
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 fishfish458
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 Eoin Mcloughlin
// SPDX-FileCopyrightText: 2023 LordCarve
// SPDX-FileCopyrightText: 2023 Morb
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 eoineoineoin
// SPDX-FileCopyrightText: 2024 Julian Giebel
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using Content.Shared.Paper;
using static Content.Shared.Paper.PaperComponent;

namespace Content.Client.Paper.UI;

[UsedImplicitly]
public sealed class PaperBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private PaperWindow? _window;

    public PaperBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<PaperWindow>();
        _window.OnSaved += InputOnTextEntered;
        _window.OnSignatureRequested += OnSignatureRequested; // Starlight-edit

        if (EntMan.TryGetComponent<PaperComponent>(Owner, out var paper))
        {
            _window.MaxInputLength = paper.ContentSize;
        }
        if (EntMan.TryGetComponent<PaperVisualsComponent>(Owner, out var visuals))
        {
            _window.InitVisuals(Owner, visuals);
        }
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        _window?.Populate((PaperBoundUserInterfaceState) state);
    }

    private void InputOnTextEntered(string text)
    {
        SendMessage(new PaperInputTextMessage(text));

        if (_window != null)
        {
            _window.Input.TextRope = Rope.Leaf.Empty;
            _window.Input.CursorPosition = new TextEdit.CursorPos(0, TextEdit.LineBreakBias.Top);
        }
    }

    private void OnSignatureRequested(int signatureIndex) => SendMessage(new PaperSignatureRequestMessage(signatureIndex)); // Starlight-edit
}
