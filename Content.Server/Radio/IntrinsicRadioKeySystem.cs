// SPDX-FileCopyrightText: 2024 Timemaster99 <57200767+Timemaster99@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Radio.Components;
using Content.Shared.Radio;
using Content.Shared.Radio.Components;

namespace Content.Server.Radio;

public sealed class IntrinsicRadioKeySystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IntrinsicRadioTransmitterComponent, EncryptionChannelsChangedEvent>(OnTransmitterChannelsChanged);
        SubscribeLocalEvent<ActiveRadioComponent, EncryptionChannelsChangedEvent>(OnReceiverChannelsChanged);
    }

    private void OnTransmitterChannelsChanged(EntityUid uid, IntrinsicRadioTransmitterComponent component, EncryptionChannelsChangedEvent args)
    {
        UpdateChannels(uid, args.Component, ref component.Channels);
    }

    private void OnReceiverChannelsChanged(EntityUid uid, ActiveRadioComponent component, EncryptionChannelsChangedEvent args)
    {
        UpdateChannels(uid, args.Component, ref component.Channels);
    }

    private void UpdateChannels(EntityUid _, EncryptionKeyHolderComponent component, ref HashSet<string> channels)
    {
        channels.Clear();
        channels.UnionWith(component.Channels);
    }
}
