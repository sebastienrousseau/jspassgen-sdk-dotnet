# SPDX-License-Identifier: Apache-2.0 OR MIT
#
# `make check` is the one local gate, and runs what CI's build jobs run:
# format, build (warnings as errors, .NET analysers, CA1502 and
# SonarAnalyzer complexity ceilings), unit tests with the 100 % line and
# branch coverage gate, the package build, the vulnerable-package audit,
# the version check and the Halstead check. `make docs-lint` runs the
# docs-lint workflow's checks. DEVELOPMENT.md maps every target to CI.

SOLUTION := Jspassgen.slnx
CONFIG   := Release

.PHONY: check format format-fix build test coverage pack audit verify-versions halstead docs-lint codespell reuse

check: format build coverage pack audit verify-versions halstead

format:
	dotnet format $(SOLUTION) --verify-no-changes

format-fix:
	dotnet format $(SOLUTION)

build:
	dotnet build $(SOLUTION) -c $(CONFIG)

test: build
	dotnet test $(SOLUTION) -c $(CONFIG) --no-build

# The unit tests with coverlet: fails below 100 % line and branch
# coverage of the library. The report is artifacts/coverage/.
coverage: build
	dotnet test tests/Jspassgen.Tests -c $(CONFIG) --no-build -p:CollectCoverage=true

# Builds the NuGet package (with package validation) into
# artifacts/package/. Nothing publishes it.
pack: build
	dotnet pack src/Jspassgen -c $(CONFIG) --no-build -o artifacts/package

# `dotnet list package --vulnerable` exits 0 even when it finds one, so
# the output is checked.
audit:
	@out="$$(dotnet list $(SOLUTION) package --vulnerable --include-transitive 2>&1)"; \
	echo "$$out"; \
	if echo "$$out" | grep -q "has the following vulnerable packages"; then exit 1; fi

# Every file that states the version agrees with the csproj.
verify-versions:
	scripts/verify-release-versions

# Python lint tools (codespell, reuse and pygments), hash-checked from
# ci/lint-requirements.txt, in a local virtual environment.
LINT_VENV ?= .venv-lint

$(LINT_VENV)/.installed: ci/lint-requirements.txt
	python3 -m venv $(LINT_VENV)
	$(LINT_VENV)/bin/pip install --quiet --require-hashes --no-deps -r ci/lint-requirements.txt
	touch $@

# Halstead difficulty of every C# method, ceiling 30. The build enforces
# the other complexity ceilings (CA1502, SonarAnalyzer S3776/S138/S104).
halstead: $(LINT_VENV)/.installed
	$(LINT_VENV)/bin/python scripts/halstead.py

# markdownlint, codespell and REUSE, as the docs-lint workflow runs them.
docs-lint: codespell reuse
	npx --yes markdownlint-cli2@0.23.3 "**/*.md"

codespell: $(LINT_VENV)/.installed
	$(LINT_VENV)/bin/codespell

reuse: $(LINT_VENV)/.installed
	$(LINT_VENV)/bin/reuse lint
