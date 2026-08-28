# Composite Pattern Slidev Implementation Plan

**Goal:** Build a polished Slidev presentation that teaches junior developers the Composite pattern in no more than 30 minutes.

**Architecture:** Keep the deck self-contained in `composite-pattern-talk`. Use Slidev Markdown for narrative, code, click reveals, and speaker notes; use one global stylesheet for the dark filesystem-tree visual system. Reuse the existing `CompositeFileSystem` code as the technical source without moving it.

**Tech Stack:** Slidev, Markdown, Vue directives supplied by Slidev, CSS, npm.

**Spec:** `composite-pattern-talk/README.md`

## Global Constraints

- Keep prepared material to approximately 27–28 minutes.
- Write for junior developers and introduce terminology only after the problem is concrete.
- Preserve the narrative: ubiquity → naive filesystem → Composite → C# parser extension → investigation task.
- Keep code excerpts focused and visually readable.
- Do not stage or commit any files.

## Task 1: Project scaffold

- Create `package.json` with local Slidev development and production-build scripts.
- Install the current `@slidev/cli` and default theme packages locally.
- Add `.gitignore` entries for dependencies, build output, and Slidev-generated files.
- Verify the local Slidev CLI starts and can parse the deck entry file.

## Task 2: Narrative deck

- Create `slides.md` with the approved 30-minute narrative and presenter notes.
- Use progressive reveals for the opening examples and naive-design pain.
- Use line highlighting for the C# refactor.
- Include a concrete Angular Forms and DOM mapping.
- End with the investigation task and recognition heuristic.

## Task 3: Visual system

- Create `style.css` with a high-contrast dark palette, restrained cyan/violet accents, readable code, and consistent spacing.
- Use a recurring filesystem-tree motif instead of unrelated decorative imagery.
- Vary slide silhouettes while preserving typography and alignment.
- Ensure content fits at the 16:9 Slidev canvas size without relying on tiny text.

## Task 4: Verification

- Run the full Slidev production build.
- Start the local deck and inspect the cover, naive-code, Composite mapping, C# extension, and assignment slides in the browser.
- Correct clipping, wrapping, weak contrast, or excessive density.
- Run a final clean production build and report its output.
