// SPDX-FileCopyrightText: 2026 portfiend
//
// SPDX-License-Identifier: MIT

using Content.Server._DEN.Speech.Components;
using Content.Server.Chat.Systems;
using Content.Server.Speech;

namespace Content.Server._DEN.Speech.EntitySystems;

/// <summary>
///     An accent system that allows you to add a prefix and/or suffix to a chat message.
///     This may optionally be processed alongside other accents (giving it the potential to be affected
///     by stuttering, et cetera) or after all accents (for things like formatting tags.)
/// </summary>
public sealed class PrefixSuffixAccentSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PrefixSuffixAccentComponent, AccentGetEvent>(OnGetAccent);
        SubscribeLocalEvent<TransformSpeechEvent>(OnTransformedSpeech, after: [typeof(AccentSystem)]);
    }

    /// <summary>
    ///     Applies a prefix/suffix to a message during the overall accent retrieval event.
    /// </summary>
    /// <remarks>
    ///     This means that your prefix/suffix can be affected by other accents, like stuttering.
    /// </remarks>
    /// <param name="ent">The entity with this accent.</param>
    private void OnGetAccent(Entity<PrefixSuffixAccentComponent> ent, ref AccentGetEvent args)
    {
        // If this is unaffected by accents, it should be processed in TransformSpeechEvent.
        if (!ent.Comp.AffectedByAccents)
            return;

        var newMessage = args.Message;
        TransformMessage(ref newMessage, ent.Comp);
        args.Message = newMessage;
    }

    /// <summary>
    ///     Applies a prefix/suffix to a message after all accents are processed.
    /// </summary>
    /// <remarks>
    ///     This means your prefix/suffix won't be affected by accents - good for formatting tags.
    /// </remarks>
    /// <param name="ent">The entity with this accent.</param>
    private void OnTransformedSpeech(TransformSpeechEvent args)
    {
        // Is there a better way to do this...?
        if (!TryComp<PrefixSuffixAccentComponent>(args.Sender, out var component))
            return;

        // If this is affected by accents, it should be processed in AccentGetEvent.
        if (component.AffectedByAccents)
            return;

        TransformMessage(ref args.Message, component);
    }

    /// <summary>
    ///     Adds a prefix and/or suffix to a message.
    /// </summary>
    /// <param name="message">The unmodified message.</param>
    /// <param name="comp">The prefix/suffix component.</param>
    private static void TransformMessage(ref string message, PrefixSuffixAccentComponent comp)
    {
        message = comp.Prefix + message + comp.Suffix;
    }
}
