using Content.Shared.Instruments;

namespace Content.Shared._DEN.Instruments;

/// <summary>
/// This component dynamically changes the Instrument component of this object based on the entity inserted into a slot.
/// </summary>
[RegisterComponent]
public sealed partial class InstrumentStandComponent : Component
{
    [DataField]
    public string SlotId = "instrument_slot";

    public SharedInstrumentComponent? Instrument = null;
}
