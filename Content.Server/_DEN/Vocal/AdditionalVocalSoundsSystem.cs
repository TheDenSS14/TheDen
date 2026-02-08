// SPDX-FileCopyrightText: 2025 Falcon
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using Content.Shared.Chat.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;


namespace Content.Server._DEN.Vocal;


/// <summary>
/// This handles imitating species noises.
/// </summary>
public sealed class AdditionalVocalSoundsSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public void AddVocalSound(Entity<AdditionalVocalSoundsComponent> ent, ProtoId<EmoteSoundsPrototype> protoId)
    {
        if (!_prototypeManager.TryIndex(protoId, out _)
            || !ent.Comp.AdditionalSounds.Add(protoId))
            return;

        Dirty(ent);
    }

    public Dictionary<string, SoundSpecifier> GetVocalSounds(Entity<AdditionalVocalSoundsComponent> ent, EmoteSoundsPrototype? baseSounds = null )
    {
        var result = baseSounds?.Sounds != null ? new(baseSounds.Sounds) : new Dictionary<string, SoundSpecifier>();

        foreach (var sound in ent.Comp.AdditionalSounds)
        {
            if (!_prototypeManager.TryIndex(sound, out var sounds)
                || sounds.Sounds.Count == 0)
                continue;

            foreach (var (soundId, specifier) in sounds.Sounds)
                result.TryAdd(soundId, specifier);
        }

        if (!_prototypeManager.TryIndex(ent.Comp.ReplacesDefaultSounds, out var replacesSounds)
            || replacesSounds.Sounds.Count == 0)
            return result;

        foreach (var (soundId, specifier) in replacesSounds.Sounds)
            result[soundId] = specifier;

        return result;
    }
}
