// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Consent.Prototypes;

/// <summary>
/// Defines a type of consentable activity or preference.
/// </summary>
[Prototype("consent")]
public sealed partial class ConsentPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string Name { get; private set; } = default!;

    [DataField]
    public string Description { get; private set; } = string.Empty;

    [DataField]
    public string Category { get; private set; } = "Uncategorized";

    [DataField]
    public SpriteSpecifier Icon { get; private set; } = SpriteSpecifier.Invalid;
}
