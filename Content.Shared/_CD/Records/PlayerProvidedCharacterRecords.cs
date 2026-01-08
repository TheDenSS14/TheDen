// SPDX-FileCopyrightText: 2025 Azzy
// SPDX-FileCopyrightText: 2025 Lyndomen
// SPDX-FileCopyrightText: 2025 Shaman
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Linq;
using System.Text.Json.Serialization;
using Robust.Shared.Serialization;

namespace Content.Shared._CD.Records;

/// <summary>
/// Contains Cosmatic Drift records that can be changed in the character editor. This is stored on the character's profile.
/// </summary>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class PlayerProvidedCharacterRecords
{
    public const int TextMedLen = 64;
    public const int TextVeryLargeLen = 65536;

    /* Basic info */

    // Additional data is fetched from the Profile

    // All
    [DataField] // TheDen
    public string Birthday { get; private set; }

    [DataField]
    public string EmergencyContactName { get; private set; }

    [DataField] // TheDen
    public string Residency { get; private set; }

    // Employment
    [DataField]
    public bool HasWorkAuthorization { get; private set; }

    // Security
    [DataField]
    public string IdentifyingFeatures { get; private set; }

    // Medical
    [DataField]
    public string Allergies { get; private set; }
    [DataField]
    public string DrugAllergies { get; private set; }
    [DataField]
    public string PostmortemInstructions { get; private set; }
    // history, prescriptions, etc. would be a record below

    // "incidents"
    [DataField, JsonIgnore]
    public List<RecordEntry> MedicalEntries { get; private set; }
    [DataField, JsonIgnore]
    public List<RecordEntry> SecurityEntries { get; private set; }
    [DataField, JsonIgnore]
    public List<RecordEntry> EmploymentEntries { get; private set; }

    [DataDefinition]
    [Serializable, NetSerializable]
    public sealed partial class RecordEntry
    {
        [DataField]
        public string Title { get; private set; }
        // players involved, can be left blank (or with a generic "CentCom" etc.) for backstory related issues
        [DataField]
        public string Involved { get; private set; }
        // Longer description of events.
        [DataField]
        public string Description { get; private set; }

        public RecordEntry(string title, string involved, string desc)
        {
            Title = title;
            Involved = involved;
            Description = desc;
        }

        public RecordEntry(RecordEntry other)
        : this(other.Title, other.Involved, other.Description)
        {
        }

        public bool MemberwiseEquals(RecordEntry other)
        {
            return Title == other.Title && Involved == other.Involved && Description == other.Description;
        }

        public void EnsureValid()
        {
            Title = ClampString(Title, TextMedLen);
            Involved = ClampString(Involved, TextMedLen);
            Description = ClampString(Description, TextVeryLargeLen);
        }
    }

    public PlayerProvidedCharacterRecords(
        bool hasWorkAuthorization,
        string birthday, // TheDen
        string emergencyContactName,
        string residency, // TheDen
        string identifyingFeatures,
        string allergies, string drugAllergies,
        string postmortemInstructions,
        List<RecordEntry> medicalEntries, List<RecordEntry> securityEntries, List<RecordEntry> employmentEntries)
    {
        HasWorkAuthorization = hasWorkAuthorization;
        Birthday = birthday; // TheDen
        EmergencyContactName = emergencyContactName;
        Residency = residency; // TheDen
        IdentifyingFeatures = identifyingFeatures;
        Allergies = allergies;
        DrugAllergies = drugAllergies;
        PostmortemInstructions = postmortemInstructions;
        MedicalEntries = medicalEntries;
        SecurityEntries = securityEntries;
        EmploymentEntries = employmentEntries;
    }

    public PlayerProvidedCharacterRecords(PlayerProvidedCharacterRecords other)
    {
        Birthday = other.Birthday; // TheDen
        EmergencyContactName = other.EmergencyContactName;
        Residency = other.Residency; // TheDen
        HasWorkAuthorization = other.HasWorkAuthorization;
        IdentifyingFeatures = other.IdentifyingFeatures;
        Allergies = other.Allergies;
        DrugAllergies = other.DrugAllergies;
        PostmortemInstructions = other.PostmortemInstructions;
        MedicalEntries = other.MedicalEntries.Select(x => new RecordEntry(x)).ToList();
        SecurityEntries = other.SecurityEntries.Select(x => new RecordEntry(x)).ToList();
        EmploymentEntries = other.EmploymentEntries.Select(x => new RecordEntry(x)).ToList();
    }

    public static PlayerProvidedCharacterRecords DefaultRecords()
    {
        return new PlayerProvidedCharacterRecords(
            hasWorkAuthorization: true,
            birthday: "", // TheDen
            emergencyContactName: "",
            residency: "", // TheDen
            identifyingFeatures: "",
            allergies: "None",
            drugAllergies: "None",
            postmortemInstructions: "Return home",
            medicalEntries: new List<RecordEntry>(),
            securityEntries: new List<RecordEntry>(),
            employmentEntries: new List<RecordEntry>()
        );
    }

    public bool MemberwiseEquals(PlayerProvidedCharacterRecords other)
    {
        // This is ugly but is only used for integration tests.
        var test = EmergencyContactName == other.EmergencyContactName
                   && Birthday == other.Birthday // TheDen
                   && Residency == other.Residency // TheDen
                   && HasWorkAuthorization == other.HasWorkAuthorization
                   && IdentifyingFeatures == other.IdentifyingFeatures
                   && Allergies == other.Allergies
                   && DrugAllergies == other.DrugAllergies
                   && PostmortemInstructions == other.PostmortemInstructions;
        if (!test)
            return false;
        if (MedicalEntries.Count != other.MedicalEntries.Count)
            return false;
        if (SecurityEntries.Count != other.SecurityEntries.Count)
            return false;
        if (EmploymentEntries.Count != other.EmploymentEntries.Count)
            return false;
        if (MedicalEntries.Where((t, i) => !t.MemberwiseEquals(other.MedicalEntries[i])).Any())
        {
            return false;
        }
        if (SecurityEntries.Where((t, i) => !t.MemberwiseEquals(other.SecurityEntries[i])).Any())
        {
            return false;
        }
        if (EmploymentEntries.Where((t, i) => !t.MemberwiseEquals(other.EmploymentEntries[i])).Any())
        {
            return false;
        }

        return true;
    }

    private static string ClampString(string str, int maxLen)
    {
        if (string.IsNullOrWhiteSpace(str))
            return string.Empty;

        if (str.Length > maxLen)
            return str[..maxLen];

        return str;
    }

    private static void EnsureValidEntries(List<RecordEntry> entries)
    {
        foreach (var entry in entries)
        {
            entry.EnsureValid();
        }
    }

    /// <summary>
    /// Clamp invalid entries to valid values
    /// </summary>
    public void EnsureValid()
    {
        Birthday = ClampString(Birthday, TextMedLen); // TheDen
        EmergencyContactName =
            ClampString(EmergencyContactName, TextMedLen);
        Residency = ClampString(Residency, TextMedLen); // TheDen
        IdentifyingFeatures = ClampString(IdentifyingFeatures, TextMedLen);
        Allergies = ClampString(Allergies, TextMedLen);
        DrugAllergies = ClampString(DrugAllergies, TextMedLen);
        PostmortemInstructions = ClampString(PostmortemInstructions, TextMedLen);

        EnsureValidEntries(EmploymentEntries);
        EnsureValidEntries(MedicalEntries);
        EnsureValidEntries(SecurityEntries);
    }
    public PlayerProvidedCharacterRecords WithWorkAuth(bool auth)
    {
        return new(this) { HasWorkAuthorization = auth };
    }

    public PlayerProvidedCharacterRecords WithBirthday(string birthday) // TheDen
    {
        return new(this) { Birthday = birthday};
    }
    public PlayerProvidedCharacterRecords WithContactName(string name)
    {
        return new(this) { EmergencyContactName = name};
    }
    public PlayerProvidedCharacterRecords WithResidency(string name) // TheDen
    {
        return new(this) { Residency = name};
    }
    public PlayerProvidedCharacterRecords WithIdentifyingFeatures(string feat)
    {
        return new(this) { IdentifyingFeatures = feat};
    }
    public PlayerProvidedCharacterRecords WithAllergies(string s)
    {
        return new(this) { Allergies = s };
    }
    public PlayerProvidedCharacterRecords WithDrugAllergies(string s)
    {
        return new(this) { DrugAllergies = s };
    }
    public PlayerProvidedCharacterRecords WithPostmortemInstructions(string s)
    {
        return new(this) { PostmortemInstructions = s};
    }
    public PlayerProvidedCharacterRecords WithEmploymentEntries(List<RecordEntry> entries)
    {
        return new(this) { EmploymentEntries = entries};
    }
    public PlayerProvidedCharacterRecords WithMedicalEntries(List<RecordEntry> entries)
    {
        return new(this) { MedicalEntries = entries};
    }
    public PlayerProvidedCharacterRecords WithSecurityEntries(List<RecordEntry> entries)
    {
        return new(this) { SecurityEntries = entries};
    }
}

public enum CharacterRecordType : byte
{
    Employment, Medical, Security
}
