<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# jspassgen for .NET

A planned native .NET port of the
[jspassgen](https://github.com/sebastienrousseau/jspassgen) password and
passphrase generator. Every random value comes from the platform CSPRNG
on the caller's machine; nothing is sent over the network. Today the
library holds only the random source, `IRandomSource` and
`CryptoRandomSource`; the generators are ported once jspassgen's
conformance kit exists.

## In this manual

- [Architecture](ARCHITECTURE.md) and the
  [decision records](adr/index.md).
- [Policies](POLICIES.md): minimum toolchain, versioning, output
  stability, deprecation.
- [Roadmap](ROADMAP.md): what exists, and what comes before the first
  release.
- For contributors: [Development](../DEVELOPMENT.md),
  [Contributing](../CONTRIBUTING.md), [Governance](../GOVERNANCE.md),
  [Security](../SECURITY.md), [Support](../SUPPORT.md),
  [Changelog](../CHANGELOG.md).

A rendered manual is not deployed yet.
