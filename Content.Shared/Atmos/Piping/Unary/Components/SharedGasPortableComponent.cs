// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.Components
{
    /// <summary>
    /// Used in <see cref="GasPortableVisualizer"/> to determine which visuals to update.
    /// </summary>
    [Serializable, NetSerializable]
        public enum GasPortableVisuals
        {
            ConnectedState,
        }
}
