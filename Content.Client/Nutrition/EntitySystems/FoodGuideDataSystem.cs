// SPDX-FileCopyrightText: 2024 Mnemotechnican <69920617+Mnemotechnician@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Chemistry.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Client.Nutrition.EntitySystems;

public sealed class FoodGuideDataSystem : SharedFoodGuideDataSystem
{
    public override void Initialize()
    {
        SubscribeNetworkEvent<FoodGuideRegistryChangedEvent>(OnReceiveRegistryUpdate);
    }

    private void OnReceiveRegistryUpdate(FoodGuideRegistryChangedEvent message)
    {
        Registry = message.Changeset;
    }

    public bool TryGetData(EntProtoId result, out FoodGuideEntry entry)
    {
        var index = Registry.FindIndex(it => it.Result == result);
        if (index == -1)
        {
            entry = default;
            return false;
        }

        entry = Registry[index];
        return true;
    }
}
