// SPDX-FileCopyrightText: 2024 brainfood1183 <113240905+brainfood1183@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Plunger.Components
{
    /// <summary>
    /// Allows entity to unblock target entity with PlungerUseComponent.
    /// </summary>
    [RegisterComponent, NetworkedComponent,AutoGenerateComponentState]
    public sealed partial class PlungerComponent : Component
    {
        /// <summary>
        /// Duration of plunger doafter event.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public float PlungeDuration = 2f;
    }
}
