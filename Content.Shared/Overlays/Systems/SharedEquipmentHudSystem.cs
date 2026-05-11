using Content.Shared.Actions;

namespace Content.Shared.Overlays.Systems;

/// <summary>
/// Provides API for creating and interacting with objectives.
/// </summary>
public abstract class SharedEquipmentHudsSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShowHealthBarsComponent, ControlHudActionEvent>(OnControlHud);
    }
    protected void OnControlHud(EntityUid uid, ShowHealthBarsComponent component, ControlHudActionEvent args)
    {
        Del(uid); // DEBUG
        //future note: spawn a urist, add the ShowHealthBarsComponent to him, and use the ActionControlHud. this will delete the urist
    }
}
public sealed partial class ControlHudActionEvent : InstantActionEvent { }