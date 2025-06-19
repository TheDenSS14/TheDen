// SPDX-FileCopyrightText: 2023 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Slava0135 <40753025+Slava0135@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Weapons.Melee.EnergySword;

[RegisterComponent]
internal sealed partial class EnergySwordComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("activatedColor"), AutoNetworkedField]
    public Color ActivatedColor = Color.DodgerBlue;

    /// <summary>
    ///     A color option list for the random color picker.
    /// </summary>
    [DataField("colorOptions")]
    public List<Color> ColorOptions = new()
    {
        Color.Tomato,
        Color.DodgerBlue,
        Color.Aqua,
        Color.MediumSpringGreen,
        Color.MediumOrchid
    };

    public bool Hacked = false;
    /// <summary>
    ///     RGB cycle rate for hacked e-swords.
    /// </summary>
    [DataField("cycleRate")]
    public float CycleRate = 1f;
}
