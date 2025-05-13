// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Shared.Consent.Components;
using Content.Shared.Consent.Prototypes;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.Consent;

/// <summary>
/// Handles shared logic for accessing and interpreting consent preferences.
/// </summary>
public sealed class SharedConsentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ILogManager _logMan = default!;
    private ISawmill _sawmill = default!;

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _domProto = "ConsentDominant";

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _subProto = "ConsentSubmissive";

    public override void Initialize()
    {
        base.Initialize();
        _sawmill = _logMan.GetSawmill("Consents");
        SubscribeLocalEvent<ConsentPreferencesComponent, ComponentStartup>(OnConsentPreferencesStartup);
        SubscribeLocalEvent<ConsentPreferencesComponent, GetVerbsEvent<InteractionVerb>>(AddViewConsentVerb); // Added verb subscription
    }

    private void AddViewConsentVerb(Entity<ConsentPreferencesComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.User == args.Target)
            return;

        var user = args.User;
        var target = args.Target;

        InteractionVerb verb = new()
        {
            Text = Loc.GetString("view-consent-verb-text"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/observe.svg.192dpi.png")),
            Act = () =>
            {
                RaiseNetworkEvent(new RequestViewConsentEvent(GetNetEntity(user), GetNetEntity(target)));
            },
        };
        args.Verbs.Add(verb);
    }


    /// <summary>
    /// Automatically populates prototypes, with <see cref="ConsentLevel"/> set to neutral.
    /// This should mean Urists and such automatically are neutral on everything, so you can... do stuff to them I guess.
    /// </summary>
    private void OnConsentPreferencesStartup(EntityUid uid, ConsentPreferencesComponent component, ComponentStartup args)
    {
        _sawmill.Debug($"OnConsentPreferencesStartup for entity {uid}. Initial Preferences count: {component.Preferences.Count}");

        var prototypes = _prototypeManager.EnumeratePrototypes<ConsentPrototype>();
        _sawmill.Debug($"Found {prototypes.Count()} ConsentPrototypes.");

        foreach (var consentProto in prototypes)
        {
            _sawmill.Debug($"Processing ConsentPrototype: {consentProto.ID}");
            bool added = component.Preferences.TryAdd(new ProtoId<ConsentPrototype>(consentProto.ID), ConsentLevel.Neutral);
            if (!added)
            {
                _sawmill.Warning($"Failed to add or already present: {consentProto.ID} for entity {uid}. Current value: {component.Preferences[new ProtoId<ConsentPrototype>(consentProto.ID)]}");
            }
        }
        _sawmill.Debug($"Finished OnConsentPreferencesStartup for entity {uid}. Final Preferences count: {component.Preferences.Count}");
    }

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
