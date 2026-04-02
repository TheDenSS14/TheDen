// SPDX-FileCopyrightText: 2020 DamianX
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2022 Veritius
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Content.Shared.Preferences;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server.Preferences.Managers
{
    public interface IServerPreferencesManager
    {
        void Init();

        Task LoadData(ICommonSession session, CancellationToken cancel);
        void SanitizeData(ICommonSession session);
        void FinishLoad(ICommonSession session);
        void OnClientDisconnected(ICommonSession session);

        bool TryGetCachedPreferences(NetUserId userId, [NotNullWhen(true)] out PlayerPreferences? playerPreferences);
        PlayerPreferences GetPreferences(NetUserId userId);
        PlayerPreferences? GetPreferencesOrNull(NetUserId? userId);
        IEnumerable<KeyValuePair<NetUserId, ICharacterProfile>> GetSelectedProfilesForPlayers(List<NetUserId> userIds);
        bool HavePreferencesLoaded(ICommonSession session);

        Task<PlayerPreferences?> GetUserPreferences(Guid userId, CancellationToken cancel); // DEN: Get offline user preferences

        Task SetProfile(NetUserId userId, int slot, ICharacterProfile profile);
    }
}
