<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Development

The single entry point for working on `jspassgen-sdk-dotnet`: the
toolchain, every CI gate reproduced locally, the test layout and the
release model. [CONTRIBUTING.md](CONTRIBUTING.md) covers how to send a
change; [AGENTS.md](AGENTS.md) lists the rules that must not drift.

## Toolchain

| Tool | Version | Used for |
| :--- | :--- | :--- |
| .NET SDK | 10.0.404 (`global.json`, `mise.toml`) | Build, tests, coverage, pack |
| Node.js | 24 (`mise.toml`; CI uses 22) | markdownlint |
| Python | 3.12 | codespell, REUSE, the Halstead check |

`mise install` gives the pinned .NET SDK and Node. Every NuGet package
is pinned exactly in `Directory.Packages.props` and locked with content
hashes in each project's `packages.lock.json`; CI restores in locked
mode. After a package change, regenerate the lock files with
`dotnet restore --force-evaluate` (never edit them by hand) and say so
in the commit body.

## Every CI gate, locally

| Gate | Local command | CI |
| :--- | :--- | :--- |
| Format | `make format` (`dotnet format --verify-no-changes`) | `ci.yml` Format |
| Build: warnings as errors, `latest-recommended` analysers, complexity | `make build` | `ci.yml` Build and unit tests (Linux, Windows, macOS) |
| Unit tests with 100 % line and branch coverage | `make coverage` | `ci.yml` Build and unit tests |
| Package build with package validation | `make pack` | `ci.yml` Build and unit tests (Linux) |
| Vulnerable packages | `make audit` | `ci.yml` Vulnerable packages |
| Version references agree | `make verify-versions` | `ci.yml` Release versions agree |
| Halstead difficulty ≤ 30 | `make halstead` | `ci.yml` Complexity |
| markdownlint, codespell, REUSE | `make docs-lint` | `docs-lint.yml` |
| Signed-off-by on every commit | `git log --format=%B` | `dco.yml` |
| CodeQL, OpenSSF Scorecard | n/a (GitHub only) | `codeql.yml`, `scorecard.yml` |

`make check` runs format, build, coverage, pack, the audit, the version
check and the Halstead check.

### Complexity ceilings

| Metric | Ceiling | Enforced by |
| :--- | :--- | :--- |
| Cyclomatic complexity | 10 | CA1502 (`CodeMetricsConfig.txt`) |
| Cognitive complexity | 15 | SonarAnalyzer S3776 (`SonarLint.xml`) |
| Lines per method | 60 | SonarAnalyzer S138 |
| Lines per file | 500 | SonarAnalyzer S104 |
| Halstead difficulty | 30 | `scripts/halstead.py` |

They hold in the library and the tests. No method is over a ceiling,
so there is no baseline.

### Coverage

`make coverage` runs the unit tests under coverlet and fails below
100 % line or branch coverage of `Jspassgen`. The report is
`artifacts/coverage/coverage.cobertura.xml`. There is no exclusion.

## Test layout

| Path | What it covers |
| :--- | :--- |
| `tests/Jspassgen.Tests/CryptoRandomSourceTests.cs` | Lengths and ranges, argument errors, chi-squared uniformity, modulo bias |

The two statistical tests are set so a correct source fails them about
once in a million runs (uniformity) or less (bias, a six-sigma window).

## Not here yet

These parts of the suite standard arrive with the ported generators,
because today they would have nothing to run: `conformance/` and
`ci/core.ref` (the kit does not exist yet), `examples/`, `benchmarks/`,
`fuzz/` (the library parses no input), the rendered manual and its
deploy workflow, `.github/demo.tape` and `demo.gif`, and the release
workflow.

## Release model

The version is `0.0.x` and moves by `0.0.1` on `feat/v<next-version>`,
the only pull request into `main`. This repository joins the suite at
`0.0.22`. The release workflow (signed-tag verification, pack, SBOM,
`SHA256SUMS`, provenance, NuGet trusted publishing) is added before the
first tag; until then nothing is published.
