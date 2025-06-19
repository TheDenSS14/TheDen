// SPDX-FileCopyrightText: 2025 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.IO;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Renamable.EntitySystems;
using Robust.Client.UserInterface;

namespace Content.Client.Renamable;

public sealed class RenamableBoundUserInterface : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    private EntityQuery<MetaDataComponent> _metaQuery;

    [ViewVariables]
    private RenamableWindow? _window;

    public RenamableBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _metaQuery = _entManager.GetEntityQuery<MetaDataComponent>();
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<RenamableWindow>();
        _window.OnNameChanged += OnNameChanged;
        Reload();
    }

    private void OnNameChanged(string newName) => SendPredictedMessage(new RenamableBuiMessage(newName));

    private void Reload(MetaDataComponent? metaData = null)
    {
        if (!_metaQuery.Resolve(Owner, ref metaData))
            return;

        _window!.SetCurrentName(metaData.EntityName);
    }
}
