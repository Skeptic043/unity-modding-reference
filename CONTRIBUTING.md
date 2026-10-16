# Contributing to the public reference

Contributions should make a reusable Unity modding finding easier to understand and verify without overstating what was established. Search the existing articles first, then update the maintained explanation when new evidence changes the same answer.

## Choose the owning repository

Keep transferable Unity, BepInEx, HarmonyX and investigation methods in this repository. Keep game mechanics, installed-build findings and game-specific implementation results in a game-specific repository. A general article may link to a game case, but it should not duplicate that case's logs, asset values or implementation history.

Use one maintained public explanation for each result. Link to it instead of creating a second independently edited version.

Let the reader's question determine the article boundary. Merge a short page when it does not answer a distinct durable question, and split a long page when several systems or responsibilities need independent explanations. Remove a reader-facing page when it is redundant, stale or mismatched with the reference.

## State the evidence type and limits

Identify how each important claim was established. Useful labels include documentation, static source or binary inspection, asset inspection, synthetic fixture, Unity editor run, live gameplay, user report, save/reload, multiplayer, performance and long-session testing.

One evidence type does not imply another. A static match does not prove gameplay, a synthetic pass does not prove Unity callback behavior, and a clean loader entry does not prove feature reachability. [Compatibility evidence levels](topics/validation/compatibility-evidence-layers.md) describes the separate checks that can support a compatibility conclusion.

Record the versions that affect the result, including the engine, backend, loader, game or provider as applicable. Use exact revisions and hashes when they are needed to identify inspected binaries, packages, assets or source. Dates describe the inspection and do not promise that the result remains current.

## Link sources without publishing private inputs

Prefer stable source links pinned to a revision when the source is public. Keep exact symbols and source locators close to the claim they support. Use relative links for pages and artifacts in this repository so a reader on a branch, tag or historical commit stays on that ref. If a source is private, describe the evidence boundary without exposing private URLs, workspace paths or unpublished implementation detail.

Do not commit proprietary assemblies, extracted game assets, saves, personal logs, credentials, user names, machine-local paths or other private data. A reproduction should use synthetic or independently supplied inputs whenever possible. If proprietary input is required, document its expected identity and keep the input outside the repository.

## Article voice

Most articles should give the high-density technical answer first, then explain the mechanism, practical implications, evidence and limitations. Recommendations belong where they follow from the evidence, but a bounded finding should not be presented as universal modding doctrine. Omit investigation chronology, raw commands, intermediate logs and incidental harness detail unless the method, tool or reproduction is itself the article's subject. Procedural language remains appropriate when readers must follow that method.

Prefer concrete nouns and standard Unity, BepInEx or Harmony terminology over project shorthand. Define an abstraction when it is still the clearest term.

Keep claim strength aligned with the evidence. Say what remains untested, place a limitation beside the claim it narrows, and retain meaningful negative results. Avoid semicolons in public prose. Do not include internal plans, session notes, test logs, speculative compatibility claims, or private and unreleased project design.

## Respect licenses and attribution

Original code in this repository uses MIT and original written explanations use CC BY 4.0. Facts, measurements and discoveries are outside those copyright claims. Follow [the license scope](LICENSE.md), preserve required notices, and confirm that copied code or retained excerpts permit redistribution.

Name third-party tools, libraries and mods accurately. Link their source or release when a specific version matters. Do not imply endorsement, ownership or compatibility beyond the cited evidence.

## Review before publication

Review the complete changed-file and changed-hunk inventory. Check local and external links, headings, relative paths, versions, hashes, evidence labels, limitations, attribution and license treatment. Inspect tracked files and history for private or proprietary material before changing repository visibility.

A second review is appropriate when a change moves ownership between repositories, changes evidence support, removes retained provenance or alters publication boundaries. Publication, visibility changes and history rewriting require their own authorization.
