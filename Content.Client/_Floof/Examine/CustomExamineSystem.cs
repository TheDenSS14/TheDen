// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

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
    [Dependency] private readonly IEntityManager _entityManager = default!;

    private SharedCustomExamineSystem _sharedCustomExamineSystem = default!;
    private CustomExamineSettingsWindow? _window = null;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GetVerbsEvent<Verb>>(OnGetVerbs);
        SubscribeLocalEvent<CustomExamineComponent, AfterAutoHandleStateEvent>(OnStateUpdate);

        _sharedCustomExamineSystem = _entityManager.System<SharedCustomExamineSystem>();
    }

    private void OnGetVerbs(GetVerbsEvent<Verb> args)
    {
        if (_player.LocalEntity is null || !CanChangeExamine(_player.LocalEntity.Value, args.Target))
            return;

        var target = args.Target;
        args.Verbs.Add(new()
        {
            Act = () => OpenUi(target),
            Text = Loc.GetString("custom-examine-verb"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/edit.svg.png")),
            ClientExclusive = true,
            DoContactInteraction = false
        });
    }

    private void OnStateUpdate(Entity<CustomExamineComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        _window?.SetData(ent.Comp.PublicData, ent.Comp.SubtleData);
    }

    private void OpenUi(EntityUid target)
    {
        if (_player.LocalEntity != null
            && !_sharedCustomExamineSystem.CanChangeExamine(target, _player.LocalEntity.Value))
            return;

        if (_window == null)
            EnsureWindow(target);

        // This will create a local component if it didn't exist before, but after sending the data to server it will become shared.
        var comp = EnsureComp<CustomExamineComponent>(target);
        _window?.SetData(comp.PublicData, comp.SubtleData);

        if (_window!.IsOpen)
            _window.Close();
        else
            _window.OpenCenteredAt(new(0.5f, 0.75f)); // mid-top-center
    }

    private void EnsureWindow(EntityUid target)
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
}
