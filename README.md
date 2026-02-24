# SPEEDMA - Unity Simulation

**Interactive Digital Twin Runtime Environment for the SPEEDMA Project**

---

## 📖 About

This repository contains the Unity-based interactive simulation for **SPEEDMA** — **S**imulation **P**latform for **E**arly 20th-Century **D**omestic **E**nergy **M**anagement **A**pparatus.

This component provides a real-time, interactive 3D environment where users can explore, operate, and understand a historically accurate digital twin of a rare 1920s Portuguese domestic hybrid energy system.

---

## 🎯 Purpose

This simulation enables users to:

- **Visualize Energy Flow**: See electricity generation, storage, and distribution in real-time
- **Operate Historical Equipment**: Interact with authentic controls (switches, rheostats, circuit breakers)
- **Experience Historical Scenarios**: Simulate documented operational modes from the 1920s
- **Learn Through Interaction**: Understand early electrification technology through hands-on exploration
- **Validate Technical Hypotheses**: Test configurations and observe system behavior

---

## 🏛️ The System

The simulation recreates a complete domestic energy installation from Chamusca, Portugal, donated to Museu Faraday in 2023:

### Components

1. **Crossley Thermal Engine** - Single-cylinder spark ignition engine with flywheel
2. **ASEA DC Dynamo** - Belt-driven 115V-160V generator
3. **ASEA Three-Phase Induction Motor** - 3hp, 380V (for grid charging)
4. **Tudor Lead-Acid Battery Bank** - 60 open-cell batteries in series
5. **Marble Control Board** - Voltmeters, ammeters, switches, and rheostats

### System Evolution

The installation evolved through three technological phases:

- **Mechanical Phase** (pre-1918): Crossley engine for mechanical work
- **DC Electrification** (1918-1923): Addition of dynamo and batteries for domestic lighting
- **AC Integration** (c.1929): Connection to public grid via three-phase motor

---

## 🎮 Simulation Features

### Interactive Controls

- Start/stop Crossley engine
- Engage/disengage dynamo belt drive
- Switch between battery discharge and charging modes
- Monitor voltage and current on period-accurate instruments
- Control rheostats for voltage regulation
- Toggle between engine-drive and grid-drive operation

### Historical Scenarios

Users can simulate documented operational modes:

1. **Scenario A**: Running the house on batteries (Engine OFF)
2. **Scenario B**: Charging batteries using the Crossley engine (c.1923)
3. **Scenario C**: Charging batteries using AC Grid and Induction Motor (post-1929)

### Physics Simulation

- **Electrical Model**: Simplified calculations for voltage (V), current (I), and power (W)
- **Energy Flow**: Real-time tracking between generation, storage, and consumption
- **State Machine**: Handles transitions between operational modes
- **Belt Drive Mechanics**: Visual representation of mechanical power transmission

### Visualization

- Free camera navigation through the machine room
- Component highlighting and information overlays
- Animated mechanical parts (flywheels, belt, motor shafts)
- Period-accurate lighting and materials

---

## 🔗 Related Repositories

This Unity simulation is part of the larger SPEEDMA Digital Twin project:

- **[speedma-rag](https://github.com/Dreadfxl/speedma-rag)** - Retrieval-Augmented Generation system for historical documentation queries
- **speedma-blender** *(coming soon)* - 3D modeling and digital restoration source files

### Integration

- 3D models created in **Blender** are imported into Unity as optimized assets
- **RAG system** provides contextual information and historical documentation within the simulation
- Unity builds can query the RAG API for component details, historical context, and technical specifications

---

## 🛠️ Technical Architecture

### Unity Version

*(To be specified - recommend Unity 2022 LTS or later)*

### Key Systems

- **Energy Manager**: Central state machine controlling power flow
- **Component Controllers**: Individual scripts for each machine component
- **UI System**: Period-appropriate gauges and information displays
- **Input Manager**: Keyboard/mouse and potential VR controller support
- **RAG Integration**: API client for historical documentation retrieval

### Asset Pipeline

1. 3D models exported from Blender (FBX format)
2. Texture optimization for real-time rendering
3. LOD (Level of Detail) generation for performance
4. Physics collider setup
5. Material and shader assignment

---

## 🚀 Development Roadmap

### Phase 1: Core Setup
- [ ] Unity project initialization
- [ ] Import 3D models from Blender
- [ ] Basic scene layout and lighting
- [ ] Camera controller implementation

### Phase 2: Component Logic
- [ ] Crossley engine state machine
- [ ] Dynamo generation simulation
- [ ] Battery charge/discharge model
- [ ] Control board interaction system

### Phase 3: Energy Flow
- [ ] Electrical circuit simulation
- [ ] Power calculation system
- [ ] Visual feedback (gauges, indicators)
- [ ] Belt drive animation

### Phase 4: Historical Scenarios
- [ ] Scenario loading system
- [ ] Guided simulation modes
- [ ] Performance metrics tracking

### Phase 5: Polish & Integration
- [ ] RAG API integration
- [ ] Audio design (engine sounds, electrical hum)
- [ ] UI/UX refinement
- [ ] Documentation and tutorials
- [ ] Build optimization

---

## 📚 Documentation

Detailed documentation will include:

- Architecture diagrams
- API reference for RAG integration
- User interaction guide
- Developer setup instructions
- Physics model specifications

---

## 🤝 Collaboration

This project is developed in collaboration with:

- **Museu Faraday** - Instituto Superior Técnico (IST), Universidade de Lisboa
- **ISEL** - Instituto Superior de Engenharia de Lisboa

Technical validation and historical accuracy provided by Museu Faraday restoration team.

---

## 📄 License

*(To be determined)*

---

## 🙏 Acknowledgements

Special thanks to:

- **Eng. Miguel Tavares Pestana** for donating the Chamusca estate equipment
- **Museu Faraday team** for technical documentation, restoration expertise, and validation
- **Prof. Moisés Piedade** (IST) for historical research and technical guidance
- **Prof. Pedro Fazenda & Prof. João Casaleiro** (ISEL) for academic supervision

---

**Project Lead**: Pedro Grilo  
**Institution**: Instituto Superior de Engenharia de Lisboa (ISEL)  
**Date**: February 2026
