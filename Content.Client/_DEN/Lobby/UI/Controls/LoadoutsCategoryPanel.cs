using System.Linq;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Prototypes;

namespace Content.Client._DEN.Lobby.UI.Controls;

/// <summary>
///     This control creates a list of loadout categories.
///     Clicking on a "leaf" category selects that tab and prompts the item list to update;
///     otherwise, clicking on a "branch" category will show that category's subcategories.
/// </summary>
/// <remarks>
///     This is a vertical list because the horizontal list of categories caused us a lot of
///     suffering. In general, a vertical list of buttons is far more scalable; a vertical
///     scrollbar is way more intuitive than a horizontal one.
///
///     I am actually not a huge fan of this navigation. I think it may be
///     possible to do something more like the guidebook with its dropdowns, but
///     the guidebook also notably suffers when these entry names get too long.
///     It might be okay for loadout categories though.
///     TODO I guess.
///
///     In the meantime, this navigates not unlike a computer's folder structure.
/// </remarks>
public sealed partial class LoadoutsCategoryPanel : ScrollContainer
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    /// <summary>
    ///     Fires when a category button is clicked, indicating the loadout category has been selected.
    /// </summary>
    public event Action<ProtoId<LoadoutCategoryPrototype>?>? OnCategorySelected;

    private const string CategoryBoxNamePrefix = "CategoryList";
    private const string CategoryButtonNamePrefix = "CategoryButton";
    private const string RootCategoryId = "Root";
    private const string RootCategoryName = "loadouts-category-panel-root-category";
    private const string ReturnToCategoryString = "loadouts-category-panel-return-button";

    private Dictionary<ProtoId<LoadoutCategoryPrototype>, LoadoutCategoryList> _subcategoryBoxes = new();
    private LoadoutCategoryList _rootCategoryBox;
    private ButtonGroup _categoryGroup = new(isNoneSetAllowed: true);

    private ProtoId<LoadoutCategoryPrototype>? _visibleCategory = null;
    private ProtoId<LoadoutCategoryPrototype>? _currentCategory = null;

    public LoadoutsCategoryPanel()
    {
        IoCManager.InjectDependencies(this);

        var rootCategories = _prototype.EnumeratePrototypes<LoadoutCategoryPrototype>()
            .Where(c => c.Root)
            .ToList();

        _rootCategoryBox = CreateCategoryListBox(null, null);
        PopulateCategoryListBox(_rootCategoryBox, rootCategories, null);
        AddChild(_rootCategoryBox);

        SelectLoadoutCategory(null, true);
    }

    #region Initialization

    private void PopulateCategoryListBox(LoadoutCategoryList categoryBox,
        List<LoadoutCategoryPrototype> categories,
        LoadoutCategoryPrototype? parentCategory)
    {
        foreach (var category in categories)
        {
            var button = CreateCategoryButton(category);
            categoryBox.AddChild(button);

            if (category.SubCategories.Count > 0)
            {
                var subcategoryBox = CreateCategoryListBox(category, parentCategory);
                PopulateCategoryListBox(subcategoryBox, category.SubCategories, category);
                AddChild(subcategoryBox);
                _subcategoryBoxes.Add(category.ID, subcategoryBox);
            }
            else if (categoryBox.FirstLeafButton == null)
            {
                categoryBox.FirstLeafCategory = category;
                categoryBox.FirstLeafButton = button;
            }
        }
    }

    private void PopulateCategoryListBox(LoadoutCategoryList categoryBox,
        List<ProtoId<LoadoutCategoryPrototype>> categoryIds,
        LoadoutCategoryPrototype? parentCategory)
    {
        var categories = categoryIds
            .Select(id => _prototype.Index(id))
            .ToList();

        PopulateCategoryListBox(categoryBox, categories, parentCategory);
    }

    private LoadoutCategoryList CreateCategoryListBox(LoadoutCategoryPrototype? category,
        LoadoutCategoryPrototype? parent)
    {
        var categoryBoxName = CategoryBoxNamePrefix + (category?.ID ?? RootCategoryId);
        var categoryLabelText = category != null
            ? GetCategoryName(category.ID)
            : Loc.GetString(RootCategoryName);

        var categoryList = new LoadoutCategoryList()
        {
            Name = categoryBoxName,
            Visible = _currentCategory == category?.ID,
        };

        categoryList.SetLabelText(categoryLabelText);

        if (category != null)
            categoryList.AddChild(CreateReturnButton(parent));

        return categoryList;
    }

    private Button CreateCategoryButton(LoadoutCategoryPrototype category)
    {
        var labelText = GetCategoryName(category.ID);
        var isLeaf = category.SubCategories.Count == 0;

        // TODO: Find a better way to denote branches/categories.
        if (!isLeaf)
            labelText += " ...";

        var button = new Button()
        {
            HorizontalExpand = true,
            Text = labelText,
            ToggleMode = isLeaf,
            Group = isLeaf ? _categoryGroup : null,
            Name = CategoryButtonNamePrefix + category.ID,
        };

        button.OnPressed += _ => SelectLoadoutCategory(category);
        return button;
    }

    private Button CreateReturnButton(LoadoutCategoryPrototype? parent)
    {
        var parentName = parent != null
            ? GetCategoryName(parent.ID)
            : Loc.GetString(RootCategoryName);

        // TODO: Give the return button a distinctive style.
        var returnButton = new Button()
        {
            HorizontalExpand = true,
            Text = Loc.GetString(ReturnToCategoryString, ("parent", parentName)),
        };

        returnButton.OnPressed += _ => SelectLoadoutCategory(parent);
        return returnButton;
    }

    #endregion Initialization
    #region Callbacks

    private void SelectLoadoutCategory(LoadoutCategoryPrototype? category, bool force = false)
    {
        _currentCategory = category?.ID;
        OnCategorySelected?.Invoke(_currentCategory);

        if (category != null && category.SubCategories.Count == 0)
            return;

        UpdateVisibleCategoryList(force);
    }

    private void UpdateVisibleCategoryList(bool force = false)
    {
        var currentVisibleBox = GetCategoryListBox(_visibleCategory);
        var newVisibleBox = GetCategoryListBox(_currentCategory);

        if (force || currentVisibleBox != newVisibleBox)
        {
            currentVisibleBox.Visible = false;
            _visibleCategory = _currentCategory;
            newVisibleBox.Visible = true;

            // Deselect buttons if we move through the hierarchy.
            if (_categoryGroup.Pressed != null)
                _categoryGroup.Pressed.Pressed = false;

            // Select the first non-branch button. This causes a redundant update but the UX
            // (of selecting the first category in the newly-visible panel) is worth it I feel.
            if (newVisibleBox.FirstLeafButton != null)
            {
                newVisibleBox.FirstLeafButton.Pressed = true;
                SelectLoadoutCategory(newVisibleBox.FirstLeafCategory);
            }
        }
    }

    #endregion Callbacks
    #region Helpers

    private LoadoutCategoryList GetCategoryListBox(ProtoId<LoadoutCategoryPrototype>? category)
    {
        var categoryBox = _rootCategoryBox;

        if (category != null && _subcategoryBoxes.TryGetValue(category.Value, out var currBox))
            categoryBox = currBox;

        return categoryBox;
    }

    private string GetCategoryName(string categoryId) => Loc.GetString($"loadout-category-{categoryId}");

    #endregion Helpers
}
