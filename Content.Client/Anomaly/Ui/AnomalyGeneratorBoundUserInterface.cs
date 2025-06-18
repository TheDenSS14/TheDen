// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Anomaly;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Anomaly.Ui;

[UsedImplicitly]
public sealed class AnomalyGeneratorBoundUserInterface : BoundUserInterface
{
    private AnomalyGeneratorWindow? _window;

    public AnomalyGeneratorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<AnomalyGeneratorWindow>();
        _window.SetEntity(Owner);

        _window.OnGenerateButtonPressed += () =>
        {
            SendMessage(new AnomalyGeneratorGenerateButtonPressedEvent());
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not AnomalyGeneratorUserInterfaceState msg)
            return;
        _window?.UpdateState(msg);
    }
}

