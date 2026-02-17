// SPDX-FileCopyrightText: 2025 Mnemotechnican
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Server.Administration.Logs;
using Content.Shared._Floof.Examine;
using Content.Shared.Database;


namespace Content.Server._Floof.Examine;


public sealed class CustomExamineSystem : SharedCustomExamineSystem
{
    [Dependency] private readonly IAdminLogManager _adminLogManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<SetCustomExamineMessage>(OnSetCustomExamineMessage);
    }

    private void OnSetCustomExamineMessage(SetCustomExamineMessage msg, EntitySessionEventArgs args)
    {
        var target = GetEntity(msg.Target);
        if (args.SenderSession.AttachedEntity == null
            || !CanChangeExamine(args.SenderSession.AttachedEntity.Value, target))
            return;

        var comp = EnsureComp<CustomExamineComponent>(target);

        TrimData(ref msg.PublicData, ref msg.SubtleData);
        comp.PublicData = msg.PublicData;
        comp.SubtleData = msg.SubtleData;

        // DEN start: Logging
        var targetStr = ToPrettyString(target);

        _adminLogManager.Add(LogType.Verb, LogImpact.Low,
            $"{targetStr:user} set custom examine text: {GetExamineDataText(comp.PublicData)}");
        _adminLogManager.Add(LogType.Verb, LogImpact.Low,
            $"{targetStr:user} set subtle examine text: {GetExamineDataText(comp.SubtleData)}");
        // DEN end: Logging

        Dirty(target, comp);
    }

    // DEN start: Logging
    private static string GetExamineDataText(CustomExamineData data)
    {
        var output = "";
        output += "\"" + (data.Content ?? "null") + "\", ";
        output += $"expiry in {data.ExpireTime.TotalMinutes} minutes, ";
        output += $"NSFW required: {data.RequiresConsent}, ";
        output += $"max range: {data.VisibilityRange}";

        return output;
    }
    // DEN end
}
