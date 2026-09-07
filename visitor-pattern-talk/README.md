# Visitor — Under The Hood

A 28-slide draft continuation of the Composite talk, using background artwork extracted from the supplied PowerPoint. The original presentation is unchanged.

## Present

```powershell
cd visitor-pattern-talk
npm install
npm run dev
```

Open http://localhost:3031. Use arrow keys or Space to advance; the data-collection slide reveals its bullets one at a time. Press P for presenter mode.

Edit `slides.md` for content and speaker notes; edit `style.css` for typography and layout. Fonts are bundled through npm rather than loaded from Google Fonts. Backgrounds are in `public/`.

## Export and build

```powershell
npm run build
npm run export
```

The build is in `dist/`; the PDF is `visitor-draft.pdf`. PowerPoint export is possible through Slidev but produces images rather than editable PowerPoint text.

## C# demo

```powershell
dotnet run --project demo
```

The demo checks the dispatch behavior and total before printing its report. Uses .NET 8 or later and C# 12 syntax.

Source references appear in the relevant slide notes. Company examples are illustrative; the file sizes are synthetic demo values.
