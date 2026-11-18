<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# AGENTS.md

`jspassgen-sdk-dotnet` is the native .NET port of the
[jspassgen](https://github.com/sebastienrousseau/jspassgen) password and
passphrase core (`packages/core`), one member of the jspassgen suite
([ADR 0001 in jspassgen](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md)).
It generates secrets on the caller's machine from the platform CSPRNG;
it is not a network client. Today it holds only the random source
(`IRandomSource`, `CryptoRandomSource`): the generators are ported once
jspassgen's conformance kit exists. These are the rules an automated
change is most likely to break; humans follow them too. Read
[CONTRIBUTING.md](CONTRIBUTING.md) first.

## Versioning

- The version is `0.0.x` and moves strictly by `0.0.1`, in lockstep
  with the jspassgen suite. Never propose a `0.1.0` or `1.0` jump, and
  never bump it as a side effect of a change. `0.0.22` is the first
  suite version and this repository's first release.
- `<Version>` in `src/Jspassgen/Jspassgen.csproj` is the single source
  of truth; `CITATION.cff`, the `CHANGELOG.md` heading, the
  `SECURITY.md` and `SUPPORT.md` tables, the README install snippets,
  `docs/ROADMAP.md` and the bug report form follow it, and
  `scripts/verify-release-versions` checks them.
- Work for the next version happens on `feat/v<next-version>`, the only
  pull request into `main`; every other branch merges into it.

## The gate

`make check` runs `dotnet format --verify-no-changes`, the Release build
(warnings as errors, `latest-recommended` analysers, CA1502 and
SonarAnalyzer complexity ceilings), the unit tests under coverlet with
100 % line and branch coverage, `dotnet pack` with package validation,
the vulnerable-package audit, the version check and the Halstead check.
`make docs-lint` runs markdownlint, codespell and REUSE. "It compiles"
is not done: say which command ran and what it printed.

## Rules

- **Native, local, CSPRNG only.** Every random draw goes through
  `IRandomSource`; the production implementation is
  `System.Security.Cryptography.RandomNumberGenerator`. Never use
  `System.Random`, never reduce raw bytes with a modulo, and never add
  a network call or telemetry.
- **Match the core, do not improve on it.** A generator ported here
  makes the same draws, in the same order, and produces the same output
  and error codes as `packages/core`, as the conformance kit's draw
  tapes will prove. A behaviour change starts in jspassgen.
- **Never log or expose secrets.** No generated value appears in an
  exception message, a `ToString()` or debug output. The library logs
  nothing.
- **No third-party runtime dependency.** Test and analyser packages are
  pinned exactly in `Directory.Packages.props`.
- Every behaviour change lands with its test in the same commit;
  coverage never drops below 100 %.
- Every source file opens with the SPDX header `Apache-2.0 OR MIT`;
  Markdown files carry it on line 1.
- Never claim a feature, a benchmark or a registry publication that
  does not exist. The README says what is not available yet.

## Leave alone

- Generated output (`bin/`, `obj/`, `artifacts/`) and the
  `packages.lock.json` files, which only `dotnet restore` writes.
- `KEYS.asc`, `LICENSE*`, `LICENSES/` and `DCO.txt`, unless the task is
  about them.
