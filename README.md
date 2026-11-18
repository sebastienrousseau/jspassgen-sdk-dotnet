<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

<p align="center">
  <img src="https://raw.githubusercontent.com/sebastienrousseau/jspassgen/main/.github/assets/logo.svg" alt="jspassgen logo" width="128" />
</p>

<h1 align="center">jspassgen-sdk-dotnet</h1>

<p align="center">
  The native .NET port of the jspassgen password and passphrase generator; this version holds its random source, which draws every value from the platform CSPRNG on your own machine.
</p>

<p align="center">
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/actions"><img src="https://img.shields.io/github/actions/workflow/status/sebastienrousseau/jspassgen-sdk-dotnet/ci.yml?style=for-the-badge&logo=github" alt="Build" height="28" /></a>
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/blob/main/docs/ROADMAP.md"><img src="https://img.shields.io/badge/NuGet-not%20published%20yet-lightgrey?style=for-the-badge&color=fc8d62&logo=nuget" alt="NuGet: not published yet" height="28" /></a>
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/blob/main/docs/index.md"><img src="https://img.shields.io/badge/docs-in%20repository-blue?style=for-the-badge&labelColor=555555&logo=dotnet" alt="Docs" height="28" /></a>
  <a href="https://scorecard.dev/viewer/?uri=github.com/sebastienrousseau/jspassgen-sdk-dotnet"><img src="https://img.shields.io/ossf-scorecard/github.com/sebastienrousseau/jspassgen-sdk-dotnet?style=for-the-badge&label=OpenSSF%20Scorecard&logo=openssf" alt="OpenSSF Scorecard" height="28" /></a>
  <a href="LICENSE-APACHE"><img src="https://img.shields.io/badge/license-Apache--2.0%20OR%20MIT-blue.svg?style=for-the-badge" alt="License: Apache-2.0 OR MIT" height="28" /></a>
  <a href="https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/blob/main/docs/POLICIES.md"><img src="https://img.shields.io/badge/.NET-10-93450a.svg?style=for-the-badge&logo=dotnet" alt=".NET 10" height="28" /></a>
</p>

---

## Contents

**Getting started**

- [Install](#install): a project reference from a checkout; NuGet `Jspassgen` is not published yet
- [Requirements](#requirements): toolchain floor, platforms
- [Quick Start](#quick-start): draw bytes and an unbiased integer from the CSPRNG

**The jspassgen ecosystem**

- [The jspassgen ecosystem](#the-jspassgen-ecosystem): the core, its tools, the native SDKs and the site

**Library reference**

- [Capabilities at a glance](#capabilities-at-a-glance): the current surface by theme
- [Ecosystem comparison](#ecosystem-comparison): not available yet
- [Benchmarks](#benchmarks): not available yet
- [Features](#features): the random source
- [Configuration](#configuration): none yet
- [Examples](#examples): not available yet

**Operational**

- [When not to use jspassgen-sdk-dotnet](#when-not-to-use-jspassgen-sdk-dotnet): limitations
- [Development](#development): make targets, CI
- [Security](#security): guarantees and their limits
- [Documentation](#documentation): all reference docs
- [Stability guarantees](#stability-guarantees): versioning, output stability, minimum toolchain discipline
- [License](#license)

---

## Development version

This branch prepares unreleased v0.0.22. It is not on NuGet yet;
reference the project from a checkout until it ships.

---

## Install

### As a .NET library

```xml
<!-- Not on NuGet yet: planned for the first suite release, 0.0.22 -->
<PackageReference Include="Jspassgen" Version="0.0.22" />
```

The package `Jspassgen` is not published yet. It is planned to ship
with the first jspassgen suite release, `0.0.22`, once the password
generators are ported and pass the conformance kit. Until then, clone
this repository and reference the project:

```bash
git clone https://github.com/sebastienrousseau/jspassgen-sdk-dotnet.git
dotnet add <your-project>.csproj reference jspassgen-sdk-dotnet/src/Jspassgen/Jspassgen.csproj
```

---

## Requirements

- .NET 10 (`net10.0`). Building needs the .NET SDK 10.0.404 or a later
  10.0 feature band (`global.json`); `mise install` gives the pinned SDK.
- CI builds and tests on Linux, Windows and macOS (GitHub-hosted
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
will draw through it, so a test can replay a fixed sequence of draws
while production code uses the CSPRNG.

---

## The jspassgen ecosystem

jspassgen is one core library and the tools, SDKs and site around it.
Every member is listed in
[`docs/ecosystem.json`](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/ecosystem.json)
and, from the suite's first lockstep release, every member ships the same
version ([ADR 0001](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md)).
The JavaScript tools depend on the core and add no generation logic of
their own; each SDK is a native port that is checked against the core's
conformance kit as its generators land.

| Component | Purpose | Use case |
| :--- | :--- | :--- |
| [`jspassgen`](https://github.com/sebastienrousseau/jspassgen) | Password and passphrase core, Node.js library, CLI, web UI and workers; source of the conformance kit | Programs that generate secrets |
| [`jspassgen-cli`](https://github.com/sebastienrousseau/jspassgen-cli) | Command-line generator built on the core | Passwords in a terminal or a script |
| [`jspassgen-mcp`](https://github.com/sebastienrousseau/jspassgen-mcp) | Local stdio MCP server | AI agents that need a generated secret |
| [`jspassgen-lsp`](https://github.com/sebastienrousseau/jspassgen-lsp) | Language server for weak literal secrets | Editors checking configuration and `.env` files |
| [`jspassgen-sdk-python`](https://github.com/sebastienrousseau/jspassgen-sdk-python) | Native port on the `secrets` module | Python programs |
| [`jspassgen-sdk-rust`](https://github.com/sebastienrousseau/jspassgen-sdk-rust) | Native port on the OS CSPRNG | Rust programs |
| [`jspassgen-sdk-java`](https://github.com/sebastienrousseau/jspassgen-sdk-java) | Native port on `SecureRandom` | JVM programs |
| **`jspassgen-sdk-dotnet`** | Native port on `RandomNumberGenerator` | .NET programs |
| [`jspassgen-sdk-cpp`](https://github.com/sebastienrousseau/jspassgen-sdk-cpp) | Native C++17 port on the OS CSPRNG | C++ programs |
| [`jspassgen.github.io`](https://github.com/sebastienrousseau/jspassgen.github.io) | Source of [jspassgen.com](https://jspassgen.com) | Evaluating the suite |

Only `@sebastienrousseau/jspassgen` is on a package registry so far
(0.0.21 on npm). The table gives each member's purpose; the other members
are early, each README's Capabilities at a glance says what works today,
and they first publish with the suite's first lockstep release.

---

## Capabilities at a glance

| Area | Capability | Status |
| :--- | :--- | :--- |
| Randomness | `IRandomSource` port, `CryptoRandomSource` over `RandomNumberGenerator`, unbiased `NextInt` | Available (unreleased) |
| Password types | `strong`, `base64`, `memorable`, `quantum-resistant`, `diceware`, `honeyword`, `pronounceable`, `custom`, as in the core's `PASSWORD_TYPES` | Not available yet |
| Validation and entropy | The core's configuration validation, error codes and entropy calculation | Not available yet |
| Conformance | jspassgen's conformance kit (draw vectors, errors, validation, entropy, EFF wordlist) vendored and run in CI | Not available yet: the kit is in jspassgen's `conformance/` but not vendored here |

---

## Ecosystem comparison

Not available yet. A comparison with other .NET password generators is
written once there is a generator to compare, from measured evidence.

---

## Benchmarks

Not available yet. There is no generator to measure, and this
repository publishes no performance numbers.

---

## Features

- `IRandomSource.GetBytes(count)` returns `count` random bytes.
- `IRandomSource.NextInt(max)` returns a uniform integer in `[0, max)`.
- Both throw `ArgumentOutOfRangeException` when the argument is below 1.
- `CryptoRandomSource` implements `IRandomSource` on
  `System.Security.Cryptography.RandomNumberGenerator`.
- `CryptoRandomSource.NextInt` uses `RandomNumberGenerator.GetInt32`,
  which samples by rejection, never by a modulo of raw bytes.
- `CryptoRandomSource` holds no state and is safe to share across
  threads.

---

## Configuration

None yet. The random source takes no options; generator options arrive
with the ported generators.

---

## Examples

Not available yet. `examples/` is added with the first ported
generator, and each example is run in CI.

---

## When not to use jspassgen-sdk-dotnet

- Do not use this version for anything but experiments: it generates no
  passwords, is not published and may change in any way before `0.0.22`.
- Use another tool when you need secrets generated by a server or a
  shared service: this library generates locally and makes no network
  call.
- Use a password manager when you need storage or synchronisation:
  jspassgen only generates.

---

## Development

```bash
mise install    # the pinned .NET SDK and Node
make check      # format, build, 100 % coverage, pack, audit, versions, Halstead
make docs-lint  # markdownlint, codespell, REUSE
```

CI runs the format check, the build and unit tests on Linux, Windows and
macOS with a 100 % line and branch coverage gate, the package build, the
vulnerable-package audit, the Halstead check and the version check.
[`DEVELOPMENT.md`](DEVELOPMENT.md) maps every target to its CI job.
There is no fuzzing yet: the library parses no input.

---

## Security

- Every random value comes from the platform CSPRNG
  (`RandomNumberGenerator`); `System.Random` is never used.
- Integers are drawn without modulo bias, and a unit test rejects a
  modulo-biased implementation.
- The library makes no network call, sends no telemetry, writes no logs
  and has no third-party runtime dependency.
- CodeQL and OpenSSF Scorecard workflows are configured, and every
  GitHub Action is pinned by commit SHA.

Report vulnerabilities according to [`SECURITY.md`](SECURITY.md).

---

## Documentation

- [User manual (in the repository; not rendered yet)](docs/index.md)
- [API reference (XML documentation comments in the source)](src/Jspassgen/)
- [Developer documentation](DEVELOPMENT.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Decision records](docs/adr/index.md)
- [Engineering policies](docs/POLICIES.md)
- [Roadmap](docs/ROADMAP.md)
- [Contributing](CONTRIBUTING.md)
- [Agent guidelines](AGENTS.md)
- [Changelog](CHANGELOG.md)
- [Ecosystem map](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/ecosystem.json)
- [Ecosystem lockstep decision (jspassgen ADR 0001)](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md)

---

## Stability guarantees

- The version is `0.0.x` and moves by `0.0.1` in lockstep with the
  jspassgen suite.
- Before `1.0` any release may change the public API, and the
  CHANGELOG says how.
- Once generators are ported, the same random draws produce the same
  password as jspassgen's core, as the conformance kit proves.
- A change to what a generator produces, or to the order in which it
  draws, is breaking even when no signature moves.
- The minimum toolchain is .NET 10, and the floor moves only as
  [`docs/POLICIES.md`](docs/POLICIES.md) states.

---

## License

Licensed under either [Apache License 2.0](LICENSE-APACHE) or
[MIT](LICENSE-MIT), at your option.
