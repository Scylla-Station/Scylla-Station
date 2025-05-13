using Content.Shared.Consent.UI;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Consent.UI;

[UsedImplicitly]
public sealed class ViewConsentBoundUserInterface : BoundUserInterface
{
    private ViewConsentWindow? _window;

    public ViewConsentBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<ViewConsentWindow>();
        _window.OnClose += Close;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window == null || state is not ViewConsentBuiState cast)
            return;

        _window.Populate(cast.TargetName, cast.ConsentPreferences);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _window?.Dispose();
        }
    }
}
