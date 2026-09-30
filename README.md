# PaperDollsGame
Casual Cozy Game based in old time paper dolls

## MVP foundation

The initial code foundation is under `Assets/_Project/`. It provides local content definitions, player inventory and outfit state, event scoring, a currency balance, JSON saving, and a small game-flow coordinator. It does not include UI, a character model, a Unity scene, or production art; these are the next integration steps rather than assumptions embedded in the services.

### Data and services

- `ItemDefinition` and `EventDefinition` are ScriptableObjects for the small local MVP catalog. Item definitions contain stable IDs, slots, tags, and optional visual references. Event definitions contain tag/slot scoring rules and a reward.
- `ContentCatalog` resolves definitions by stable ID and rejects missing, blank, or duplicate IDs.
- `PlayerSaveData` stores only player-owned item IDs, equipped item IDs/slots, and balances. `SaveService` stores version 1 JSON at `Application.persistentDataPath/player-save.json`.
- `InventoryService`, `OutfitService`, `ScoringService`, and `EconomyService` are plain C# classes. Scoring is deterministic and returns a per-rule breakdown. Currency is identified by string ID, initially `GEMS`.
- `GameBootstrap` wires the services from a scene object. `GameFlowController` exposes the MVP flow; UI can start an event, equip/remove items, submit, display `LastResult`, and continue to event selection.

### Unity setup

1. Add the `Assets/_Project/Scripts/` files to a Unity project using Unity 6.
2. Create `ItemDefinition` assets, fill in unique IDs, slots, and tags, and add them to a `ContentCatalog`.
3. Create an `EventDefinition` asset with tag/slot score rules and a non-empty reward currency ID; add it to the same catalog.
4. Add `GameBootstrap` to a scene GameObject, assign the catalog, and configure starter item IDs and initial currency.
5. Build UI views that call `GameBootstrap.Flow` and render item definitions and the score result. Assign visuals through the item icon/prefab references as needed.

On the first launch, starter ownership and starting currency are written to the save. Subsequent launches load the existing save; changing starter settings does not overwrite existing player data. Outfit changes and event rewards are saved locally. Unsupported/corrupt save data raises an error instead of silently resetting progress.

### Scope and next steps

The current scoring model sums configured points for each matching item tag and each selected slot rule. It intentionally has no set bonuses, penalties by default, AI, networking, analytics, purchases, ads, or remote configuration. Google Sheets export and runtime JSON content are deferred until the catalog is large enough to benefit from bulk authoring; stable IDs provide the migration seam. JSON save data is not the content pipeline.

This repository does not yet contain a Unity project manifest, scene, test assembly, or build configuration. Consequently, the code foundation has not been compiled or exercised in the Unity Editor here. The first validation in Unity should cover content catalog ID validation, equip/replace/remove behavior, scoring breakdowns, save/reload, and submitting a round exactly once.
