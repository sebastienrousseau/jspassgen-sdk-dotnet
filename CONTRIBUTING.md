<!-- SPDX-License-Identifier: Apache-2.0 OR MIT -->

# Contributing

This repository is the planned native .NET port of the
[jspassgen](https://github.com/sebastienrousseau/jspassgen) core. It
holds only the random source today; the generators are ported once
jspassgen's conformance kit exists. Read [AGENTS.md](AGENTS.md) for the
rules that must not drift, and [DEVELOPMENT.md](DEVELOPMENT.md) for the
toolchain and every gate.

## Before you open a pull request

You need the .NET 10 SDK (`mise install` gives the pinned one).

```sh
make check          # format, build, 100 % coverage, pack, audit, versions, Halstead
make docs-lint      # markdownlint, codespell, REUSE
```

`make check` is what CI's build jobs run on every pull request, on
Linux, Windows and macOS.

## What the tests are for

- `CryptoRandomSourceTests`: the random source returns the requested
  length, stays in range, rejects arguments below 1, is uniform across
  a small range (chi-squared) and shows no modulo bias.

## Behaviour belongs to the core

A generator here must make the same random draws, in the same order,
and produce the same output and errors as jspassgen's `packages/core`.
A change to what a generator does is made in jspassgen first and
reaches this repository through the conformance kit.

## Commits

Conventional commits (`feat:`, `fix:`, `docs:`, ...), an imperative
subject of 50 characters at most, and a body wrapped at 72 that says
what and why. Every commit is signed off (`git commit -s`), certifying
the [Developer Certificate of Origin](DCO.txt), and signed.

## Licence

Apache-2.0 OR MIT, at your option. By contributing you agree your work
is released under the same dual grant.
