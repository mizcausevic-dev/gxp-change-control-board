# gxp-change-control-board

C# / ASP.NET operator surface for biotech GxP change packets, validation evidence gaps, approval continuity, and release-safe cutover posture.

## Why this matters

Biotech and diagnostics teams do not need another vague compliance landing page. They need a board that keeps change classification, validation evidence, SOP acknowledgments, approval continuity, audit trails, and cutover readiness visible together before weak packets slip into downstream release review.

This repo is the public proof surface for that pattern:

- `Hosted preview planned` for a browser-based GxP change-control board
- `Embedded by engagement` for teams that need the routing model inside a regulated lab or diagnostics workflow

## What it includes

- ASP.NET Core minimal API in C#
- synthetic GxP change snapshots, control gaps, and release packets
- operator surfaces for:
  - `/change-board`
  - `/control-gaps`
  - `/release-posture`
  - `/verification`
  - `/docs`
- structured JSON endpoints under `/api/*`
- static Pages export with `robots.txt`, `sitemap.xml`, and `CNAME`

## Screenshots

![Overview](./screenshots/01-overview.svg)
![Change board](./screenshots/02-change-board.svg)
![Release posture](./screenshots/03-release-posture.svg)

## Verification

- synthetic GxP change-control evidence only
- no patient, clinician, or proprietary biotech secrets
- no claim of GMP, GxP, FDA, or clinical compliance
- this is a control-plane proof surface for biotech workflow depth, not a compliance certification claim

## Local run

```powershell
dotnet test
dotnet run --project src/GxpChangeControlBoard.Api -- --demo
dotnet run --project src/GxpChangeControlBoard.Api
```

Then open:

- `http://127.0.0.1:5094/`
- `http://127.0.0.1:5094/change-board`
- `http://127.0.0.1:5094/control-gaps`
- `http://127.0.0.1:5094/release-posture`

## Render static site

```powershell
dotnet run --project src/GxpChangeControlBoard.Api -- --prerender
```

## Related docs

- [Embedded framing](./docs/KINETIC_GAIN_EMBEDDED.md)
- [Origin story](./docs/ORIGIN.md)
