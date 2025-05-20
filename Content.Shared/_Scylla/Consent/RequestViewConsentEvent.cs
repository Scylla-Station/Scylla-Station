// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Scylla.Consent;

/// <summary>
/// Raised from client via verb action to request the server open the View Consent UI.
/// </summary>
[Serializable, NetSerializable]
public sealed class RequestViewConsentEvent : EntityEventArgs
{
    public NetEntity User { get; }
    public NetEntity Target { get; }

    public RequestViewConsentEvent(NetEntity user, NetEntity target)
    {
        User = user;
        Target = target;
    }
}
