<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Security Policy

## Supported versions

This SDK follows the jspassgen suite cadence. `0.0.23`, the suite's
second lockstep release, is the latest. While pre-`1.0`, the latest
released `0.0.x` receives security fixes; older `0.0.x` versions do
not.

| Version | Status | Receives security fixes? |
| :--- | :--- | :--- |
| `0.0.23` | Latest release | Yes |
| `0.0.22` | Previous release | No |

## Reporting a vulnerability

**Do not open a public issue for security reports.**

Report privately through GitHub Private Vulnerability Reporting:
<https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/security/advisories/new>

Include the SDK version or commit, the .NET version, and the smallest
reproduction. Never send a password, passphrase or other secret you
use. A vulnerability in the JavaScript core belongs to
[jspassgen](https://github.com/sebastienrousseau/jspassgen/security).

## Security posture

### Scope

This library generates random values on the caller's machine. It makes
no network call, writes no files, logs nothing and has no third-party
runtime dependency.

### How it handles risk

| Surface | How it's handled |
| :--- | :--- |
| **Randomness** | Every draw comes from `System.Security.Cryptography.RandomNumberGenerator`, the platform CSPRNG. `System.Random` is never used. |
| **Bias** | `NextInt(max)` uses `RandomNumberGenerator.GetInt32`, which samples by rejection; a unit test fails on a modulo-reduced implementation. |
| **Secrets in logs** | The library logs nothing and puts no generated value in an exception message. |
| **Dependencies** | No runtime dependency. Test and analyser packages are pinned exactly and locked with content hashes; CI fails on any package `dotnet list package --vulnerable` reports. |

### Cryptography status

The library implements no cryptographic primitive. Randomness is the
operating system's CSPRNG, reached through .NET's
`RandomNumberGenerator`.
