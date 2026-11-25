# Changelog

All notable changes to this package are documented in this file.

## [1.0.0] - 2026-11-25

### Added

- Inspector toolbar under the GameObject header, hidden by default behind a small `GameInspector` foldout (state saved in `EditorPrefs`).
- `Expand All` / `Collapse All` for every component block of the selected GameObject.
- Prefab section: `Apply Overrides` and `Revert` for prefab instances, `Create Prefab` for plain scene objects (saved to `Assets/Prefabs`, folder is created when missing).
- Component clipboard: per-component `Copy`, `Paste Values` and `Remove`, plus `Paste Component As New` for the whole object. Undo supported; `Transform` cannot be removed.
- Editor-only assembly `GameInspector.Editor`, excluded from player builds.
