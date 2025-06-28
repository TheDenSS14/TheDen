// SPDX-FileCopyrightText: 2021 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Debug <49997488+DebugOk@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.Player;


namespace Content.Server.Voting
{
    /// <summary>
    ///     Options for creating a vote.
    /// </summary>
    public sealed class VoteOptions
    {
        /// <summary>
        ///     The text that is shown for "who called the vote".
        /// </summary>
        public string InitiatorText { get; set; } = "<placeholder>";

        /// <summary>
        ///     The player that started the vote. Used to keep track of player cooldowns to avoid vote spam.
        /// </summary>
        public ICommonSession? InitiatorPlayer { get; set; }

        /// <summary>
        ///     The shown title of the vote.
        /// </summary>
        public string Title { get; set; } = "<somebody forgot to fill this in lol>";

        /// <summary>
        ///     How long the vote lasts.
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        ///     How long the initiator should be timed out from calling votes. Defaults to duration * 2;
        /// </summary>
        public TimeSpan? InitiatorTimeout { get; set; }

        /// <summary>
        ///     The options of the vote. Each entry is a tuple of the player-shown text,
        ///     and a data object that can be used to keep track of options later.
        /// </summary>
        public List<(string text, object data)> Options { get; set; } = new();

        /// <summary>
        ///     Whether the iconic vote sound should play or not.
        /// </summary>
        public bool PlayVoteSound { get; set; } = true;

        /// <summary>
        ///     Sets <see cref="InitiatorPlayer"/> and <see cref="InitiatorText"/>
        ///     by setting the latter to the player's name.
        /// </summary>
        public void SetInitiator(ICommonSession player)
        {
            InitiatorPlayer = player;
            InitiatorText = player.Name;
        }

        public void SetInitiatorOrServer(ICommonSession? player)
        {
            if (player != null)
            {
                SetInitiator(player);
            }
            else
            {
                InitiatorText = Loc.GetString("vote-options-server-initiator-text");
            }
        }
    }
}
