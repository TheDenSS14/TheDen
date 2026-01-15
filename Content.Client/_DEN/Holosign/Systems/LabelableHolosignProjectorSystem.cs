using Content.Client._DEN.Holosign.UI;
using Content.Shared._DEN.Holosign.Components;
using Content.Shared._DEN.Holosign.Events;
using Content.Shared._DEN.Holosign.Systems;
using Robust.Client.GameObjects;


namespace Content.Client._DEN.Holosign.Systems;


public sealed class LabelableHolosignProjectorSystem : SharedLabelableHolosignProjectorSystem
{
    protected override void UpdateUI(Entity<LabelableHolosignProjectorComponent> ent)
    {
        if (_uiSystem.TryGetOpenUi(ent.Owner, LabelableHolosignUIKey.Key, out var bui)
            && bui is LabelableHolosignProjectorBoundUserInterface cBui)
        {
            cBui.Reload();
        }
    }
}
