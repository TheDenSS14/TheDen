using Content.Shared._DEN.Earmuffs;

namespace Content.Client._DEN.Earmuffs;


public sealed class EarmuffsSystem : SharedEarmuffsSystem
{
    public void UpdateEarmuffs(float range)
    {
        var msg = new EarmuffsUpdated(range);
        RaiseNetworkEvent(msg);
    }
}
