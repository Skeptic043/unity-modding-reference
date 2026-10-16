# Placement displacement and offset searches

When moving a placement relative to an existing valid reference, distinguish translating the chosen candidate from translating the search origin. A search that minimizes distance to an obstacle can absorb the origin change by choosing a shorter distance, leaving the final object where it started.

[Placement](README.md)

The [complete arithmetic probe](../../evidence/placement/placement-offset-search.ps1) demonstrates this without game files. With clearance requiring x >= 10, the original search selects origin 0 plus distance 10. Moving the origin to 4 lets the search select distance 6, so the final position remains 10. Translating the chosen valid position instead produces 14, which retains the requested four-unit displacement and still passes clearance. The probe was executed on Windows and all three checks passed.

For a deliberate relative displacement, translating an already valid reference candidate preserves the requested offset before the translated shape is evaluated against the normal constraints. Translating the search origin instead can silently shorten the offset, and selecting another reference candidate can also change the final displacement relative to the intended reference.

For saved poses, the stored position is already the candidate under evaluation. Translating the validation reference is different from translating the stored object again.

## Evidence limits

This is a search-method observation, not Unity physics, terrain, attachment or gameplay evidence. The one-axis example does not establish safety in a real scene. Changes to the search objective, frame axes, ranking or fallback rules require a new check of the reasoning. Game-specific geometry and live results do not transfer to another project automatically.
