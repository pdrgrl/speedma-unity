# SPEEDMA - Unity Simulation

**Interactive 3D Digital Twin Runtime Environment for the SPEEDMA Project**

---

## 📖 About

This repository contains the Unity-based interactive simulation for **SPEEDMA** — **S**imulation **P**latform for **E**arly 20th-Century **D**omestic **E**nergy **M**anagement **A**pparatus.

It provides a real-time, interactive 3D digital twin of a rare 1920s Portuguese domestic hybrid energy system. Users can explore, operate, and learn about the historical equipment in the machine room, with physical equations driven by an FMU co-simulation backend and context-graph RAG documentation.

---

## 🏛️ The System

The simulation recreates a complete domestic energy installation from Chamusca, Portugal:

### Components
1. **Crossley Thermal Engine** - Single-cylinder spark ignition engine with flywheels and belt transmission.
2. **ASEA DC Dynamo** - Belt-driven generator supplying 115V-160V.
3. **ASEA Three-Phase Induction Motor** - 3hp, 380V motor acting as prime mover in c.1929.
4. **Tudor Lead-Acid Battery Bank** - 60 open-cell batteries in series.
5. **Marble Control Board** - Voltmeters, ammeters, breakers, switches, and rheostats.

### Operational Scenarios
- **Scenario A:** House runs entirely on batteries (engine OFF).
- **Scenario B:** Crossley engine charges batteries via DC dynamo (c.1923).
- **Scenario C:** AC grid charges batteries via induction motor + dynamo (c.1929).

---

## 🛠️ Technical Architecture

### Core Technical Specs
* **Unity Version:** Unity 6000 LTS (`6000.3.9f1`)
* **Render Pipeline:** Universal Render Pipeline (URP)
* **Input System:** New Input System (`com.unity.inputsystem`)
* **Target Platforms:** WebGL (primary), Standalone PC

### Key Components & Scripts
- **`ChamuscaInteractable`**: Attached to components to enable hotspot generation, billboard glowing indicators, camera focus parameters, and custom hover coloring.
- **`ChamuscaInteractionManager`**: Raycasts the scene using mouse/touch coordinates, driving hover states and dispatching selection events to the UI manager.
- **`ChamuscaUIManager`**: Connects in-scene TextMeshPro HUD labels and serializes component selections to the WebGL `.jslib` bridge (`SpeedmaBridge.jslib`) to notify the web host.
- **`InspectionCamera`**: Handles orbit navigation, scrolling zoom, mobile pinch-to-zoom gestures, and transitions the focus target smoothly onto selected interactables.
- **`FmuToggleSwitch` / `FmuBreakerSwitch` / `VoltmeterSelectorFuse`**: Physical controls that map user input (e.g. lever drags and clicks) directly to FMU inputs.
- **`SpeedmaSimManager`**: Manages the API communications and state sync with the backend FMU server.

---

## 🔧 Editor Tools

To ease scene editing and setup, custom editor utilities are included under `Assets/Editor/`:

1. **Setup Hotspot Mesh Colliders** (`Tools/Setup Hotspot Mesh Colliders`)
   - Locates all `ChamuscaInteractable` instances in the active scene and automatically attaches a convex `MeshCollider` to their child meshes (handling undo states cleanly) so raycasts detect clicks accurately.
2. **Pivot Editor** (`Tools/Pivot Editor`)
   - Allows repositioning the pivot point of selected GameObjects. Supports the non-destructive **Parent Wrapper** method (recommended) or direct **Modify Mesh Vertices** offset.

---

## 🚀 Development Roadmap

- [x] **Phase 1: Core Setup** - Blender FBX imports, layout, URP lighting.
- [x] **Phase 2: Component Logic** - Engine animation, switch dragging, physical switch handles.
- [x] **Phase 3: Energy Flow** - FMU integration, voltage/current metering, belt drive animation, active visual feedbacks.
- [x] **Phase 4: Historical Scenarios** - Multi-scenario setup and controls reset.
- [x] **Phase 5: Polish & WebGL Integration** - WebGL build optimizations, custom `.jslib` bridge with iframe support, billboard glowing hotspot dots with additive rendering, and context-graph RAG connection.

---

## 🤝 Collaboration

This project is developed in collaboration with:
- **Museu Faraday** - Instituto Superior Técnico (IST), Universidade de Lisboa
- **ISEL** - Instituto Superior de Engenharia de Lisboa (ISEL)

**Project Lead**: Pedro Grilo  
**Institution**: Instituto Superior de Engenharia de Lisboa (ISEL)  
**Date**: July 2026
