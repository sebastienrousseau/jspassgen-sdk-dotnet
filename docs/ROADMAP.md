<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Roadmap

The current version is `0.0.22`, unreleased: the planned first jspassgen
suite release. The order of work follows jspassgen's
[ADR 0001](https://github.com/sebastienrousseau/jspassgen/blob/main/docs/adr/0001-ecosystem-lockstep.md).

| Step | Where | Status |
| :--- | :--- | :--- |
| Repository scaffold, gate and CI; the random source | here | Done (this version) |
| Conformance kit generator and kit | jspassgen | Done in jspassgen 0.0.21 |
| Vendor the kit (`conformance/`, `ci/core.ref`) and run it in CI | here | Next |
| Port the generators in `PASSWORD_TYPES`, validation and entropy | here | After the kit is vendored |
| Examples, benchmarks, demo, rendered manual | here | After the generators |
| Release workflow and NuGet trusted publishing of `Jspassgen` | here | Workflow done (this version); the `nuget` environment and the nuget.org trusted publishing policy before the first tag |
| First suite release; status `active` in `docs/ecosystem.json` | all members | After every member passes its gate |

Until the last row, `Jspassgen` is not on NuGet and this repository
generates no passwords.
