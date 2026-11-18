<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Architecture

## Today

```text
src/Jspassgen/
├── IRandomSource.cs       the random port: GetBytes(count), NextInt(max)
└── CryptoRandomSource.cs  the port on RandomNumberGenerator
```

`IRandomSource` mirrors the two required methods of jspassgen's
`RandomGeneratorPort` (`packages/core/src/ports/RandomGeneratorPort.js`):

| jspassgen (JavaScript) | This port (.NET) |
| :--- | :--- |
| `generateRandomBytes(byteLength)` | `byte[] GetBytes(int count)` |
| `generateRandomInt(max)` | `int NextInt(int max)` |
| `RangeError` when the argument is not a positive integer | `ArgumentOutOfRangeException` below 1 |

The JavaScript methods return promises because the Web Crypto API is
reached asynchronously in some hosts. `RandomNumberGenerator` is
synchronous, so the .NET methods are too.

`CryptoRandomSource` is stateless: `GetBytes` calls
`RandomNumberGenerator.GetBytes`, and `NextInt` calls
`RandomNumberGenerator.GetInt32`, which rejects out-of-range samples
instead of reducing raw bytes with a modulo.

## Planned

Each generator in the core's `PASSWORD_TYPES` is ported as a class that
takes an `IRandomSource`. The conformance kit's draw tapes then replay,
through a test implementation of the port, the exact draws the core
makes, and the port must produce byte-identical output
([ADR 0001](adr/0001-native-port.md)). Nothing beyond the random source
exists yet.
