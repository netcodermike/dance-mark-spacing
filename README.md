# Dance Mark Spacing

Measure minimum spacing and flag close pairs in a rehearsal formation.

![Sample run from Dance Mark Spacing](docs/demo.webp)

## Run

Install .NET 8 SDK and runtime. Run these commands from the repository root.

```sh
dotnet build --nologo
dotnet bin/Debug/net8.0/App.dll sample.json
```

Replace `sample.json` with a JSON file following the same structure. The app prints a JSON report to standard output.

Give [label, x, y] markers with distinct labels. Distances are Euclidean, and a pair is flagged when its distance is below the limit.

## Verify

Run `python check.py` with Python 3.11 or newer after the build step when applicable. The check verifies the bundled scenario and confirms that an invalid input returns an error.
