// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Scylla.Consent.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Scylla.Consent.Components;

/// <summary>
/// Stores a player's consent preferences.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ConsentPreferencesComponent : Component
{
    /// <summary>
    /// Dictionary mapping ConsentPrototype IDs to the player's chosen ConsentLevel.
    /// If a prototype ID is not present, it implies a default behavior of Ask.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<ProtoId<ConsentPrototype>, ConsentLevel> Preferences = new();
}
