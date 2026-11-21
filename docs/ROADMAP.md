<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Roadmap

The next version is `0.0.25`, in development on `feat/v0.0.25`. The
latest release is `0.0.24`, released 2026-11-21: the jspassgen suite's
third lockstep release, in which every member ships at `0.0.24`.
`0.0.23`, released 2026-11-20, was the second, and `0.0.22`, released
2026-11-18, the first. The order of work follows jspassgen's
[ADR 0001](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md).

| Step | Where | Status |
| :--- | :--- | :--- |
| Repository scaffold, gate and CI; the random source | here | Done (this version) |
| Conformance kit generator and kit | jspassgen | Done in jspassgen 0.0.21 |
| Vendor the kit (`conformance/`, `ci/core.ref`) and run it in CI | here | Next |
| Port the generators in `PASSWORD_TYPES`, validation and entropy | here | After the kit is vendored |
| Examples, benchmarks, demo, rendered manual | here | After the generators |
| Release workflow and NuGet trusted publishing of `Jspassgen` | here | Done; first used for 0.0.22 |
| First suite release; status `active` in `docs/ecosystem.json` | all members | 0.0.22 released; the `active` status is not set yet |

`Jspassgen` 0.0.22 is on NuGet; it generates no passwords until the
generators are ported.
