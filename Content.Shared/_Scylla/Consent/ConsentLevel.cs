// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Scylla.Consent;

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

/// <summary>
/// Provides helper methods for consent levels.
/// </summary>
public static class ConsentLevelHelpers
{
    public static string GetConsentLevelText(ConsentLevel level)
    {
        return level switch
        {
            ConsentLevel.Ask => Loc.GetString("consent-level-ask"),
            ConsentLevel.HardDeny => Loc.GetString("consent-level-hard-deny"),
            ConsentLevel.Deny => Loc.GetString("consent-level-deny"),
            ConsentLevel.SoftDeny => Loc.GetString("consent-level-soft-deny"),
            ConsentLevel.Neutral => Loc.GetString("consent-level-neutral"),
            ConsentLevel.SoftAllow => Loc.GetString("consent-level-soft-allow"),
            ConsentLevel.Allow => Loc.GetString("consent-level-allow"),
            ConsentLevel.EnthusiasticAllow => Loc.GetString("consent-level-enthusiastic-allow"),
            _ => level.ToString()
        };
    }

    public static Color GetColorForConsentLevel(ConsentLevel level)
    {
        return level switch
        {
            ConsentLevel.HardDeny => Color.FromHex("#6B0000"),
            ConsentLevel.Deny => Color.FromHex("#A00000"),
            ConsentLevel.SoftDeny => Color.FromHex("#D06060"),
            ConsentLevel.Neutral => Color.FromHex("#505058"),
            ConsentLevel.Ask => Color.FromHex("#303038"),
            ConsentLevel.SoftAllow => Color.FromHex("#60B060"),
            ConsentLevel.Allow => Color.FromHex("#008000"),
            ConsentLevel.EnthusiasticAllow => Color.FromHex("#00A000"),
            _ => Color.FromHex("#303038")
        };
    }
}
