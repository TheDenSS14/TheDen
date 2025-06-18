// SPDX-FileCopyrightText: 2021 Watermelon914 <37270891+Watermelon914@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Labels;
using Content.Shared.Labels.Components;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client.Labels.UI
{
    /// <summary>
    /// Initializes a <see cref="HandLabelerWindow"/> and updates it when new server messages are received.
    /// </summary>
    public sealed class HandLabelerBoundUserInterface : BoundUserInterface
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        [ViewVariables]
        private HandLabelerWindow? _window;

        public HandLabelerBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
            IoCManager.InjectDependencies(this);
        }

        protected override void Open()
        {
            base.Open();

            _window = this.CreateWindow<HandLabelerWindow>();

            _window.OnLabelChanged += OnLabelChanged;
            Reload();
        }

        private void OnLabelChanged(string newLabel)
        {
            // Focus moment
            if (_entManager.TryGetComponent(Owner, out HandLabelerComponent? labeler) &&
                labeler.AssignedLabel.Equals(newLabel))
                return;

            SendPredictedMessage(new HandLabelerLabelChangedMessage(newLabel));
        }

        public void Reload()
        {
            if (_window == null || !_entManager.TryGetComponent(Owner, out HandLabelerComponent? component))
                return;

            _window.SetCurrentLabel(component.AssignedLabel);
        }
    }
}
