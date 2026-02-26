# Gemini Code-Assist Guidance

This file provides guidance to Gemini when working with code in this repository.

## Project Overview

This is a Unity 6 (6000.0.62f1) creature simulation game named ShoeboxSim. The game simulates small creatures ("Grems") in a shoebox diorama. The project uses the Universal Render Pipeline (URP) and Unity's new Input System.

## Build & Run

- **This is a Unity project and must be opened and built through the Unity Editor.**
- There is no command-line build or test pipeline.
- **Unity version**: 6000.0.62f1
- **Render pipeline**: URP (`com.unity.render-pipelines.universal`)
- **Input System**: New Input System (`com.unity.inputsystem`). All input is handled via `Mouse.current` and `Keyboard.current`.

## Architecture

### AI Systems

- **Preferred System**: The Behaviour Tree System (`Assets/Scripts/BTs WIP/`) is the designated AI approach for Grem behavior. All new AI development and modifications should focus on this system.
    - **`TaskCheckDragging`**: This node now handles the complete Grem dragging functionality, including input detection, Grem movement with the mouse, and applying a visual "struggle" effect.

- **Deprecated System**: The Behaviour-Component System (`Assets/Scripts/GremData/`) is considered deprecated and will no longer be actively used or developed.

### Food System

- Located in `Assets/Scripts/GremData/Food/`.
- **`FoodData`**: A `ScriptableObject` defining food properties.
- **`FoodItem`**: A MonoBehaviour on food GameObjects; holds a `FoodData` reference.
- **Tag**: Food objects must be tagged with `"Food"`.

### Game Economy

- **`CurrencyManager` (Singleton MonoBehaviour)**: Manages the game's currency, allowing other systems to add or spend currency.
- **`GremCurrencyProducer` (MonoBehaviour)**: Attached to Grem GameObjects, this script handles currency production outside the Behavior Tree. It uses `GremData` for production values, reduces production rate if the Grem is hungry or sleepy, and stops production when the Grem is sleeping.

### Player Interaction

- **`Feed`** — Spawns `marshmallowPrefab` at mouse click position on the floor. Raycasts against `floorLayer` and `gremLayer` — clicking a Grem does not spawn food.
- **`InputManager` (Static Class)**: Manages global input states, such as `IsFeedingEnabled`. Toggled by 'F' key. Designed for easy future integration with UI elements.
- **`InputManagerHook` (MonoBehaviour)**: A simple MonoBehaviour to call `InputManager.HandleGlobalInput()` in `Update()`, allowing the static `InputManager` to process input. Should be attached to an active GameObject in the scene.
- **`FurniturePlacementManager`**: Handles the selection, placement, and manipulation of furniture items within the scene. Activated by the 'B' key, allowing users to pick a furniture prefab, move it with the mouse, rotate it with 'R', and place it with left-click (or cancel with right-click).
- **Grem Dragging**: The dragging of Grems is now handled by the `TaskCheckDragging` Behavior Tree node using physics. When dragging, the Grem's `Rigidbody` is controlled to pull it towards the mouse position, allowing it to collide and be stopped by physical barriers like walls.

### Camera

- Located in `Assets/Scripts/CameraControl/`.
- A pivot-based camera rig.
- `PanCamera` — Full-featured: orbit (right-click), pitch (vertical right-click), pan (middle-click or shift+right-click), scroll zoom. Clamps within `boxLimits`.
- **`PlayerCamera`** — Simpler: horizontal orbit + zoom only.
- **`WallFader`** / **`WallTransparency`** — Raycasts from camera to pivot; walls in the way get dissolved via a `_Dissolve` shader property on `DitherShader.shadergraph`.

### Other

- **`Assets/IA/`**: This folder and its contents should be ignored by the AI agent for modifications, but NOT from version control (`.gitignore`). It contains unused or deprecated steering AI concepts.
- **`Assets/Shaders/DitherShader.shadergraph`** — URP Shader Graph with a `_Dissolve` float property on the wall transparency system.
- **`Assets/TO DO/`**: This folder is used to store text files (e.g., `ZZZ_implement.txt`, `FurniturePlacement_implement.txt`) that provide manual setup instructions for functionalities requiring Unity Editor or prefab modifications that cannot be automated via code.

## Key Conventions

- All scripts are in the global namespace (no C# namespaces) except `SteeringOutput` (`IA26.Movement`).
- ScriptableObjects are created through Unity's asset menu under `GremSystem/`.
    - **`GremData`**: Now includes `currencyProductionValue` and `currencyProductionRate` fields to define how much and how often a Grem produces currency. Also includes `squashAmount` and `squashSpeed` for visual squish and squash effects during movement, and `dragPullForce` to control the strength of physics-based dragging.
- **`GremBT`**:
        - Now tracks `previousPosition` to calculate movement direction.
        - Includes `ApplySquashAndSquishEffect(float progress, float amount, float speed)` for generalized visual scaling.
        - Includes `ResetSpriteScale()` to restore original scale.
        - Includes `FlipSpriteToDirection(Vector3 direction)` to orient the Grem's sprite based on its horizontal movement.
        - **Physics-Based Dragging**: Now requires a `Rigidbody` component and manages physics-driven movement when `isBeingDraggedPhysics` is true, moving towards `dragTargetPosition` in `FixedUpdate()`.
- Input is always read via Unity's new Input System static accessors (`Mouse.current`, `Keyboard.current`), never the legacy `Input` class.
- Sprites are billboarded to face the camera in `LateUpdate` using `Camera.main`.
    - **`GremBT`**: Now includes `ApplySquashAndSquishEffect` and `ResetSpriteScale` methods to visually modify the Grem's sprite based on movement.
    - **`TaskWander`**: Now calls `GremBT.ApplySquashAndSquishEffect` during movement to animate the Grem.
    - **`TaskMoveToFood`**: Also calls `GremBT.ApplySquashAndSquishEffect` during movement to animate the Grem.
    - **`TaskSleep`**: Now calls `GremBT.ApplySquashAndSquishEffect` with sleep-specific parameters to create a breathing effect.
- Food GameObjects must have the `"Food"` tag to be discoverable by Grem AI.
- Grem movement is bounded by `minBounds`/`maxBounds` defined in `GremData`.

## Commenting Conventions

- All comments have been removed from C# scripts. Do not add any new comments.

## External Dependencies & Manual Setup

- If a requested functionality requires manual work in the Unity Editor (e.g., changing Inspector properties, creating new assets/prefabs, or any step that cannot be automated via code changes), a unique text file detailing the necessary steps will be created in the `Assets/TO DO/` folder. This file will serve as a guide for the user to complete the implementation.