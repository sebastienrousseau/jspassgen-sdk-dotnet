<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Getting support

## Read first

- [README.md](README.md): what exists today, what does not, install
  from source and the quick start.
- [`docs/`](docs/index.md): architecture, policies, roadmap and decision
  records.
- [DEVELOPMENT.md](DEVELOPMENT.md): building, testing and every CI gate
  on your own machine.

## Questions and how-to

Open a [GitHub issue](https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/issues/new/choose).
Include:

- the `Jspassgen` package version or SDK commit, and the .NET SDK version
  (`dotnet --version`);
- the smallest code that shows the problem;
- the exception type and message.

Never paste a password, passphrase or other secret you use.

A question about what a generator should produce belongs in
[jspassgen](https://github.com/sebastienrousseau/jspassgen/issues): this
port follows the core.

## Bugs and feature requests

Use the [bug report](https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/issues/new?template=bug_report.yml)
or [feature request](https://github.com/sebastienrousseau/jspassgen-sdk-dotnet/issues/new?template=feature_request.yml)
form. Security reports never go in an issue: follow
[SECURITY.md](SECURITY.md).

## Supported versions

While pre-`1.0`, only the latest `0.0.x` receives fixes.

| Version | Status | Supported |
| :--- | :--- | :--- |
| `0.0.23` | Latest release | Yes |
| `0.0.22` | Previous release | No |

## Response times

This project has a single maintainer (see [GOVERNANCE.md](GOVERNANCE.md)).
Issues are usually answered within a few business days; there is no
guaranteed response time.
