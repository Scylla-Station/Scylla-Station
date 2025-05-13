using Content.Shared.Consent;
using Content.Shared.Consent.Components;
using Content.Shared.Consent.Prototypes;
using Content.Shared.Consent.UI;
using Robust.Shared.Prototypes;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Consent;

public sealed class ViewConsentSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<RequestViewConsentEvent>(OnRequestViewConsent);
    }

    private void OnRequestViewConsent(RequestViewConsentEvent msg, EntitySessionEventArgs args)
    {
        var userUid = GetEntity(msg.User);
        var targetUid = GetEntity(msg.Target);

        OpenViewConsentUI(userUid, targetUid);
    }

    public void OpenViewConsentUI(EntityUid user, EntityUid target)
    {
        if (!HasComp<ActorComponent>(user))
            return;

        _userInterfaceSystem.TryOpenUi(target, ViewConsentUiKey.Key, user);
        UpdateUI(target, user);
    }

    private void UpdateUI(EntityUid target, EntityUid user)
    {
        string targetName = "Unknown";
        if (TryComp<MetaDataComponent>(target, out var metaData))
            targetName = metaData.EntityName;

        Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> consentsToShow;

        if (TryComp<ConsentPreferencesComponent>(target, out var consentPreferencesComponent) && consentPreferencesComponent.Preferences != null)
            consentsToShow = consentPreferencesComponent.Preferences;
        else
            consentsToShow = new Dictionary<ProtoId<ConsentPrototype>, ConsentLevel>();

        var state = new ViewConsentBuiState(targetName, consentsToShow);

        _userInterfaceSystem.SetUiState(target, ViewConsentUiKey.Key, state);
    }
}

