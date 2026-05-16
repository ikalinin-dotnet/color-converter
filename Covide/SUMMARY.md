# Color Converter Refactoring — Summary

## Approach

I applied the **canonical pivot pattern** with RGB as the central hub. Each color space (Hex, RgbDecimal, RgbPercentage, CMYK, HSL, HSV, XYZ) implements `IColorSpace<TRepresentation>` with two methods: `ToRgb(value)` and `FromRgb(rgb)`. Any A→B conversion routes through RGB, so adding a new format = one new class, no changes to existing code. This is N implementations instead of N² pairs, which directly addresses the brief's "any color representation to any other" requirement.

## What I changed

- Extracted 7 color spaces from the 160-line controller into separate classes implementing a common interface. Math preserved verbatim.
- Introduced `RgbColor` as the canonical domain model (struct with R/G/B + normalized double properties).
- Moved color name lookup behind `IColorNameService` — separates DB concerns from conversion logic.
- Introduced `ColorConversionService` as the orchestrator the controller calls into.
- Replaced the anonymous response object with a typed `ColorConversionResponse` DTO. Property order preserves the original JSON shape exactly.
- Slimmed `ConversionController` from ~160 lines to ~12 — validation + delegation only.

## Tests

Added xUnit characterization tests using `WebApplicationFactory` covering 4 cases: `8e35ef`, `ff0000`, `123456` (null name path), and `zzzzzz` (400 path). Assertions normalize JSON via `JsonDocument` round-trip to tolerate whitespace but catch any field, order, type, or precision regression. Tests were written **first**, confirmed green against the original code as a baseline, then re-run after each refactor phase as a regression guard. All 4 pass on the final code.

## Alternatives considered

- **Direct N×N converters** — rejected. Adding format 8 means writing 7 new converter classes; combinatorial growth.
- **Visitor pattern** — rejected. Double dispatch adds indirection without benefit here; color formats are independent data shapes, not polymorphic behaviors.

## What I'd do with more time

- Auto-discover `IColorSpace` implementations via reflection or source generators, so new formats register themselves without touching `ColorConversionService`.
- Property-based tests (FsCheck) for round-trip invariants like `Hex → RGB → Hex == original`.
- Cache the color-name DB lookup (currently hits SQLite per request); the named-color set is small and static.
- OpenAPI/Swagger documentation, plus proper `ProblemDetails` responses for validation errors.
- Consider whether `RgbColor` should be a richer canonical model (alpha channel, gamut info) if future formats require it.

## Notes on the brief

- The task explicitly said not to implement new conversions or fix existing math — I preserved all calculation logic bit-for-bit. The `ComputeXYZ` helper moved verbatim into `XyzColorSpace`.
- JSON output is byte-identical to the original. Worth noting: `System.Text.Json` drops trailing zeros, so `269.0` in the brief renders as `269` in actual output. This was already the case in the original code; the refactor preserves it.
- Test project is colocated inside `Covide.Web` rather than a separate assembly — unconventional for production, but it eliminated a class of DI/seeding complications and got the safety net up faster within the 2-hour window.
