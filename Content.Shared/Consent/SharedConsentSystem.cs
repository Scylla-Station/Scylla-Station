// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Consent.Components;
using Content.Shared.Consent.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.Consent;

/// <summary>
/// Handles shared logic for accessing and interpreting consent preferences.
/// </summary>
public abstract class SharedConsentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _domProto = "ConsentDominant";

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _subProto = "ConsentSubmissive";

    /// <summary>
    /// Gets the consent level for a specific prototype ID for a given entity.
    /// Defaults to Ask if the entity has no component or no specific preference set.
    /// </summary>
    public ConsentLevel GetConsentLevel(EntityUid uid, ProtoId<ConsentPrototype> consentId, ConsentPreferencesComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return ConsentLevel.Ask;

        return component.Preferences.GetValueOrDefault(consentId, ConsentLevel.Ask);
    }

    /// <summary>
    /// Checks if an action is generally allowed based on the consent level (SoftAllow, Allow, EnthusiasticAllow).
    /// </summary>
    public bool IsConsentAllowed(EntityUid uid, ProtoId<ConsentPrototype> consentId, ConsentPreferencesComponent? component = null)
    {
        var level = GetConsentLevel(uid, consentId, component);
        return level >= ConsentLevel.SoftAllow;
    }

    /// <summary>
    /// Determines a Dom/Sub position based on preferences, weighted by the levels.
    /// Returns true for Dom, false for Sub.
    /// Returns null if relevant prototypes don't exist or preferences aren't set appropriately.
    /// </summary>
    public bool? DetermineDomSubPosition(EntityUid uid, ConsentPreferencesComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return null;

        var domLevel = GetConsentLevel(uid, _domProto, component);
        var subLevel = GetConsentLevel(uid, _subProto, component);

        var domWeight = (int) domLevel + 3;
        var subWeight = (int) subLevel + 3;

        domWeight = Math.Max(0, domWeight);
        subWeight = Math.Max(0, subWeight);

        var totalWeight = domWeight + subWeight;

        if (totalWeight <= 0)
            return null;

        return _random.Next(0, totalWeight) < domWeight;
    }
}
