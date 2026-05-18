// SPDX-FileCopyrightText: 2024 ArchPigeon
// SPDX-FileCopyrightText: 2024 FoxxoTrystan
// SPDX-FileCopyrightText: 2024 Krunklehorn
// SPDX-FileCopyrightText: 2024 Morb
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared._DEN.Cybereye.Components;
using Content.Shared._DEN.Actions;
using Content.Shared.Actions;
using Robust.Shared.Prototypes;

namespace Content.Shared.Cybereye;

/// <summary>
/// Adds an action to toggle the HUD given by cybereyes.
/// </summary>
public sealed class SharedCybereyeControlSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly ILogManager _logManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CybereyeControlComponent, MapInitEvent>(OnCybereyeControlMapInit);
        SubscribeLocalEvent<CybereyeControlComponent, ComponentShutdown>(OnCybereyeControlShutdown);

        SubscribeLocalEvent<SecCybereyeControlComponent, CybereyeControlActionEvent>(OnActionEvent);
        SubscribeLocalEvent<MedCybereyeControlComponent, CybereyeControlActionEvent>(OnActionEvent);
        SubscribeLocalEvent<DiagCybereyeControlComponent, CybereyeControlActionEvent>(OnActionEvent);
        SubscribeLocalEvent<OmniCybereyeControlComponent, CybereyeControlActionEvent>(OnActionEvent);

        _sawmill = _logManager.GetSawmill("CybereyeControl");
    }

    private void OnCybereyeControlMapInit(EntityUid uid, CybereyeControlComponent component, MapInitEvent args)
    {
        _actions.AddAction(uid, ref component.ActionEntity, component.Action, uid);
    }

    private void OnCybereyeControlShutdown(EntityUid uid, CybereyeControlComponent component, ComponentShutdown args)
    {
        _actions.RemoveAction(uid, component.ActionEntity);
    }

    private void OnActionEvent(EntityUid uid, BaseCybereyeControlComponent component, CybereyeControlActionEvent args)
    {
        // we don't check for handling here, since multiple components will have to handle the event at the same time
        if (!TryToggleCybereye(uid, component))
            _sawmill.Error($"Failed to toggle cybereye on entity {uid}");
    }

    public bool TryToggleCybereye(EntityUid uid, BaseCybereyeControlComponent? cybereyeControlComponent)
    {
        if (!Resolve(uid, ref cybereyeControlComponent))
            return false;

        ToggleComponents(uid, cybereyeControlComponent.RemoveOnToggle, cybereyeControlComponent.Active);

        cybereyeControlComponent.Active = !cybereyeControlComponent.Active;

        return true;
    }

    public void ToggleComponents(EntityUid target, ComponentRegistry? removeOnToggle, bool activated)
    {
        if (activated)
        {
            if (removeOnToggle is not null)
                _entityManager.RemoveComponents(target, removeOnToggle);
        }
        else
        {
            if (removeOnToggle is not null)
                _entityManager.AddComponents(target, removeOnToggle);
        }
    }
}
