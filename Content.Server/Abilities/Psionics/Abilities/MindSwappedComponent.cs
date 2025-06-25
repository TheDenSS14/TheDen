// SPDX-FileCopyrightText: 2023 PHCodes <47927305+PHCodes@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Abilities.Psionics
{
    [RegisterComponent]
    public sealed partial class MindSwappedComponent : Component
    {
        [ViewVariables]
        public EntityUid OriginalEntity = default!;
        [DataField("mindSwapReturnActionId",
        customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? MindSwapReturnActionId = "ActionMindSwapReturn";

        [DataField("mindSwapReturnActionEntity")]
        public EntityUid? MindSwapReturnActionEntity;
    }
}
