// SPDX-FileCopyrightText: 2022 Alex Evgrashin
// SPDX-FileCopyrightText: 2023 Morb
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2025 88tv
// SPDX-FileCopyrightText: 2025 lzk
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chat.TypingIndicator;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.Chat.TypingIndicator;

public sealed class TypingIndicatorVisualizerSystem : VisualizerSystem<TypingIndicatorComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    protected override void OnAppearanceChange(EntityUid uid, TypingIndicatorComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!_prototypeManager.TryIndex<TypingIndicatorPrototype>(component.Prototype, out var proto))
        {
            Log.Error($"Unknown typing indicator id: {component.Prototype}");
            return;
        }

        var layerExists = args.Sprite.LayerMapTryGet(TypingIndicatorLayers.Base, out var layer);
        if (!layerExists)
            layer = args.Sprite.LayerMapReserveBlank(TypingIndicatorLayers.Base);

        args.Sprite.LayerSetRSI(layer, proto.SpritePath);
        args.Sprite.LayerSetState(layer, proto.TypingState);
        args.Sprite.LayerSetShader(layer, proto.Shader);
        args.Sprite.LayerSetOffset(layer, proto.Offset);

        AppearanceSystem.TryGetData<TypingIndicatorState>(uid, TypingIndicatorVisuals.State, out var state);
        args.Sprite.LayerSetVisible(layer, state != TypingIndicatorState.None);
        switch (state)
        {
            case TypingIndicatorState.Idle:
                args.Sprite.LayerSetState(layer, proto.IdleState);
                break;
            case TypingIndicatorState.Typing:
                args.Sprite.LayerSetState(layer, proto.TypingState);
                break;
        }
    }
}
