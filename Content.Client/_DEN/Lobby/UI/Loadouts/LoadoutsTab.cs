using Content.Client._DEN.Lobby.UI.Controls;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._DEN.Lobby.UI.Loadouts;

public sealed partial class LoadoutsTab : BoxContainer
{
    /// <remarks>
    /// This is about how wide the text "Nanotrasen Representative" is. Any smaller and it'll
    /// resize itself unpredictably.
    /// </remarks>
    private const float CategoryPanelWidth = 250f;
    private readonly Thickness _margin = new Thickness(5f, 5f);

    public LoadoutsTab()
    {
        Orientation = LayoutOrientation.Horizontal;
        Align = AlignMode.Begin;
        Margin = _margin;

        var categories = new LoadoutsCategoryPanel()
        {
            HorizontalExpand = false,
            VerticalExpand = true,
            VScrollEnabled = true,
            HorizontalAlignment = HAlignment.Stretch,
            SetWidth = CategoryPanelWidth,
        };

        var itemList = new LoadoutsItemListPanel();

        categories.OnCategorySelected += itemList.SetVisibleCategory;

        AddChild(categories);
        AddChild(itemList);

        categories.SelectLoadoutCategory(null, true);
    }
    public static string GetCategoryName(string categoryId) => Loc.GetString($"loadout-category-{categoryId}");
}
