# Text-Based RPG Engine

A C# console-based RPG designed with a focus on **Object-Oriented Programming (OOP) principles**, modular architecture, and scalable game logic. This project serves as a robust foundation for text-based adventure systems.

## 🛠 Features & Architecture

The engine is built on professional software engineering patterns to ensure maintainability and clean code:

* **State Pattern:** Implements a decoupled menu and game-flow navigation system using interfaces.
* **Dependency Injection:** Uses a centralized `GameContext` to manage and share data across different services seamlessly.
* **Polymorphism & Abstraction:** Features a rich class hierarchy for Heroes, Items, and Equipment, utilizing abstract classes and inheritance.
* **Data Persistence:** Robust JSON serialization/deserialization logic for game state management and progress saving.
* **Combat Math:** Turn-based mechanics driven by formulaic armor mitigation and critical hit calculations.

## 🚀 Development Roadmap

The project is being developed in iterative phases. Below are the current progress and upcoming milestones:

### Core Foundation
- [x] State-based Navigation System
- [x] Hero Abstraction (Warrior, Archer, Mage)
- [x] JSON Serialization Engine (Save/Load)
- [x] Scalable Item/Equipment Hierarchy

### 🔜 Near Future (Ongoing Development)
- [ ] **Inventory & Equipment Management**
    * Implementation of **pagination** for large-scale inventory navigation.
    * Dynamic **Equipping/Unequipping** system with real-time stat recalculation.
- [ ] **Advanced Combat & Enemy System**
    * Dynamic enemy scaling and turn-based combat mechanics.
    * **Loot & Rewards:** Procedural rewarding for gold, items, and experience (EXP).
- [ ] **Character Progression**
    * **Training System:** Spending accumulated stat points to influence hero attributes.
- [ ] **Economy System (Blacksmith)**
    * Integrated shop system for buying and selling items.
    * **Equipment Refinement:** A scaling upgrade system (+1, +2, etc.) for weapons and armor.

## 💻 Tech Stack
- **Language:** C# / .NET 10.0
- **Data:** System.Text.Json
- **Logic:** LINQ, Polymorphic Mapping

Last updated (January 28 2026)
---
*Developed by [Yilmaz Batal](https://yilmazbatal.com)*
