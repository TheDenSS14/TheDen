// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

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
        if (!_prototypeManager.TryIndex(protoId, out _))
            return;

        ent.Comp.AdditionalSounds = protoId;
        Dirty(ent);
    }

    public Dictionary<string, SoundSpecifier> GetVocalSounds(Entity<AdditionalVocalSoundsComponent> ent, EmoteSoundsPrototype? baseSounds = null )
    {

        var result = baseSounds?.Sounds ?? new Dictionary<string, SoundSpecifier>();

        if (string.IsNullOrEmpty(ent.Comp.AdditionalSounds))
            return result;

        var exists = _prototypeManager.TryIndex(ent.Comp.AdditionalSounds, out var sounds);

        if (!exists || sounds == null || sounds.Sounds.Count == 0)
            return result;

        foreach (var (soundId, specifier) in sounds.Sounds)
            result.TryAdd(soundId, specifier);

        return result;
    }
}
