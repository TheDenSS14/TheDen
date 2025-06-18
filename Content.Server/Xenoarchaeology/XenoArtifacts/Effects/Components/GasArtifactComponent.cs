// SPDX-FileCopyrightText: 2022 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos;

namespace Content.Server.Xenoarchaeology.XenoArtifacts.Effects.Components;

/// <summary>
///     Spawn a random gas with random temperature when artifact activated.
/// </summary>
[RegisterComponent]
public sealed partial class GasArtifactComponent : Component
{
    /// <summary>
    ///     Gas that will be spawned when artifact activated.
    ///     If null it will be picked on startup from <see cref="PossibleGases"/>.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("spawnGas")]
    public Gas? SpawnGas;

    /// <summary>
    ///     List of possible activation gases to pick on startup.
    /// </summary>
    [DataField("possibleGas")]
    public List<Gas> PossibleGases = new()
    {
        Gas.Oxygen,
        Gas.Plasma,
        Gas.Nitrogen,
        Gas.CarbonDioxide,
        Gas.Tritium,
        Gas.Ammonia,
        Gas.NitrousOxide,
        Gas.Frezon,
        Gas.BZ, // Assmos - /tg/ gases
        Gas.Healium, // Assmos - /tg/ gases
        Gas.Nitrium, // Assmos - /tg/ gases
        Gas.Pluoxium, // Assmos - /tg/ gases
        Gas.Hydrogen, // Assmos - /tg/ gases
        Gas.HyperNoblium, // Assmos - /tg/ gases
        Gas.ProtoNitrate, // Assmos - /tg/ gases
        Gas.Zauker, // Assmos - /tg/ gases
        Gas.Halon, // Assmos - /tg/ gases
        Gas.Helium, // Assmos - /tg/ gases
        Gas.AntiNoblium, // Assmos - /tg/ gases
    };

    /// <summary>
    ///     Temperature of spawned gas. If null it will be picked on startup from range from
    ///     <see cref="MinRandomTemperature"/> to <see cref="MaxRandomTemperature"/>.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("spawnTemperature")]
    public float? SpawnTemperature;

    [DataField("minRandomTemp")]
    public float MinRandomTemperature = 100;

    [DataField("maxRandomTemp")]
    public float MaxRandomTemperature = 400;

    /// <summary>
    ///     Max allowed external atmospheric pressure.
    ///     Artifact will stop spawn gas.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("maxExternalPressure")]
    public float MaxExternalPressure = Atmospherics.GasMinerDefaultMaxExternalPressure;

    /// <summary>
    ///     Moles of gas to spawn each time when artifact activated.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("spawnAmount")]
    public float SpawnAmount = Atmospherics.MolesCellStandard * 3;
}
