// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 2021 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Construction.Components;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Containers;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Server.Construction.Components
{
    [RegisterComponent]
    public sealed partial class MachineFrameComponent : Component
    {
        public const string PartContainerName = "machine_parts";
        public const string BoardContainerName = "machine_board";

        [ViewVariables]
        public bool HasBoard => BoardContainer?.ContainedEntities.Count != 0;

        [DataField("progress", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<int, MachinePartPrototype>))]
        public Dictionary<string, int> Progress = new();

        [ViewVariables]
        public readonly Dictionary<string, int> MaterialProgress = new();

        [ViewVariables]
        public readonly Dictionary<string, int> ComponentProgress = new();

        [ViewVariables]
        public readonly Dictionary<string, int> TagProgress = new();

        [DataField("requirements", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<int, MachinePartPrototype>))]
        public Dictionary<string, int> Requirements = new();

        [ViewVariables]
        public Dictionary<string, int> MaterialRequirements = new();

        [ViewVariables]
        public Dictionary<string, GenericPartInfo> ComponentRequirements = new();

        [ViewVariables]
        public Dictionary<string, GenericPartInfo> TagRequirements = new();

        [ViewVariables]
        public Container BoardContainer = default!;

        [ViewVariables]
        public Container PartContainer = default!;
    }
}
