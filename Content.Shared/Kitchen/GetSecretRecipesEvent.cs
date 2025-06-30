// SPDX-FileCopyrightText: 2024 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared.Kitchen;

/// <summary>
/// This returns a list of recipes not found in the main list of available recipes.
/// </summary>
[ByRefEvent]
public struct GetSecretRecipesEvent()
{
    public List<FoodRecipePrototype> Recipes = new();
}
