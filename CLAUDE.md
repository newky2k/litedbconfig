# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

`LiteDB.Config` — an extension for LiteDB that adds EF-style entity configuration for `BsonMapper` (`IEntityBuilderConfiguration<T>` classes applied from an assembly with `ApplyConfigurationsFromAssembly`). One NuGet package (`LiteDB.Config`, project `LiteDb.Config/LiteDb.Config.csproj`, assembly `LiteDb.Config`) plus `SampleApp`, a console harness. There are no automated tests.

## Build

```bash
dotnet restore LiteDb.Config.sln
dotnet build LiteDb.Config.sln -c Release
```

- The library multi-targets `netstandard2.0;net8.0;net9.0;net10.0;net11.0` and depends on `LiteDB` 5.0.21. The sample targets `net10.0-windows` and `net11.0-windows` (it writes to `D:\test.db`). Build with the .NET 10 and .NET 11 SDKs (`global.json` rolls forward to the latest major); on Linux or macOS add `-p:EnableWindowsTargeting=true`.
- `GeneratePackageOnBuild=true`, so every build of the library produces a `.nupkg` (with `README.md` and `DSIcon.png`). Release builds add SourceLink and an embedded PDB.
- The assembly is strong-name signed with `LiteDb.Config/DSoft.snk`; do not remove it.
- The csproj file name is `LiteDb.Config.csproj` (lower-case `b`); references must match that case so the build works on Linux.

## Branching

- `development` is the working branch: branch from it and open pull requests into it. Never commit or open a pull request directly against `main`.
- `main` is the release branch. Every push to `main` runs `release.yml`, which publishes the package to nuget.org and creates a GitHub release, so only merge `development` into `main` when a release is intended, and only when asked to release.

## CI

GitHub Actions (`.github/workflows/`) on `windows-latest`:

- `ci.yml` builds Release for pull requests into `main` and `development`; it publishes nothing.
- `release.yml` runs on every push to `main` (changes only to Markdown or workflow files are skipped; run it by hand to test a workflow change). It builds Release as `5.0.<yyMM>.<run number>` plus `RELEASE_SUFFIX` (empty for a stable version), pushes `LiteDB.Config` to nuget.org with Trusted Publishing (OIDC, `NUGET_USER` secret, `nuget` environment), then tags the commit `v<version>` and creates a GitHub release with the package attached.
