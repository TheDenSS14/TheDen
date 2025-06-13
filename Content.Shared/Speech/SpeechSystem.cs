using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Shared.Speech
{
    public sealed class SpeechSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<SpeakAttemptEvent>(OnSpeakAttempt);
        }

        public void SetSpeech(EntityUid uid, bool value, SpeechComponent? component = null)
        {
            if (value && !Resolve(uid, ref component))
                return;

            component = EnsureComp<SpeechComponent>(uid);

            if (component.Enabled == value)
                return;

            component.Enabled = value;

            Dirty(uid, component);
        }

        public void AddAllowedEmote(Entity<SpeechComponent?> ent, ProtoId<EmotePrototype> emoteId)
        {
            if (!Resolve(ent.Owner, ref ent.Comp, false)
                || !_prototypeManager.TryIndex(emoteId, out _)
                || ent.Comp.AllowedEmotes.Contains(emoteId))
                return;

            ent.Comp.AllowedEmotes.Add(emoteId);
            Dirty(ent);
        }

        private void OnSpeakAttempt(SpeakAttemptEvent args)
        {
            if (!TryComp(args.Uid, out SpeechComponent? speech) || !speech.Enabled)
                args.Cancel();
        }
    }
}
