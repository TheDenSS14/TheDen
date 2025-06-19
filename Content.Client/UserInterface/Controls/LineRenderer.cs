// SPDX-FileCopyrightText: 2025 Solaris <60526456+SolarisBirb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using System.Numerics;

namespace Content.Client.UserInterface.Controls;

/// <summary>
/// A simple control that contains one or more rendered lines
/// </summary>
public sealed class LineRenderer : Control
{
    /// <summary>
    /// List of lines to render (their start and end x-y coordinates).
    /// Position (0,0) is the top left corner of the control and
    /// position (1,1) is the bottom right corner.
    /// The color of the lines is inherited from the control.
    /// </summary>
    public List<(Vector2, Vector2)> Lines = new List<(Vector2, Vector2)>();

    public LineRenderer(List<(Vector2, Vector2)> lines = default!)
    {
        Lines = lines;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        foreach (var line in Lines)
        {
            var start = PixelPosition +
                new Vector2(PixelWidth * line.Item1.X, PixelHeight * line.Item1.Y);

            var end = PixelPosition +
                new Vector2(PixelWidth * line.Item2.X, PixelHeight * line.Item2.Y);

            handle.DrawLine(start, end, ActualModulateSelf);
        }
    }
}