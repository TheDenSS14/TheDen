// SPDX-FileCopyrightText: 2020 DamianX
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Javier Guardia Fernández
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2022 DrSmugleaf
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Jezithyr
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 TemporalOroboros
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 Tayrtahn
// SPDX-FileCopyrightText: 2025 portfiend
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Server.Body.Systems;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Rotation;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Body
{
    [TestFixture]
    [TestOf(typeof(BodyPartComponent))]
    [TestOf(typeof(BodyComponent))]
    public sealed class LegTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: HumanBodyAndAppearanceDummy
  id: HumanBodyAndAppearanceDummy
  components:
  - type: Appearance
  - type: Body
    prototype: Human
    requiredLegs: 2
  - type: StandingState
";

        [Test]
        public async Task RemoveLegsFallTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            EntityUid human = default!;
            AppearanceComponent appearance = null;

            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var appearanceSystem = entityManager.System<SharedAppearanceSystem>();
            var xformSystem = entityManager.System<SharedTransformSystem>();

            var map = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                BodyComponent body = null;

                human = entityManager.SpawnEntity("HumanBodyAndAppearanceDummy",
                    new MapCoordinates(Vector2.Zero, map.MapId));

                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(human, out body));
                    Assert.That(entityManager.TryGetComponent(human, out appearance));
                });

                Assert.That(!appearanceSystem.TryGetData(human, RotationVisuals.RotationState, out RotationState _, appearance));

                var bodySystem = entityManager.System<BodySystem>();
                var legs = bodySystem.GetBodyChildrenOfType(human, BodyPartType.Leg, body);

                foreach (var leg in legs)
                {
                    xformSystem.DetachEntity(leg.Id, entityManager.GetComponent<TransformComponent>(leg.Id));
                }
            });

            await server.WaitAssertion(() =>
            {
#pragma warning disable NUnit2045
                // Interdependent assertions.
                Assert.That(appearanceSystem.TryGetData(human, RotationVisuals.RotationState, out RotationState state, appearance));
                Assert.That(state, Is.EqualTo(RotationState.Horizontal));
#pragma warning restore NUnit2045
            });
            await pair.CleanReturnAsync();
        }
    }
}
