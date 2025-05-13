using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Content.Shared.Consent.Prototypes;

namespace Content.Shared.Consent.UI;

[Serializable, NetSerializable]
public sealed class ViewConsentBuiState : BoundUserInterfaceState
{
    public string TargetName { get; }
    public Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> ConsentPreferences { get; }

    public ViewConsentBuiState(string targetName, Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> consentPreferences)
    {
        TargetName = targetName;
        ConsentPreferences = consentPreferences;
    }
}
