using Content.Client._DEN.Lobby.UI.Controls;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._DEN.Lobby.UI.Loadouts;

public sealed partial class LoadoutsTab : BoxContainer
{
    public LoadoutsTab()
    {
        var categories = new LoadoutsCategoryPanel()
        {
            HorizontalExpand = true,
            VerticalExpand = true,
            VScrollEnabled = true,
        };

        AddChild(categories);
    }
}
