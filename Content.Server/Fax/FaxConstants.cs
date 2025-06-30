// SPDX-FileCopyrightText: 2022 Morb <14136326+Morb0@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 2023 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

namespace Content.Server.Fax;

public static class FaxConstants
{
    // Commands

    /**
     * Used to get other faxes connected to current network
     */
    public const string FaxPingCommand = "fax_ping";

    /**
     * Used as response to ping command
     */
    public const string FaxPongCommand = "fax_pong";

    /**
     * Used when fax sending data to destination fax
     */
    public const string FaxPrintCommand = "fax_print";

    // Data

    public const string FaxNameData = "fax_data_name";
    public const string FaxPaperNameData = "fax_data_title";
    public const string FaxPaperLabelData = "fax_data_label";
    public const string FaxPaperPrototypeData = "fax_data_prototype";
    public const string FaxPaperContentData = "fax_data_content";
    public const string FaxPaperStampStateData = "fax_data_stamp_state";
    public const string FaxPaperStampedByData = "fax_data_stamped_by";
    public const string FaxSyndicateData = "fax_data_i_am_syndicate";
}
