// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Snowni
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 AJCM-git
// SPDX-FileCopyrightText: 2023 Chief-Engineer
// SPDX-FileCopyrightText: 2023 Julian Giebel
// SPDX-FileCopyrightText: 2025 creku
// SPDX-FileCopyrightText: 2025 slarticodefast
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.DeviceLinking.Systems;
using Content.Server.Explosion.Components;
using Content.Shared.DeviceLinking.Events;

// DEN - much of this has been touched or renamed to allow for the SignalOnTrigger component from Wizden
namespace Content.Server.Explosion.EntitySystems
{
    public sealed partial class TriggerSystem
    {
        [Dependency] private readonly DeviceLinkSystem _signalSystem = default!;
        private void InitializeSignal()
        {
            SubscribeLocalEvent<TriggerOnSignalComponent, ComponentInit>(TriggerOnSignalInit);
            SubscribeLocalEvent<TriggerOnSignalComponent, SignalReceivedEvent>(OnSignalReceived);

            SubscribeLocalEvent<SignalOnTriggerComponent, ComponentInit>(SignalOnTriggerInit);
            SubscribeLocalEvent<SignalOnTriggerComponent, TriggerEvent>(HandleSignalOnTrigger);

            SubscribeLocalEvent<TimerStartOnSignalComponent, ComponentInit>(OnTimerSignalInit);
            SubscribeLocalEvent<TimerStartOnSignalComponent, SignalReceivedEvent>(OnTimerSignalReceived);
        }

        // DENWIZ - rough import of wizden SignalOnTrigger
        private void TriggerOnSignalInit(EntityUid uid, TriggerOnSignalComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSinkPorts(uid, component.Port);
        }

        private void OnSignalReceived(EntityUid uid, TriggerOnSignalComponent component, ref SignalReceivedEvent args)
        {
            if (args.Port != component.Port)
                return;

            Trigger(uid, args.Trigger);
        }

        // DENWIZ - rough import of wizden SignalOnTrigger
        private void SignalOnTriggerInit(EntityUid uid, SignalOnTriggerComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSourcePorts(uid, component.Port);
        }

        private void HandleSignalOnTrigger(EntityUid uid, SignalOnTriggerComponent component, TriggerEvent args)
        {
            _signalSystem.InvokePort(uid, component.Port);
            args.Handled = true;
        }

        private void OnTimerSignalReceived(EntityUid uid, TimerStartOnSignalComponent component, ref SignalReceivedEvent args)
        {
            if (args.Port != component.Port)
                return;

            StartTimer(uid, args.Trigger);
        }
        private void OnTimerSignalInit(EntityUid uid, TimerStartOnSignalComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSinkPorts(uid, component.Port);
        }
    }
}
