// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Shared._Shitmed.Targeting;
public abstract class SharedTargetingSystem : EntitySystem
{
    /// <summary>
    /// Returns all Valid target body parts as an array.
    /// </summary>
    public static TargetBodyPart[] GetValidParts()
    {
        var parts = new[]
        {
            TargetBodyPart.Head,
            TargetBodyPart.Torso,
            //TargetBodyPart.Groin,
            TargetBodyPart.LeftArm,
            TargetBodyPart.LeftHand,
            TargetBodyPart.LeftLeg,
            TargetBodyPart.LeftFoot,
            TargetBodyPart.RightArm,
            TargetBodyPart.RightHand,
            TargetBodyPart.RightLeg,
            TargetBodyPart.RightFoot,
        };

        return parts;
    }
}
