// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Scylla.Consent.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Scylla.Consent;

/// <summary>
/// Network event sent from server to a specific client to open the consent viewing window
/// with the specified data.
/// </summary>
[Serializable, NetSerializable]
public sealed class OpenConsentWindowClientEvent : EntityEventArgs
{
    public string TargetName { get; }
    public Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> ConsentPreferences { get; }

    public OpenConsentWindowClientEvent(string targetName, Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> consentPreferences)
    {
        TargetName = targetName;
        ConsentPreferences = consentPreferences;
    }
}
