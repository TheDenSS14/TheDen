// SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Blitz <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.StoryGen;

namespace Content.Server.Paper;

public sealed class PaperRandomStorySystem : EntitySystem
{
    [Dependency] private readonly StoryGeneratorSystem _storyGen = default!;
    [Dependency] private readonly PaperSystem _paper = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PaperRandomStoryComponent, MapInitEvent>(OnMapinit);
    }

    private void OnMapinit(Entity<PaperRandomStoryComponent> paperStory, ref MapInitEvent ev)
    {
        if (!TryComp<PaperComponent>(paperStory, out var paper))
            return;

        if (!_storyGen.TryGenerateStoryFromTemplate(paperStory.Comp.Template, out var story))
            return;

        _paper.SetContent(paperStory.Owner, story, paper);
    }
}
