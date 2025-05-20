// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Scylla.Consent.Components;
using Content.Shared.Scylla.Consent.Prototypes;
using Content.Shared.Examine;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Scylla.Consent;

/// <summary>
/// Handles shared logic for accessing and interpreting consent preferences.
/// </summary>
public sealed class SharedConsentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _domProto = "ConsentDominant";

    [ValidatePrototypeId<ConsentPrototype>]
    ProtoId<ConsentPrototype> _subProto = "ConsentSubmissive";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ConsentPreferencesComponent, ComponentStartup>(OnConsentPreferencesStartup);
        SubscribeLocalEvent<ConsentPreferencesComponent, GetVerbsEvent<ExamineVerb>>(AddViewConsentVerb);
    }

    private void AddViewConsentVerb(Entity<ConsentPreferencesComponent> ent, ref GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var user = args.User;
        var target = args.Target;
        var detailsRange = _examine.IsInDetailsRange(args.User, ent);

        ExamineVerb verb = new()
        {
            Text = Loc.GetString("view-consent-verb-text"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/information.svg.192dpi.png")),
            Category = VerbCategory.Examine,
            Disabled = !detailsRange,
            Message = detailsRange ? null : Loc.GetString("detail-examinable-verb-disabled"),
            Act = () =>
            {
                if (_timing.IsFirstTimePredicted)
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
        var prototypes = _prototypeManager.EnumeratePrototypes<ConsentPrototype>();

        foreach (var consentProto in prototypes)
            component.Preferences.TryAdd(new ProtoId<ConsentPrototype>(consentProto.ID), ConsentLevel.Neutral);
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
