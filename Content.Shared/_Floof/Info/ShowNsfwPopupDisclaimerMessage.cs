using Robust.Shared.Serialization;


namespace Content.Shared.FloofStation.Info;


/// <summary>
///     Sent server->client to command the client to open an NSFW content disclaimer dialog.
/// </summary>
[Serializable, NetSerializable]
public sealed class ShowNsfwPopupDisclaimerMessage : EntityEventArgs;
