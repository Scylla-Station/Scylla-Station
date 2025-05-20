// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
// SPDX-FileCopyrightText: 2025 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Goobstation.Common.DoAfter;

/// <summary>
/// Event raised on the doafter user after a doafter ends.
/// </summary>
[ByRefEvent]
public readonly record struct DoAfterEndedEvent(EntityUid? Target, bool Cancelled);
