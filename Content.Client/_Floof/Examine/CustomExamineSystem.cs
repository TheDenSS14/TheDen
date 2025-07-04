using System.Numerics;
using Content.Client.Strip;
using Content.Shared._Floof.Examine;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Client.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;


namespace Content.Client._Floof.Examine;


public sealed class CustomExamineSystem : SharedCustomExamineSystem
{
    [Dependency] private IPlayerManager _player = default!;
    [Dependency] private IGameTiming _timing = default!;

    private CustomExamineSettingsWindow? _window = null;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GetVerbsEvent<Verb>>(OnGetVerbs);
        SubscribeLocalEvent<ActivateInWorldEvent>(OnActivateInWorld, after: [typeof(StrippableSystem)]);
        SubscribeLocalEvent<CustomExamineComponent, AfterAutoHandleStateEvent>(OnStateUpdate);
    }

    private void OnGetVerbs(GetVerbsEvent<Verb> args)
    {
        if (_player.LocalSession is null || !CanChangeExamine(_player.LocalSession, args.Target))
            return;

        var target = args.Target;
        args.Verbs.Add(new Verb
        {
            Act = () => OpenUi(target),
            Text = Loc.GetString("custom-examine-verb"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/edit.svg.png")),
            ClientExclusive = true,
            DoContactInteraction = false
        });
    }

    private void OnActivateInWorld(ActivateInWorldEvent ev)
    {
        // This one works only if user == target, because otherwise it would conflict with stripping ui
        if (ev.User != ev.Target || _player.LocalEntity != ev.User || ev.Handled)
            return;

        if (!_timing.IsFirstTimePredicted)
            return;

        OpenUi(ev.Target);
    }

    private void OnStateUpdate(Entity<CustomExamineComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        if (_window == null)
            return;

        _window.SetData(ent.Comp.PublicData, ent.Comp.SubtleData);
    }

    private void OpenUi(EntityUid target)
    {
        if (_window == null)
        {
            _window = new();
            _window.Public.MaxContentLength = PublicMaxLength;
            _window.Subtle.MaxContentLength = SubtleMaxLength;
            _window.OnClose += () => _window = null;

            _window.OnReset += () =>
            {
                if (TryComp<CustomExamineComponent>(target, out var comp2))
                    _window.SetData(comp2.PublicData, comp2.SubtleData, force: true);
            };
            _window.OnSave += (data) =>
            {
                var ev = new SetCustomExamineMessage
                {
                    PublicData = data.publicData,
                    SubtleData = data.subtleData,
                    Target = GetNetEntity(target)
                };
                RaiseNetworkEvent(ev);
            };
        }

        // This will create a local component if it didn't exist before, but after sending the data to server it will become shared.
        var comp = EnsureComp<CustomExamineComponent>(target);
        _window.SetData(comp.PublicData, comp.SubtleData);

        if (_window.IsOpen)
            _window.Close();
        else
            _window.OpenCenteredAt(new(0.5f, 0.75f)); // mid-top-center
    }
}
