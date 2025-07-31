namespace Content.Server._DEN.Announcements;

/// <summary>
/// Alerts players of an incoming ion storm.
/// </summary>
[RegisterComponent]
public sealed partial class IonStormNotifierComponent : Component
{
    /// <summary>
    /// The chance that the synth is alerted of an ion storm
    /// </summary>
    [DataField]
    public float Chance = 0.7f; // TheDen - 0.7, was 0.3

    /// <summary>
    /// Text to display when alert occurs
    /// </summary>
    [DataField]
    public string Loc = "station-event-ion-storm-synth";
}
