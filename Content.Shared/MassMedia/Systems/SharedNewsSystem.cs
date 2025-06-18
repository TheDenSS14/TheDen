// SPDX-FileCopyrightText: 2023 MishaUnity <81403616+MishaUnity@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Serialization;

namespace Content.Shared.MassMedia.Systems;

public abstract class SharedNewsSystem : EntitySystem
{
    public const int MaxTitleLength = 50;
    public const int MaxContentLength = 2560;
}

[Serializable, NetSerializable]
public struct NewsArticle
{
    [ViewVariables(VVAccess.ReadWrite)]
    public string Title;

    [ViewVariables(VVAccess.ReadWrite)]
    public string Content;

    [ViewVariables(VVAccess.ReadWrite)]
    public string? Author;

    [ViewVariables]
    public ICollection<(NetEntity, uint)>? AuthorStationRecordKeyIds;

    [ViewVariables]
    public TimeSpan ShareTime;
}

[ByRefEvent]
public record struct NewsArticlePublishedEvent(NewsArticle Article);

[ByRefEvent]
public record struct NewsArticleDeletedEvent;
