<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# ADR 0001: A native port, not a network client

- Status: Accepted
- Date: 2026-11-18

## Context

jspassgen's suite decision
([jspassgen ADR 0001](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md),
sections 4 and 5) gives every SDK the same contract. Its .NET sibling in
another suite, crypto-service-sdk-dotnet, is a thin REST client; that
shape does not fit here, because a password generated on a server and
sent over the network breaks jspassgen's promise that a secret is made
on the user's machine and never leaves it.

## Decision

- This SDK is a native .NET implementation of `packages/core`. It makes
  no network call and has no telemetry.
- Every random draw goes through `IRandomSource`, the .NET form of
  `RandomGeneratorPort`. The production implementation,
  `CryptoRandomSource`, uses `RandomNumberGenerator`; `NextInt` is an
  unbiased draw (`RandomNumberGenerator.GetInt32`), never a modulo of
  raw bytes.
- Generators are ported only against jspassgen's conformance kit: each
  vector's draw tape is replayed through a test `IRandomSource`, and the
  port must make the same draws in the same order and produce
  byte-identical output and the same error codes. Until the kit exists,
  no generator is ported.
- Statistical properties of `CryptoRandomSource` (uniformity, no modulo
  bias) are tested here, since tapes cannot cover them.

## Consequences

- A change to how the core consumes randomness is a breaking change for
  this port and lands in the same suite version.
- The repository ships its scaffold, gate and random source before any
  generator; the README and roadmap say so.
