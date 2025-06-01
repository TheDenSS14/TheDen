using Content.Server.Emp;
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
        SubscribeLocalEvent<ActiveRadioComponent, EmpPulseEvent>(OnEmpPulse);
        SubscribeLocalEvent<IntrinsicRadioTransmitterComponent, EmpPulseEvent>(OnEmpPulse);
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

    private void OnEmpPulse(EntityUid uid, ActiveRadioComponent component, ref EmpPulseEvent args)
    {
        if (component.Enabled)
        {
            args.Affected = true;
            args.Disabled = true;
        }
    }

    private void OnEmpPulse(EntityUid uid, IntrinsicRadioTransmitterComponent component, ref EmpPulseEvent args)
    {
        if (component.Enabled)
        {
            args.Affected = true;
            args.Disabled = true;
        }
    }
}
