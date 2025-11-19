using System.Collections.Generic;
using System.Linq;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;

namespace Content.IntegrationTests.Tests._DEN.Reagents;

[TestFixture]
[TestOf(typeof(ReagentPrototype))]
public sealed class RespectConsentReagentTest
{
    private readonly HashSet<string> _consentBreakingReagents =
    [
        "Cum",
        "NaturalLubricant",
    ];

    /// <summary>
    /// Checks to make sure all "lewd" reagents are disabled from being randomly generated (e.g. by Oracle).
    /// </summary>
    [Test]
    public async Task AllLewdReagentsNoRandom()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var client = pair.Client;
        var failedPrototypes = new HashSet<string>(); // HashSet because items are unique

        foreach (var recipe in server.ProtoMan.EnumeratePrototypes<ReactionPrototype>())
        {
            var ingredients = recipe.Reactants.Keys.ToList();
            if (ingredients.Intersect(_consentBreakingReagents).Any()) // yucky...
                foreach (var product in recipe.Products)
                    if (server.ProtoMan.TryIndex<ReagentPrototype>(product.Key, out var reagent)
                        && !reagent.NoRandom)
                        failedPrototypes.Add(product.Key);
        }

        Assert.That(failedPrototypes, Is.Empty,
            $"Found reactions with consent-breaking reactants for randomizable reagents:\n"
            + string.Join(", ", failedPrototypes));

        await pair.CleanReturnAsync();
    }
}
