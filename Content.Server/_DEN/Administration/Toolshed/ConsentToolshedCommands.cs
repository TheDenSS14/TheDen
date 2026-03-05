// SPDX-FileCopyrightText: 2026 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Server._Floof.Consent;
using Content.Server.Administration;
using Content.Shared._Floof.Consent;
using Content.Shared.Administration;
using Content.Shared.Mind.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed;


namespace Content.Server._DEN.Administration.Toolshed;

[ToolshedCommand, AdminCommand(AdminFlags.Admin)]
internal sealed class ConsentCommand : ToolshedCommand
{
    private ConsentSystem? _consentSystem;

    [CommandImplementation("with")]
    public IEnumerable<EntityUid> With([PipedArgument] IEnumerable<EntityUid> input, ProtoId<ConsentTogglePrototype> consentToggle)
    {
        _consentSystem ??= GetSys<ConsentSystem>();
        return input.Where(uid => HasConsent(uid, consentToggle));
    }

    [CommandImplementation("without")]
    public IEnumerable<EntityUid> Without([PipedArgument] IEnumerable<EntityUid> input, ProtoId<ConsentTogglePrototype> consentToggle)
    {
        _consentSystem ??= GetSys<ConsentSystem>();
        return input.Where(uid => !HasConsent(uid, consentToggle));
    }

    private bool HasConsent(EntityUid input, ProtoId<ConsentTogglePrototype> consentToggle)
    {
        return TryComp(input, out MindContainerComponent? mindContainer)
            && _consentSystem!.HasConsent((input, mindContainer), consentToggle);
    }

    private bool DoesNotConsent(EntityUid input, ProtoId<ConsentTogglePrototype> consentToggle)
    {
        return TryComp(input, out MindContainerComponent? mindContainer)
            && !_consentSystem!.HasConsent((input, mindContainer), consentToggle);
    }
}
