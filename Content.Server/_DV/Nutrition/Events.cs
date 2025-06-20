// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Nutrition;

/// <summary>
/// Raised on a food being sliced.
/// Used by deep frier to apply friedness to slices (e.g. deep fried pizza)
/// </summary>
/// <remarks>
/// Not to be confused with upstream SliceFoodEvent which doesn't pass the slice entities, and is only raised once.
/// </remarks>
[ByRefEvent]
public sealed class FoodSlicedEvent : EntityEventArgs
{
    /// <summary>
    /// Who did the slicing?
    /// <summary>
    public EntityUid User;

    /// <summary>
    /// What has been sliced?
    /// <summary>
    /// <remarks>
    /// This could soon be deleted if there was not enough food left to
    /// continue slicing.
    /// </remarks>
    public EntityUid Food;

    /// <summary>
    /// What is the slice?
    /// <summary>
    public EntityUid Slice;

    public FoodSlicedEvent(EntityUid user, EntityUid food, EntityUid slice)
    {
        User = user;
        Food = food;
        Slice = slice;
    }
}