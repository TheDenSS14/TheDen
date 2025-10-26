using Content.Server.Bed.Cryostorage;


namespace Content.Server._DEN.Bed.Cryostorage;

/// <summary>
/// Marker component that indicates this user wants to no longer receive messages when people enter cryo.
/// </summary>
[RegisterComponent, Access(typeof(CryostorageSystem))]
public sealed partial class IgnoringCryoMessagesComponent : Component { }
