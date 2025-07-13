// SPDX-FileCopyrightText: 2019 moneyl
// SPDX-FileCopyrightText: 2020 Exp
// SPDX-FileCopyrightText: 2020 ShadowCommander
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2020 ike709
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Galactic Chimp
// SPDX-FileCopyrightText: 2021 Ygg01
// SPDX-FileCopyrightText: 2022 0x6273
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Kevin Zheng
// SPDX-FileCopyrightText: 2024 metalgearsloth
// SPDX-FileCopyrightText: 2025 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Guidebook.Components;
using Content.Client.UserInterface.Controls;
using Content.Shared.Chemistry;
using Content.Shared.Containers.ItemSlots;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client.Chemistry.UI
{
    /// <summary>
    /// Initializes a <see cref="ReagentDispenserWindow"/> and updates it when new server messages are received.
    /// </summary>
    [UsedImplicitly]
    public sealed class ReagentDispenserBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private ReagentDispenserWindow? _window;

        public ReagentDispenserBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        /// <summary>
        /// Called each time a dispenser UI instance is opened. Generates the dispenser window and fills it with
        /// relevant info. Sets the actions for static buttons.
        /// <para>Buttons which can change like reagent dispense buttons have their actions set in <see cref="UpdateReagentsList"/>.</para>
        /// </summary>
        protected override void Open()
        {
            base.Open();

            // Setup window layout/elements
            _window = this.CreateWindow<ReagentDispenserWindow>();
            _window.SetInfoFromEntity(EntMan, Owner);

            // Setup static button actions.
            _window.EjectButton.OnPressed += _ => SendMessage(new ItemSlotButtonPressedEvent(SharedReagentDispenser.OutputSlotName));
            _window.ClearButton.OnPressed += _ => SendMessage(new ReagentDispenserClearContainerSolutionMessage());

            _window.AmountGrid.OnButtonPressed += s => SendMessage(new ReagentDispenserSetDispenseAmountMessage(s));

            _window.OnDispenseReagentButtonPressed += (id) => SendMessage(new ReagentDispenserDispenseReagentMessage(id));
            _window.OnEjectJugButtonPressed += (id) => SendMessage(new ItemSlotButtonPressedEvent(id));
        }

        /// <summary>
        /// Update the UI each time new state data is sent from the server.
        /// </summary>
        /// <param name="state">
        /// Data of the <see cref="ReagentDispenserComponent"/> that this UI represents.
        /// Sent from the server.
        /// </param>
        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            var castState = (ReagentDispenserBoundUserInterfaceState) state;
            _window?.UpdateState(castState); //Update window state
        }
    }
}
