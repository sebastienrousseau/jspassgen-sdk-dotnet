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
| `tests/Jspassgen.Tests/CryptoRandomSourceProperties.cs` | FsCheck properties: exact length, [0, max) range and argument errors over generated inputs |

The two statistical tests are set so a correct source fails them about
once in a million runs (uniformity) or less (bias, a six-sigma window).

## Not here yet

These parts of the suite standard arrive with the ported generators,
because today they would have nothing to run: `conformance/` and
`ci/core.ref` (the kit ships in jspassgen 0.0.21 and is vendored with
the first generator), `examples/`, `benchmarks/`,
a `fuzz/` harness (the library parses no input; FsCheck property tests
run with the unit tests instead), the rendered manual and its
deploy workflow, and `.github/demo.tape` and `demo.gif`.

## Release model

The version is `0.0.x` and moves by `0.0.1` on `feat/v<next-version>`,
the only pull request into `main`. This repository joins the suite at
`0.0.22`. `.github/workflows/release.yml` releases a pushed
`v<version>` tag; nothing has been published yet.

1. On `feat/v<version>`: move `<Version>` in
   `src/Jspassgen/Jspassgen.csproj` and its restatements
   (`make verify-versions` lists them), and date the `CHANGELOG.md`
   heading. Merge the release pull request into `main` once CI is green.
2. Tag `main` with a signed annotated tag and check it before pushing:

   ```bash
   git tag -s v<version> -m "jspassgen-sdk-dotnet v<version>"
   scripts/verify-release-tag v<version> "$(git rev-parse HEAD)"
   git push origin v<version>
   ```

3. The `check` job runs `scripts/verify-release-tag`, which refuses a
   tag that is lightweight, unsigned, signed by a key not in `KEYS.asc`
   (OpenSSH allowed signers), not on the checked-out commit, named for
   another version than the csproj, or whose message is not
   `jspassgen-sdk-dotnet v<version>`, and a CHANGELOG heading that is
   still `Unreleased`. It then runs `make check`, which packs
   `Jspassgen.<version>.nupkg` and its `.snupkg`, writes a CycloneDX
   SBOM (`Jspassgen.<version>.cdx.json`, the `cyclonedx` tool pinned in
   `.config/dotnet-tools.json`) and `SHA256SUMS` over the three.
4. The `attest` job records SLSA build provenance for both packages
   (`gh attestation verify <file> --repo
   sebastienrousseau/jspassgen-sdk-dotnet`).
5. The `publish-nuget` job waits for a maintainer to approve the `nuget`
   environment, checks the files against `SHA256SUMS`, and pushes the
   package and its symbols to nuget.org through NuGet trusted
   publishing: `NuGet/login` exchanges the job's OIDC token for a
   short-lived API key, so no NuGet key is stored. A version
   already on nuget.org is skipped, so a rerun is safe; the job then
   polls until the version is visible.
6. The `github-release` job drafts the GitHub release
   `jspassgen-sdk-dotnet <version>` with the packages, the SBOM and
   `SHA256SUMS`, and notes with What's Changed, the checksums and the
   Full Changelog line. The maintainer writes the Highlights and
   publishes the draft.

Set up once, before the first tag:

- the `nuget` environment, with the maintainer as required reviewer and
  deployments only from `main` and `v*` tags;
- the `NUGET_USER` repository variable: the nuget.org account that owns
  `Jspassgen`;
- on nuget.org, a trusted publishing policy for that account naming the
  owner `sebastienrousseau`, the repository `jspassgen-sdk-dotnet`, the
  workflow file `release.yml` and the environment `nuget`.
