# Tactical Darkness Game

A tactical horror battle royale where 10 players fight in complete darkness with only a light halo to see. Features one-shot kills, shrinking map by deaths, and unique items.

## Core Mechanics

- **Darkness-Based Visibility**: 360° light halo reveals only what's close
- **One-Shot Kills**: 1 bullet = instant death, no HP
- **Shrinking Map**: Map reduces with each kill, not by timer
- **Unique Items**: 5 special items with 15-second effects
- **AI Bots**: Fill lobbies to 10 players with sound/light-based AI

## Tech Stack

- Unreal Engine 5.x
- C++ / Blueprints
- Git for version control

## Project Structure

```
Source/
  TacticalDarkness/
    Core/           - Game mode, player controller
    Player/         - Player character, movement
    Combat/         - Shooting, damage system
    Items/          - Item system, effects
    AI/             - Bot behavior
    Map/            - Shrinking, visibility
Content/
  Blueprints/       - Visual scripting
  Materials/        - Lighting, shaders
  Maps/             - Game levels
  Audio/            - Sound effects
```

## Getting Started

1. Clone this repository
2. Open `TacticalDarkness.uproject` in Unreal Engine
3. Build and run

## Team

- Project Manager
- Programmers
- Artists
- Designers

## License

Private - All rights reserved
