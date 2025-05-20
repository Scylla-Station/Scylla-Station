// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Scylla.Consent;
using Content.Shared.Scylla.Consent.Components;
using Content.Shared.Scylla.Consent.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;

namespace Content.Server.Scylla.Consent;

public sealed class ViewConsentSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<RequestViewConsentEvent>(OnRequestViewConsent);
    }

    private void OnRequestViewConsent(RequestViewConsentEvent msg, EntitySessionEventArgs args)
    {
        var userUid = GetEntity(msg.User);
        var targetUid = GetEntity(msg.Target);

        if (!userUid.IsValid() || !targetUid.IsValid())
            return;

        if (!TryComp<ActorComponent>(userUid, out var actor))
            return;

        string targetName = "Unknown";
        if (TryComp<MetaDataComponent>(targetUid, out var metaData))
            targetName = metaData.EntityName;

        Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> consentsToShow;
        if (TryComp<ConsentPreferencesComponent>(targetUid, out var consentPreferencesComponent) && consentPreferencesComponent.Preferences != null)
            consentsToShow = consentPreferencesComponent.Preferences;
        else
            consentsToShow = new Dictionary<ProtoId<ConsentPrototype>, ConsentLevel>();

        RaiseNetworkEvent(new OpenConsentWindowClientEvent(targetName, consentsToShow), args.SenderSession);
    }
}

