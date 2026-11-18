<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Policies

## Minimum toolchain

| Item | Floor | Enforced by |
| :--- | :--- | :--- |
| Target framework | `net10.0` | `<TargetFramework>` in every project |
| SDK to build | .NET SDK 10.0.404 | `global.json` (roll forward within the feature band), `mise.toml` |
| Platforms tested | Linux, Windows and macOS, on the GitHub-hosted `*-latest` runners | `ci.yml` build matrix |

The floor is a .NET release Microsoft supports. It moves:

- only when the current floor reaches end of support, or a newer
  release is needed for a feature the library uses;
- in a release whose CHANGELOG says so under **Changed**;
- never as a side effect of another change.

## Versioning

The version is `0.0.x` and moves by `0.0.1`, in lockstep with the
jspassgen suite. Before `1.0`, any release may change the public API;
the CHANGELOG says how.

## Output stability

Once generators are ported, the same sequence of random draws produces
the same password, validation result and entropy as jspassgen's core,
and the conformance kit proves it. A change to what a generator
produces, or to the number or order of the draws it makes, is breaking
even when no signature moves, and lands in every suite member in the
same version.

## Deprecation

A public member is marked `[Obsolete]` with the replacement named, for
at least one release, before it is removed.
