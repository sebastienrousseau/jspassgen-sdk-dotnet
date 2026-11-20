<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
The version follows the jspassgen suite and moves by `0.0.1`.

## [0.0.23] - 2026-11-20

The suite's second lockstep release. The library's API and behaviour
are unchanged from 0.0.22.

### Added

- `.github/CODEOWNERS`: every file is owned by the maintainer, whose
  review the `main` ruleset requires.
- FsCheck property-based tests of the random source
  (`CryptoRandomSourceProperties`): `GetBytes` returns exactly `count`
  bytes, `NextInt(max)` stays in [0, max) for bounds from 1 to
  `int.MaxValue`, and both reject every argument below 1. They run
  with the unit tests; `FsCheck.Xunit` 3.6.0 is a test-only package,
  pinned exactly.

### Changed

- The version is `0.0.23`, in lockstep with the jspassgen suite.

## [0.0.22] - 2026-11-18

The first release, with the first jspassgen suite release. This
repository joins the suite at the suite's version, never at `0.0.1`
(jspassgen ADR 0001).

### Added

- `IRandomSource`, the .NET form of jspassgen's `RandomGeneratorPort`:
  `GetBytes(count)` and `NextInt(max)`, both throwing
  `ArgumentOutOfRangeException` below 1.
- `CryptoRandomSource` on `RandomNumberGenerator`, with an unbiased
  `NextInt` (`RandomNumberGenerator.GetInt32`), and its unit tests at
  100 % line and branch coverage: range, argument errors, a
  chi-squared uniformity test and a modulo-bias test.
- The repository standard: the `make check` gate, CI on Linux, Windows
  and macOS, CodeQL, OpenSSF Scorecard, DCO and docs-lint workflows,
  Dependabot, and the community and policy documents.
- The release workflow (`.github/workflows/release.yml`) for a pushed
  `v<version>` tag: `scripts/verify-release-tag` refuses a tag that is
  not annotated, not signed by a key in `KEYS.asc`, not on the released
  commit, or not named and worded for the csproj version; then
  `make check`, the package and its symbols package, a CycloneDX SBOM,
  `SHA256SUMS`, SLSA build provenance, NuGet trusted publishing behind
  the `nuget` environment, and a draft GitHub release.
- A symbols package (`Jspassgen.<version>.snupkg`) beside the `.nupkg`.

### Not yet

- The password generators, validation and entropy calculation, the
  vendored conformance kit (`conformance/`, `ci/core.ref`), examples,
  benchmarks, fuzzing, the rendered manual and the demo. They come in a
  later version.
