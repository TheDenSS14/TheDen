// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 KekaniCreates <87507256+KekaniCreates@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Sapphire <98045970+sapphirescript@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 juniwoofs <180479595+juniwoofs@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Humanoid.Markings
{
    [Serializable, NetSerializable]
    public enum MarkingCategories : byte
    {
        Face,
        Hair,
        FacialHair,
        Eyes, // Imp - Adds selectable eyes
        Head,
        HeadTop,
        HeadSide,
        Snout,
        Chest,
        Genitals,
        Nipples,
        NeckFluff,
        RightArm,
        RightHand,
        LeftArm,
        LeftHand,
        RightLeg,
        RightFoot,
        LeftLeg,
        LeftFoot,
        Wings,
        Underwear,
        Undershirt,
        Tail,
        Overlay
    }

    public static class MarkingCategoriesConversion
    {
        public static MarkingCategories FromHumanoidVisualLayers(HumanoidVisualLayers layer)
        {
            return layer switch
            {
                HumanoidVisualLayers.Face => MarkingCategories.Face,
                HumanoidVisualLayers.Hair => MarkingCategories.Hair,
                HumanoidVisualLayers.FacialHair => MarkingCategories.FacialHair,
				HumanoidVisualLayers.Eyes => MarkingCategories.Eyes, // Imp - Adds selectable eyes
                HumanoidVisualLayers.Head => MarkingCategories.Head,
                HumanoidVisualLayers.HeadTop => MarkingCategories.HeadTop,
                HumanoidVisualLayers.HeadSide => MarkingCategories.HeadSide,
                HumanoidVisualLayers.Snout => MarkingCategories.Snout,
                HumanoidVisualLayers.Undershirt => MarkingCategories.Undershirt,
                HumanoidVisualLayers.Underwear => MarkingCategories.Underwear,
                HumanoidVisualLayers.Chest => MarkingCategories.Chest,
                HumanoidVisualLayers.Genitals => MarkingCategories.Genitals,
                HumanoidVisualLayers.Nipples => MarkingCategories.Nipples,
                HumanoidVisualLayers.NeckFluff => MarkingCategories.NeckFluff,
                HumanoidVisualLayers.RArm => MarkingCategories.RightArm,
                HumanoidVisualLayers.LArm => MarkingCategories.LeftArm,
                HumanoidVisualLayers.RHand => MarkingCategories.RightHand,
                HumanoidVisualLayers.LHand => MarkingCategories.LeftHand,
                HumanoidVisualLayers.LLeg => MarkingCategories.LeftLeg,
                HumanoidVisualLayers.RLeg => MarkingCategories.RightLeg,
                HumanoidVisualLayers.LFoot => MarkingCategories.LeftFoot,
                HumanoidVisualLayers.RFoot => MarkingCategories.RightFoot,
                HumanoidVisualLayers.Wings => MarkingCategories.Wings,
                HumanoidVisualLayers.Tail => MarkingCategories.Tail,
                _ => MarkingCategories.Overlay
            };
        }
    }
}
