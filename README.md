# Silent Shores — A Non-Verbal Asymmetric VR Co-op Game *(Work in Progress)*

**Silent Shores** is an experimental asymmetric multiplayer game that explores non-verbal communication and collaboration between players using VR and tangible interfaces.

> ⚠️ **This is a prototype in early development. Expect placeholder art, basic mechanics, and experimental features.**

## 🏴‍☠️ Project Concept

One player wears a **VR headset** (Meta Quest) and navigates a pirate island in first-person view, trying to find a hidden treasure.

Meanwhile, **one or more table players** interact with a **physical tabletop interface** (with tangible objects and a top-down map view) to guide the VR player — **but no one is allowed to speak.**

Players must cooperate using non-verbal cues such as:

- Sound triggers (e.g. bell = stop, drum = go)
- Visual markers (e.g. lights, compass flashes)
- Symbolic signals (icons or feedback in the environment)
- Physical interaction with objects on the board

## ❓ Research Questions

- How can non-verbal, asymmetric communication support cooperation and navigation in digital play?
- How do players experience presence and collaboration when communication is restricted to physical and interactive signals?
- Which forms of feedback (audio, visual, symbolic) are most effective for meaning-making in silent gameplay?

## 🛠 Technologies

- **VR Player**: Meta Quest headset (Unity build)
- **Table Players**: Interactive tangible setup with tracking
- **Communication Layer**: EventBus system to sync Unity VR and tabletop inputs

## 🎮 MVP Features

### VR Player
- First-person view in a pirate island environment
- Receives real-time feedback via visuals, sound, and environmental cues
- Can send back basic signals (e.g. ping, crouch, gaze)

### Table Player(s)
- Sees a top-down map with live player location
- Interacts via physical objects (e.g. rotating blocks, placing pieces)
- Can trigger in-game signals visible to VR player

## 🧩 Gameplay & Levels (In Progress)

**Level 1**: Simple path with basic signals  
**Level 2**: Dangerous choices (player learns warning signals)  
**Level 3**: Time pressure decisions (locked gates, ambushes)  
**Level 4**: Multi-player table roles with distributed knowledge  
**Level 5**: Complex labyrinth requiring active feedback both ways

## 🌍 World Design (WIP)

- Pirate island environment with beaches, cliffs, jungles, and ruins
- Landmarks: shipwrecks, tents, treasure chests, boats
- Hazards: chasms, rivers, locked gates, traps
- Signals: smoke, glowing stones, sound cues

## 📦 Unity Assets & Resources

- **Graphy FPS Counter** (works in VR)
- **Crest Water FREE BIRP Github variant**
- **Meta SDK**
- **Self-modeled Assets from Autodesk Maya**

## 🧪 Theoretical Inspiration

- Asymmetric Game Design (e.g. *Keep Talking and Nobody Explodes*)
- Non-verbal Communication & Affordances
- Presence & Immersion in Mixed Modal Systems
- Multimodal & Cross-device Interaction