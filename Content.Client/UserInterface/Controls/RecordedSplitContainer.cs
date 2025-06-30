// SPDX-FileCopyrightText: 2023 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls;

/// <summary>
///     A split container that performs an action when the split resizing is finished.
/// </summary>
public sealed class RecordedSplitContainer : SplitContainer
{
    public double? DesiredSplitCenter;

    protected override Vector2 ArrangeOverride(Vector2 finalSize)
    {
        if (ResizeMode == SplitResizeMode.RespectChildrenMinSize
            && DesiredSplitCenter != null
            && !finalSize.Equals(Vector2.Zero))
        {
            SplitFraction = (float) DesiredSplitCenter.Value;

            if (!Size.Equals(Vector2.Zero))
            {
                DesiredSplitCenter = null;
            }
        }

        return base.ArrangeOverride(finalSize);
    }
}
