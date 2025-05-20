// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Client.Scylla.Consent.UI;
using Content.Shared.Scylla.Consent;

namespace Content.Client.Scylla.Consent;

public sealed class ClientConsentSystem : EntitySystem
{
    private ViewConsentWindow? _window;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<OpenConsentWindowClientEvent>(OnOpenConsentWindow);
    }

    private void OnOpenConsentWindow(OpenConsentWindowClientEvent ev)
    {
        if (_window != null)
            _window.Close();

        _window = new ViewConsentWindow();
        _window.Populate(ev.TargetName, ev.ConsentPreferences);
        _window.OnClose += () =>
        {
            _window = null;
        };
        _window.OpenCentered();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _window?.Dispose();
        _window = null;
    }
}
