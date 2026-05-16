# Refactoring summary

## Approach

I used RGB as a canonical intermediate representation. Each color format
(Hex, RGB decimal, RGB percentage, CMYK, HSL, HSV, XYZ) implements a generic
interface IColorSpace<T> with FromRgb and ToRgb methods. Conversion between
any two formats goes through RGB. This way adding a new format means
implementing one new class, instead of adding direct converters to every
existing format.

## Changes

- Moved all conversion math from the controller into separate classes,
  one per color format. Math was not modified, just relocated.
- Added RgbColor as canonical domain model (struct, immutable, computed
  normalized properties to avoid duplication).
- Extracted color name lookup into IColorNameService so the DB dependency
  is isolated.
- Added ColorConversionService that orchestrates the conversion flow.
- Replaced the anonymous return object with a typed DTO 
  (ColorConversionResponse). Property order matches original JSON output.
- Controller went from ~160 lines to ~12 lines, validation + delegation only.

## Tests

Added xUnit tests with WebApplicationFactory. Four test cases covering
8e35ef, ff0000, 123456 (null name case) and zzzzzz (400 case). Tests were
written before refactoring to confirm the original behavior, then re-run
after each refactor step. All pass on final code.

JSON comparison normalizes both expected and actual through JsonDocument
to avoid false positives from whitespace differences.

## Alternatives I considered

- Direct converter for each pair of formats: rejected, this grows as N*N
  and most pairs are never used.
- Visitor pattern: not needed here, the formats are data shapes not
  behaviors.

## What I would improve with more time

- Auto-registration of IColorSpace implementations (reflection or DI scan)
  so ColorConversionService doesn't need to know all of them explicitly.
- Cache the color name lookup. Currently it hits SQLite on every request,
  but the data is static and small.
- Unit tests per color space class. Current tests are characterization 
  tests through the API endpoint - good as a refactoring safety net, 
  but they don't isolate individual color space classes.
- Implement ToRgb for HSL, HSV, XYZ. I left these as NotImplementedException
  because the brief said not to add new conversions, and the original code
  didn't have reverse math for these. The interface is ready for them.
- Extract shared hue/saturation calculation between HslColorSpace and
  HsvColorSpace, currently duplicated.
- OpenAPI documentation.

## Notes

- The brief said not to implement new conversions or change the math.
  I kept all formulas exactly as they were, just moved them to the new
  classes. ComputeXYZ moved to XyzColorSpace as-is.
- JSON output is identical to original. Note: System.Text.Json drops
  trailing zeros, so "269.0" in the brief shows as "269". This is
  original behavior, refactor did not change it.
- Tests live inside Covide.Web instead of a separate test project.
  Not ideal for production but saved time on setup within 2 hours.