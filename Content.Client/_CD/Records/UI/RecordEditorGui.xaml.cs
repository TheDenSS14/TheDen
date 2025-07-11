// SPDX-FileCopyrightText: 2025 Lyndomen <49795619+Lyndomen@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Preferences;
using Content.Shared._CD.Records;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.XAML;
using Robust.Client.UserInterface;
using Serilog;


namespace Content.Client._CD.Records.UI;

/// <summary>
/// The record editor tab that gets "injected" into the character editor.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class RecordEditorGui : Control
{
    /// <summary>
    /// Delegate that tells the editor to save records when the save button is pressed
    /// </summary>
    private Action<PlayerProvidedCharacterRecords>? _updateProfileRecords;
    private PlayerProvidedCharacterRecords _records = default!;

    public RecordEditorGui()
    {
        RobustXamlLoader.Load(this);

        Logger.Info("meow");

        #region General

        ContactNameEdit.OnTextChanged += args =>
        {
            UpdateRecords(_records.WithContactName(args.Text));
        };

        #endregion

        #region Employment

        WorkAuthCheckBox.OnToggled += args =>
        {
            UpdateRecords(_records.WithWorkAuth(args.Pressed));
        };

        #endregion

        #region Security

        IdentifyingFeaturesEdit.OnTextChanged += args =>
        {
            UpdateRecords(_records.WithIdentifyingFeatures(args.Text));
        };

        #endregion

        #region Medical

        AllergiesEdit.OnTextChanged += args =>
        {
            UpdateRecords(_records.WithAllergies(args.Text));
        };

        DrugAllergiesEdit.OnTextChanged += args =>
        {
            UpdateRecords(_records.WithDrugAllergies(args.Text));
        };

        PostmortemEdit.OnTextChanged += args =>
        {
            UpdateRecords(_records.WithPostmortemInstructions(args.Text));
        };

        #endregion

        #region Entries

        EntryEditorTabs.SetTabTitle(0, Loc.GetString("humanoid-profile-editor-cd-records-employment"));
        EntryEditorTabs.SetTabTitle(1, Loc.GetString("department-Medical"));
        EntryEditorTabs.SetTabTitle(2, Loc.GetString("department-Security"));

        EmploymentEntrySelector.OnUpdateEntries += args =>
        {
            UpdateRecords(_records.WithEmploymentEntries(args.Entries));
        };

        MedicalEntrySelector.OnUpdateEntries += args =>
        {
            UpdateRecords(_records.WithMedicalEntries(args.Entries));
        };

        SecurityEntrySelector.OnUpdateEntries += args =>
        {
            UpdateRecords(_records.WithSecurityEntries(args.Entries));
        };

        #endregion
    }

    public void SetRecordUpdateFunction(Action<PlayerProvidedCharacterRecords> updateProfileRecords)
    {
        _updateProfileRecords = updateProfileRecords;
    }

    public void Update(HumanoidCharacterProfile? profile)
    {
        _records = profile?.CDCharacterRecords ?? PlayerProvidedCharacterRecords.DefaultRecords();
        EmploymentEntrySelector.UpdateContents(_records.EmploymentEntries);
        MedicalEntrySelector.UpdateContents(_records.MedicalEntries);
        SecurityEntrySelector.UpdateContents(_records.SecurityEntries);
        UpdateWidgets();
    }

    private void UpdateRecords(PlayerProvidedCharacterRecords records)
    {
        records.EnsureValid();
        _records = records;

        if (_updateProfileRecords != null)
            _updateProfileRecords(_records);

        UpdateWidgets();
    }

    private void UpdateWidgets()
    {
        ContactNameEdit.SetText(_records.EmergencyContactName);

        WorkAuthCheckBox.Pressed = _records.HasWorkAuthorization;

        IdentifyingFeaturesEdit.SetText(_records.IdentifyingFeatures);

        AllergiesEdit.SetText(_records.Allergies);
        DrugAllergiesEdit.SetText(_records.DrugAllergies);
        PostmortemEdit.SetText(_records.PostmortemInstructions);
    }
}
