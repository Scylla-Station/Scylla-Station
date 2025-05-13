// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Consent;

/// <summary>
/// Defines the different levels of consent a player can choose.
/// </summary>
[Serializable, NetSerializable]
public enum ConsentLevel : sbyte
{
    Ask = -4,
    HardDeny = -3,
    Deny = -2,
    SoftDeny = -1,
    Neutral = 0,
    SoftAllow = 1,
    Allow = 2,
    EnthusiasticAllow = 3,
}
