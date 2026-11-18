<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Governance

`jspassgen-sdk-dotnet` is one repository of the jspassgen suite,
released in lockstep with
[jspassgen](https://github.com/sebastienrousseau/jspassgen) (the core
and its npm package) and its other members.

## Roles

| Role | Who | Responsibilities |
| :--- | :--- | :--- |
| Maintainer | Sebastien Rousseau ([@sebastienrousseau](https://github.com/sebastienrousseau)) | Reviews and merges pull requests, decides when a release opens, signs tags, publishes the NuGet package, answers security reports, owns the repository settings and secrets |
| Contributor | Anyone | Opens issues and pull requests under [CONTRIBUTING.md](CONTRIBUTING.md), signing off every commit ([DCO.txt](DCO.txt)) |
| Automated contributor | AI coding agents | Work under [AGENTS.md](AGENTS.md): prepare commits and pull requests, never merge, tag or publish |

This is a single-maintainer project. There is no committee and no vote:
the maintainer decides, in the open, on the pull request or issue
concerned.

## How decisions are made

- **Day to day**: on the pull request. CI ([DEVELOPMENT.md](DEVELOPMENT.md))
  is the bar every change meets; review checks what CI cannot.
- **Decisions that will be questioned later** get an Architecture
  Decision Record in [`docs/adr/`](docs/adr/index.md), merged with the
  change it explains.
- **What a generator produces** belongs to jspassgen's core. A change is
  decided there and reaches this port through the conformance kit; this
  repository does not diverge from it.
- **Versions** move by `0.0.1`, with the suite; the maintainer opens a
  release with `feat/v<next-version>`.

## Access continuity

- **Source**: the Git history is public and mirrored by every clone.
  The licence (Apache-2.0 OR MIT) lets anyone fork and continue it.
- **Releases**: tags are signed with a key the maintainer publishes in
  `KEYS.asc`; a successor publishes a new key there, in a signed commit,
  before signing a tag with it.
- **NuGet**: nothing is published yet. The release workflow publishes
  the package by trusted publishing (OIDC) from the protected `nuget`
  GitHub environment, so no long-lived API key exists to lose.
- **GitHub**: a successor is added as an admin of this repository and of
  jspassgen, which owns the conformance kit, before any handover.
- **Security reports**: GitHub private vulnerability reporting stays on,
  so reports reach whoever holds admin rights.

## Changing this document

Changes to governance go through a pull request like any other change,
and are recorded in the [CHANGELOG](CHANGELOG.md).
