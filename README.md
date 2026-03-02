⚔️ Text-Based RPG Engine (v0.6)

A robust, modular, and scalable **RPG Game Engine** built entirely with C#. This project is not just a game, but a demonstration of complex software architecture including custom data management, dynamic combat math, and a state-based navigation system.

## 🎮 Core Gameplay Systems (What's Ready?)

The "Bones and Muscles" of the engine are fully operational. You can currently experience:

* **Hero Selection:** Choose from specialized classes (**Warrior, Archer, Mage**). Each has its own base stats and progression curves.
* **Inventory & Equipment:** A fully integrated system to inspect, equip, and unequip items. Stats like **Total HP** and **Defense** recalculate in real-time when you change gear.
* **Character Progression:** Level up by gaining EXP. Use the **Training System** to manually invest points into STR, DEX, or VIT to shape your build.
* **World Exploration:** A travel system that handles movement between different game locations and maps.
* **Data Persistence:** A high-performance **Save/Load** system using `data.json`.
* **Data Security:** A built-in "Wipe" feature that physically deletes the save file and clears the application's RAM for a fresh start.



## 🛠️ Technical Architecture

From a developer's perspective, this engine focuses on **Clean Code** and **OOP Principles**:

* **State Machine:** Navigation is handled via an `IMenuState` interface, preventing the common "nested loop" mess in console apps.
* **Data Hydration:** Static metadata (Items/Maps) is loaded from JSON and "hydrated" into the live `GameContext` during runtime.
* **Defensive Programming:** Implemented strict null-checks (`?.`, `??`) and file existence validations to ensure a crash-free user experience.
* **Computed Properties:** Battle stats are calculated dynamically, meaning armor or status effects reflect on the character instantly without manual updates.



## 🚀 The Roadmap (Future Development)

The engine is constantly evolving. Upcoming milestones include:

- [ ] **Economy System (The Blacksmith):** A full shop for buying/selling items and a scaling **Upgrade System** (+1, +2...) for weapons.
- [ ] **Quest & Crafting:** NPC interaction systems and crafting recipes using materials found in the world.
- [ ] **Strategic PvE & Dungeons:** Advanced turn-based combat with procedural enemy scaling and strategic dungeon crawling.

## 💻 Tech Stack
- **Language:** C# / .NET 10.0
- **Serialization:** System.Text.Json
- **Architecture:** Interface-based State Pattern, LINQ, Polymorphic Mapping.


<img width="931" height="587" alt="resim" src="https://github.com/user-attachments/assets/165e42d2-8839-4586-9999-de1cbd9ff7e2" />

---
*Developed by [Yilmaz Batal](https://yilmazbatal.com)* *Last updated: March 3, 2026*

