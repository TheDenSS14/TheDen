using System.Collections.Generic;
using System.Linq;
using Content.Shared.Clothing.Loadouts.Prototypes;
using Content.Shared.Customization.Systems;
using Content.Shared.Prototypes;
using Content.Shared.Traits;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests._DEN.ItemGroups;

[TestFixture]
[TestOf(typeof(CharacterItemGroupPrototype))]
public sealed class CharacterItemGroupTest
{
    /// <summary>
    /// Check if all loadouts that have CharacterItemGroupRequirements are also in the required group.
    /// </summary>
    [Test]
    public async Task LoadoutsInRequiredItemGroup()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var client = pair.Client;
        var serverCompFac = server.ResolveDependency<IComponentFactory>();
        var failingPrototypes = new Dictionary<string, string>();

        foreach (var loadout in server.ProtoMan.EnumeratePrototypes<LoadoutPrototype>())
            IsInGroup(loadout.ID, loadout.Requirements, ref failingPrototypes, server.ProtoMan);

        Assert.That(failingPrototypes, Is.Empty,
            $"The following loadouts do not exist in a required CharacterItemGroup:\n"
            + string.Join("\n  ", failingPrototypes));

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Check if all traits that have CharacterItemGroupRequirements are also in the required group.
    /// </summary>
    [Test]
    public async Task TraitsInRequiredItemGroup()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var client = pair.Client;
        var serverCompFac = server.ResolveDependency<IComponentFactory>();
        var failingPrototypes = new Dictionary<string, string>();

        foreach (var trait in server.ProtoMan.EnumeratePrototypes<TraitPrototype>())
            IsInGroup(trait.ID, trait.Requirements, ref failingPrototypes, server.ProtoMan);

        Assert.That(failingPrototypes, Is.Empty,
            $"The following traits do not exist in a required CharacterItemGroup:\n"
            + string.Join("\n  ", failingPrototypes));

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Check if all items in CharacterItemGroupPrototypes require the group as well.
    /// </summary>
    [Test]
    public async Task ItemsInGroupHaveRequirement()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var client = pair.Client;
        var serverCompFac = server.ResolveDependency<IComponentFactory>();
        var failingPrototypes = new Dictionary<string, List<string>>();

        foreach (var itemGroup in server.ProtoMan.EnumeratePrototypes<CharacterItemGroupPrototype>())
        {
            var failed = new List<string>();

            foreach (var item in itemGroup.Items)
            {
                var prototypeId = item.ID;
                IPrototype prototype = item.Type switch
                {
                    "loadout" => server.ProtoMan.TryIndex<LoadoutPrototype>(prototypeId, out var loadout) ? loadout : null,
                    "trait" => server.ProtoMan.TryIndex<TraitPrototype>(prototypeId, out var trait) ? trait : null,
                    _ => throw new ArgumentOutOfRangeException(item.Type)
                };

                // just gonna ignore invalid ids really. whatever
                if (prototype is null)
                    continue;

                var requirements = prototype switch
                {
                    LoadoutPrototype loadout => loadout.Requirements,
                    TraitPrototype trait => trait.Requirements,
                    _ => throw new ArgumentOutOfRangeException(prototype.GetType().ToString())
                };

                if (!requirements.OfType<CharacterItemGroupRequirement>().Any(req => req.Group == itemGroup.ID))
                    failed.Add($"{prototype.ID} ({item.Type})");
            }

            if (failed.Count > 0)
                failingPrototypes.Add(itemGroup.ID, failed);
        }

        Assert.That(failingPrototypes, Is.Empty,
            "The following CharacterItemGroups have items that lack requirements:\n"
            + string.Join("\n  ", failingPrototypes
                .Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value)}")));

        await pair.CleanReturnAsync();
    }

    private static void IsInGroup(string id,
        List<CharacterRequirement> requirements,
        ref Dictionary<string, string> failingPrototypes,
        IPrototypeManager protoMan)
    {
        var failed = new List<string>();

        foreach (var groupRequirement in requirements.OfType<CharacterItemGroupRequirement>())
        {
            var groupExists = protoMan.TryIndex(groupRequirement.Group, out var itemGroup);
            Assert.That(groupExists, $"CharacterItemGroup with ID {groupRequirement.Group} does not exist!");

            if (!groupExists)
                continue;

            if (!itemGroup.Items.Any(item => item.Type == "loadout" && item.ID == id))
                failed.Add(groupRequirement.Group);
        }

        if (failed.Count > 0)
            failingPrototypes.Add(id, string.Join(", ", failed));
    }
}
