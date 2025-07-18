// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: MPL-2.0


using System.Linq;
using System.Text;
using Content.Client.Stylesheets;
using Content.Client.UserInterface.Controls;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Content.Shared.Clothing.Loadouts.Systems;
using Content.Shared.Labels.Components;
using Content.Shared.Paint;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client._DEN.Lobby.UI.Controls;

/// <summary>
///     A control that represents one loadout item in the loadout item list.
///     Each loadout item button displays their cost, icon, and name. It also displays a guidebook entry
///     button (if it has one) and the "customize" button (if it is currently selected).
///     This button may also be dynamically shown/hidden depending on if it is "unwearable/unusable" or not.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class LoadoutItemButton : StyledButtonGroup
{
    [Dependency] private readonly IEntityManager _entity = default!;

    private readonly SharedAppearanceSystem _appearance;

    /// <summary>
    ///     Fired when this button's loadout preference changes (on/off, appearance, etc).
    /// </summary>
    public event Action<LoadoutPreference>? OnPreferenceChanged;

    /// <summary>
    ///     Fired when the "customize" button is clicked for this button.
    /// </summary>
    public event Action<ProtoId<LoadoutPrototype>>? OnCustomizeToggled;

    /// <summary>
    ///     Fired when the "guidebook" button is clicked.
    /// </summary>
    public event Action<string>? OnOpenGuidebook;

    private const string CustomizeButtonStyleClass = StyleNano.StyleClassAltButton;
    private const string UnwearableStyleClass = StyleBase.ButtonCaution;
    private const string UnusableStyleClass = StyleBase.ButtonDanger;
    private const float MaxTooltipWidth = 650f;

    private readonly Thickness _customizeButtonPadding = new Thickness(5, 0);

    private List<string> _reasons = new();

    /// <summary>
    ///     The loadout prototype represented by this button.
    /// </summary>
    public LoadoutPrototype Loadout;

    /// <summary>
    ///     Whether or not this button is visually currently selected.
    /// </summary>
    public bool Pressed => ItemToggleButton.Pressed;

    /// <summary>
    ///     Whether or not this loadout is "unusable" (character does not meet the requirements to use it).
    /// </summary>
    public bool Unusable => ItemToggleButton.HasStyleClass(UnusableStyleClass);

    /// <summary>
    ///     Whether or not this loadout is "unwearable" (character lacks the slots to equip it).
    /// </summary>
    public bool Unwearable => ItemToggleButton.HasStyleClass(UnwearableStyleClass);

    /// <summary>
    ///     The name of this loadout item.
    /// </summary>
    public string LoadoutName => LoadoutNameLabel.Text ?? string.Empty;

    /// <summary>
    ///     Whether or not this button matches the current search filter.
    ///     Used externally.
    /// </summary>
    public bool MatchFilter = true;

    private LoadoutPreference _preference;

    /// <summary>
    ///     The current preference of this loadout item. This includes whether or not it is selected,
    ///     and custom name, description, color tint, and heirloom status.
    /// </summary>
    /// <remarks>
    ///     This control only changes the "selected" property of the loadout preference; everything else is
    ///     handled in the customization panel.
    /// </remarks>
    public LoadoutPreference Preference
    {
        get => _preference;
        set
        {
            _preference = value;
            ItemToggleButton.Pressed = value.Selected;
            UpdatePressedVisuals();
            UpdatePaint();
        }
    }

    /// <summary>
    ///     The entity being used as this loadout's icon.
    /// </summary>
    public EntityUid? PreviewEntity { get; private set; }

    public LoadoutItemButton(LoadoutPrototype loadout)
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);
        _appearance = _entity.System<SharedAppearanceSystem>();

        CustomizeButton.AddStyleClass(CustomizeButtonStyleClass);
        CustomizeButton.Label.Margin = _customizeButtonPadding;

        Loadout = loadout;
        InitPreview();
        Preference = _preference = new(Loadout.ID);

        LoadoutNameLabel.Text = GetName();
        CostLabel.Text = Loadout.Cost.ToString();
        ItemToggleButton.TooltipSupplier = SupplyTooltip;

        if (!string.IsNullOrWhiteSpace(Loadout.GuideEntry))
        {
            GuidebookButton.Visible = true;
            UpdateStyles();
        }

        ItemToggleButton.OnToggled += OnButtonToggled;
        ItemToggleButton.OnToggled += _ => UpdatePressedVisuals();
        CustomizeButton.OnPressed += _ => OnCustomizeToggled?.Invoke(Loadout.ID);
        GuidebookButton.OnPressed += _ => OpenGuidebook();
    }

    protected override void Deparented()
    {
        if (PreviewEntity != null)
            _entity.QueueDeleteEntity(PreviewEntity);

        base.Deparented();
    }

    /// <summary>
    ///     Set the unusable status of this loadout. Whether or not a loadout is usable is based on
    ///     requiremments - playtime, currently-equipped job, species, etc.
    /// </summary>
    /// <param name="unusable">Whether or not this loadout is unusable.</param>
    /// <param name="reasons">A list of requirements to use this loadout.</param>
    public void SetUnusable(bool unusable, List<string> reasons)
    {
        SetStyleClass(UnusableStyleClass, unusable);
        _reasons = reasons;
    }

    /// <summary>
    ///     Set the unwearable status of this loadout. A loadout is unwearable if it is a clothing item
    ///     that this character doesn't have a slot for, e.g. because of their species.
    /// </summary>
    /// <param name="unwearable">Whether or not this loadout is unwearable.</param>
    public void SetUnwearable(bool unwearable)
    {
        SetStyleClass(UnwearableStyleClass, unwearable);
    }

    /// <summary>
    ///     Set the selected status of this button (without invoking a preference update event).
    /// </summary>
    /// <param name="selected">Whether or not this button should be selected.</param>
    public void SetSelected(bool selected)
    {
        _preference.Selected = selected;
        ItemToggleButton.Pressed = selected;
        UpdatePressedVisuals();
    }

    private void InitPreview()
    {
        if (PreviewEntity != null)
            _entity.QueueDeleteEntity(PreviewEntity);

        var previewProto = Loadout.Items.First();
        PreviewEntity = _entity.Spawn(previewProto, MapCoordinates.Nullspace);
        PreviewSprite.SetEntity(PreviewEntity);
    }

    private void SetStyleClass(string styleClass, bool enabled)
    {
        if (enabled && !ItemToggleButton.HasStyleClass(styleClass))
            ItemToggleButton.AddStyleClass(styleClass);
        else if (!enabled && ItemToggleButton.HasStyleClass(styleClass))
            ItemToggleButton.RemoveStyleClass(styleClass);
    }

    private void UpdatePressedVisuals()
    {
        CustomizeButton.Visible = ItemToggleButton.Pressed;
        UpdateStyles();
    }

    private void UpdatePaint()
    {
        if (!Preference.Selected || PreviewEntity == null || !Loadout.CustomColorTint)
            return;

        var hasColor = _preference.CustomColorTint != null;
        var appearance = _entity.EnsureComponent<AppearanceComponent>(PreviewEntity.Value);
        if (hasColor)
        {
            var paint = _entity.EnsureComponent<PaintedComponent>(PreviewEntity.Value);
            paint.Enabled = true;
            paint.Color = Color.FromHex(_preference.CustomColorTint);
        }
        else
            _entity.RemoveComponent<PaintedComponent>(PreviewEntity.Value);

        _appearance.TryGetData(PreviewEntity.Value, PaintVisuals.Painted, out bool isPainted, appearance);
        _appearance.SetData(PreviewEntity.Value, PaintVisuals.Painted, !isPainted, appearance);
    }

    private void OnButtonToggled(BaseButton.ButtonToggledEventArgs args)
    {
        _preference.Selected = args.Pressed;
        OnPreferenceChanged?.Invoke(_preference);
    }

    private void OpenGuidebook()
    {
        if (string.IsNullOrWhiteSpace(Loadout.GuideEntry))
            return;

        OnOpenGuidebook?.Invoke(Loadout.GuideEntry);
    }

    private Control? SupplyTooltip(Control sender)
    {
        var formattedTooltip = new Tooltip() { MaxWidth = MaxTooltipWidth };
        var tooltipText = new StringBuilder();
        var desc = GetDescription();

        if (!string.IsNullOrEmpty(desc))
            tooltipText.Append(desc);

        if (_reasons.Count > 0)
            tooltipText.Append("\n");

        foreach (var reason in _reasons)
            tooltipText.Append($"\n{reason}");

        if (tooltipText.Length > 0)
        {
            var message = FormattedMessage.FromMarkupPermissive(tooltipText.ToString());
            formattedTooltip.SetMessage(message);
        }

        return formattedTooltip;
    }

    private string GetName()
    {
        var locId = $"loadout-name-{Loadout.ID}";
        var name = Loc.GetString(locId);

        if (name == locId && PreviewEntity != null)
            name = _entity.GetComponent<MetaDataComponent>(PreviewEntity.Value).EntityName;

        if (_entity.TryGetComponent<LabelComponent>(PreviewEntity, out var label))
        {
            var itemLabel = label.CurrentLabel;
            if (!string.IsNullOrEmpty(itemLabel))
                name += $" ({Loc.GetString(itemLabel)})";
        }

        return name;
    }

    private string GetDescription()
    {
        var locId = $"loadout-description-{_preference?.LoadoutName}";
        var description = Loc.GetString(locId);

        if (description == locId && PreviewEntity != null)
            description = _entity.GetComponent<MetaDataComponent>(PreviewEntity.Value).EntityDescription;

        return description;
    }
}
