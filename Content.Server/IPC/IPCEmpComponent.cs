using Robust.Shared.GameStates;

namespace Content.Server.Ipc;

[RegisterComponent, NetworkedComponent]
public sealed partial class IpcEmpComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Disabled = false;

    [DataField]
    public float DurationMultiplier = 1f;
}
