// SPDX-FileCopyrightText: 2022 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Resources;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;

namespace Content.Client.UserInterface.XamlExtensions;


[PublicAPI]
public sealed class TexExtension
{
    private IResourceCache _resourceCache;
    public string Path { get; }

    public TexExtension(string path)
    {
        _resourceCache = IoCManager.Resolve<IResourceCache>();
        Path = path;
    }

    public object ProvideValue()
    {
        return _resourceCache.GetTexture(Path);
    }
}
