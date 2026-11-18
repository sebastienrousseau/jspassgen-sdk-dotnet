<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

<p align="center">
  <img src="https://raw.githubusercontent.com/sebastienrousseau/jspassgen/main/.github/assets/logo.svg" alt="jspassgen logo" width="128" />
</p>

<h1 align="center">jspassgen for .NET</h1>

<p align="center">
  A planned native .NET port of the jspassgen password and passphrase generator, drawing every random value from the platform CSPRNG on your own machine.
</p>

<p align="center">
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/actions"><img src="https://img.shields.io/github/actions/workflow/status/sebastienrousseau/jspassgen-sdk-dotnet/ci.yml?style=for-the-badge&logo=github" alt="Build" height="28" /></a>
  <a href="docs/ROADMAP.md"><img src="https://img.shields.io/badge/NuGet-not%20published-lightgrey?style=for-the-badge&color=fc8d62&logo=nuget" alt="Registry: not published yet" height="28" /></a>
  <a href="docs/index.md"><img src="https://img.shields.io/badge/docs-in%20repository-blue?style=for-the-badge&labelColor=555555&logo=dotnet" alt="Docs" height="28" /></a>
  <a href="https://scorecard.dev/viewer/?uri=github.com/sebastienrousseau/jspassgen-sdk-dotnet"><img src="https://img.shields.io/ossf-scorecard/github.com/sebastienrousseau/jspassgen-sdk-dotnet?style=for-the-badge&label=OpenSSF%20Scorecard&logo=openssf" alt="OpenSSF Scorecard" height="28" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-Apache--2.0%20OR%20MIT-blue.svg?style=for-the-badge" alt="License: Apache-2.0 OR MIT" height="28" /></a>
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/blob/main/docs/POLICIES.md"><img src="https://img.shields.io/badge/.NET-10-93450a.svg?style=for-the-badge&logo=dotnet" alt=".NET 10 or later" height="28" /></a>
</p>

<p align="center">
  No demo yet: there is no generator to show. The demo (<code>.github/demo.gif</code>, rendered from a committed VHS tape) is added with the first ported generator, so it shows real output.
</p>

---

## Contents

**Getting started**

- [Install](#install): a project reference from a clone; NuGet `Jspassgen` is not published yet
- [Requirements](#requirements): toolchain floor, platforms
- [Quick Start](#quick-start): draw bytes and an unbiased integer from the CSPRNG

**The jspassgen ecosystem**

- [The jspassgen ecosystem](#the-jspassgen-ecosystem): the core, three JavaScript tools, five native SDKs and the site

**Library reference**

- [Capabilities at a glance](#capabilities-at-a-glance): the current surface by theme
- [Ecosystem comparison](#ecosystem-comparison): not available yet
- [Benchmarks](#benchmarks): not available yet
- [Features](#features): the random source
- [Configuration](#configuration): none yet
- [Examples](#examples): not available yet

**Operational**

- [When not to use jspassgen](#when-not-to-use-jspassgen): limitations
- [Development](#development): make targets, CI
- [Security](#security): guarantees and their limits
- [Documentation](#documentation): all reference docs
- [Stability guarantees](#stability-guarantees): versioning, output stability, minimum toolchain discipline
- [License](#license)

---

## Install

### As a .NET library

```xml
<!-- Not on NuGet yet: planned for the first suite release, 0.0.22 -->
<PackageReference Include="Jspassgen" Version="0.0.22" />
```

The package `Jspassgen` is **not published yet**. It ships with the
first jspassgen suite release, `0.0.22`, once the password generators
are ported and pass the conformance kit. Until then, clone this
repository and reference the project:

```sh
git clone https://github.com/sebastienrousseau/jspassgen-sdk-dotnet.git
dotnet add <your-project>.csproj reference jspassgen-sdk-dotnet/src/Jspassgen/Jspassgen.csproj
```

---

## Requirements

- .NET 10 (`net10.0`); building needs the .NET SDK 10.0.404 or a later
  10.0 feature band (`global.json`). `mise install` gives the pinned SDK.
- Built and tested in CI on Linux, Windows and macOS (GitHub-hosted
  `*-latest` runners).
- No network access and no third-party runtime dependency.

The floor and when it moves are in [`docs/POLICIES.md`](docs/POLICIES.md).

---

## Quick Start

```csharp
using Jspassgen;

IRandomSource random = new CryptoRandomSource();

byte[] salt = random.GetBytes(16);  // 16 bytes from RandomNumberGenerator
int index = random.NextInt(94);     // uniform in [0, 94), no modulo bias

Console.WriteLine($"{Convert.ToHexString(salt)} {index}");
```

This is everything the library does today. `IRandomSource` is the .NET
form of jspassgen's `RandomGeneratorPort`: every generator ported here
will draw through it, so a test can replay a fixed sequence of draws and
production code uses the CSPRNG.

---

## The jspassgen ecosystem

jspassgen is planned as a suite of ten repositories released at one
version. The list below mirrors
[`docs/ecosystem.json`](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/ecosystem.json)
in jspassgen, the source of truth; registry names are proposed until a
member first publishes. Only `jspassgen` itself is released today.

| Component | Purpose | Use case |
| :--- | :--- | :--- |
| [jspassgen](https://github.com/sebastienrousseau/jspassgen) | Zero-dependency core, Node.js library, web UI and workers; source of the conformance kit | Generating passwords in JavaScript (released, npm `@sebastienrousseau/jspassgen`) |
| [jspassgen-cli](https://github.com/sebastienrousseau/jspassgen-cli) | Command-line generator | Terminal use (planned) |
| [jspassgen-mcp](https://github.com/sebastienrousseau/jspassgen-mcp) | Local stdio MCP server | AI agents generating secrets locally (planned) |
| [jspassgen-lsp](https://github.com/sebastienrousseau/jspassgen-lsp) | Language server for weak literal secrets | Editors (planned) |
| [jspassgen-sdk-python](https://github.com/sebastienrousseau/jspassgen-sdk-python) | Native Python port | Python (planned) |
| [jspassgen-sdk-rust](https://github.com/sebastienrousseau/jspassgen-sdk-rust) | Native Rust port | Rust (planned) |
| [jspassgen-sdk-java](https://github.com/sebastienrousseau/jspassgen-sdk-java) | Native Java port | JVM (planned) |
| **jspassgen-sdk-dotnet** | Native .NET port (this repository) | .NET (planned) |
| [jspassgen-sdk-cpp](https://github.com/sebastienrousseau/jspassgen-sdk-cpp) | Native C++17 port | C and C++ (planned) |
| [jspassgen.github.io](https://github.com/sebastienrousseau/jspassgen.github.io) | jspassgen.com | Product site and SDK pages (planned) |

---

## Capabilities at a glance

| Area | Capability | Status |
| :--- | :--- | :--- |
| Randomness | `IRandomSource` port, `CryptoRandomSource` over `RandomNumberGenerator`, unbiased `NextInt` | Available (unreleased) |
| Password types | `strong`, `base64`, `memorable`, `quantum-resistant`, `diceware`, `honeyword`, `pronounceable`, `custom`, as in the core's `PASSWORD_TYPES` | Not available yet |
| Validation and entropy | The core's configuration validation, error codes and entropy calculation | Not available yet |
| Conformance | jspassgen's conformance kit (draw tapes, errors, EFF wordlist) vendored and run in CI | Not available yet: the kit does not exist |

---

## Ecosystem comparison

Not available yet. A comparison with other .NET password generators is
written once there is a generator to compare, from measured evidence.

---

## Benchmarks

Not available yet. There is no generator to measure; this repository
publishes no performance numbers.

---

## Features

- `IRandomSource`: `GetBytes(count)` returns `count` random bytes;
  `NextInt(max)` returns a uniform integer in `[0, max)`. Both throw
  `ArgumentOutOfRangeException` below 1.
- `CryptoRandomSource`: the implementation on
  `System.Security.Cryptography.RandomNumberGenerator`.
  `NextInt` uses `RandomNumberGenerator.GetInt32`, which samples by
  rejection, never by a modulo of raw bytes. Stateless and thread-safe.

---

## Configuration

None yet. The random source takes no options; generator options arrive
with the ported generators.

---

## Examples

Not available yet. `examples/` is added with the first ported
generator, and each example is run in CI.

---

## When not to use jspassgen

- **Today, for anything but experiments.** This version generates no
  passwords, is not published and may change in any way before
  `0.0.22`.
- When you need secrets generated by a server or shared service: every
  jspassgen member generates locally and makes no network call.
- When you need a password manager, storage or synchronisation: jspassgen
  only generates.

---

## Development

```bash
mise install    # the pinned .NET SDK and Node
make check      # format, build, 100 % coverage, pack, audit, versions, Halstead
make docs-lint  # markdownlint, codespell, REUSE
```

[DEVELOPMENT.md](DEVELOPMENT.md) maps every target to its CI job.
There is no fuzzing yet: the library parses no input.

---

## Security

- Every random value comes from the platform CSPRNG
  (`RandomNumberGenerator`); `System.Random` is never used.
- Integers are drawn without modulo bias, and a unit test rejects a
  modulo-biased implementation.
- No network call, no telemetry, no logging, no third-party runtime
  dependency.
- CodeQL and OpenSSF Scorecard workflows are configured; every GitHub
  Action is pinned by commit SHA.

Report vulnerabilities according to [`SECURITY.md`](SECURITY.md).

---

## Documentation

- User manual: [`docs/`](docs/index.md) in this repository; a rendered
  manual is not deployed yet.
- API reference: the XML documentation comments in
  [`src/Jspassgen/`](src/Jspassgen/); a rendered reference is not
  published yet.
- Developer docs: [DEVELOPMENT.md](DEVELOPMENT.md),
  [CONTRIBUTING.md](CONTRIBUTING.md), [AGENTS.md](AGENTS.md).
- Ecosystem map: [ADR 0001 in jspassgen](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md)
  and [`docs/ecosystem.json`](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/ecosystem.json).

---

## Stability guarantees

- **Versioning.** `0.0.x`, moving by `0.0.1` in lockstep with the
  jspassgen suite. Before `1.0` any release may change the public API;
  the CHANGELOG says how.
- **Output stability.** Once generators are ported, the same random
  draws produce the same password as jspassgen's core, as the
  conformance kit proves. A change to what a generator produces, or to
  the order in which it draws, is breaking even when no signature moves.
- **Minimum toolchain.** .NET 10; the floor moves only as
  [`docs/POLICIES.md`](docs/POLICIES.md) states.

---

## License

Dual-licensed under either of the [Apache License, Version 2.0](LICENSE-APACHE)
or the [MIT license](LICENSE-MIT), at your option (SPDX expression
`Apache-2.0 OR MIT`).

Unless you explicitly state otherwise, any contribution you submit for
inclusion in this work, as defined in the Apache-2.0 license, is dual
licensed as above, without any additional terms or conditions.
