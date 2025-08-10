using Content.Shared._DEN.Earmuffs;
using Robust.Client.Timing;
using Range = Robust.Client.UserInterface.Controls.Range;


namespace Content.Client._DEN.Earmuffs;


public sealed class EarmuffsSystem : SharedEarmuffsSystem
{
    public void UpdateEarmuffs(Range range)
    {
        var msg = new EarmuffsUpdated((int) range.Value);
        RaiseNetworkEvent(msg);
    }
}
