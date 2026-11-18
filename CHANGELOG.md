<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
The version follows the jspassgen suite and moves by `0.0.1`.

## [0.0.22] - Unreleased

The planned first release, with the first jspassgen suite release. This
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

### Not yet

- The password generators, validation and entropy calculation, the
  vendored conformance kit (`conformance/`, `ci/core.ref`), examples,
  benchmarks, fuzzing, the rendered manual, the demo and the release
  workflow. They arrive before `0.0.22` is tagged; the version is not
  released without them.
