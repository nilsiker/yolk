# Yolk

[![License](https://img.shields.io/badge/license-MIT%2FApache--2.0-informational)](COPYRIGHT.md)

> ⚠️ Yolk is under heavy development and is not currently intended for public use.
>
> With that said, feel free to try it out!
>
> Do not hesitate to reach out, if you have any feedback or ideas!

An opinionated game template for Godot C# using [Chickensoft tools](https://chickensoft.games/), with a focus on ease-of-understanding.

<p align="center">
  <img  alt="a pixelated, cracked egg" src="docs/media/logo.png" width="160" />
</p>

The template provides a set of modules that can help accelerate your next game project in Godot.

## 📦 Modules

> 🙋🏼‍♂️ Below you'll find a quick overview of what Yolk provides. There is no detailed description just yet, but I'm planning to fully describe the modules, their contents, and examples on how they can be used!

Yolk is composed of several modules, each providing a specific set of functionality. Each module is implemented in a separate .NET project and is designed to be reusable and composable. This allows you to pick and choose the features you need for your game!

### 🧠 Logic

The `Yolk.Logic` module acts as the brain for your game and includes a set of classes and interfaces that make up the foundational building blocks of your game.

It provides an opinionated way to manage:

- App lifecycle
- Game state
- Main menu
- Pause menu
- and more!

### 💾 Saving & Loading


The module `Yolk.Data` provides a generic `YolkSave`, currently implemented to support:

- Multiple named saves
- One (1) global autosave
- One (1) in-memory quicksave slot.

It uses save chunks from `Chickensoft.SaveFileBuilder` to store save data of your choice, in a generic and composable way.

This module only provides the data structures for saving and loading. It does not assume what state your game needs to store or how to grab a hold of it.

Neither does it handle the actual reading and writing of files. For that, you can use the `Yolk.FS` module.

### 📁 File System

The `Yolk.FS` module provides way to read from and write to the file system, using the standard Godot user directory `user://`.

- Read and write save files - `GodotSaver`
  - Save data is handled in a generic way, allowing you to save any data type.
  - Currently serializes to JSON.
- Read and write settings files - `GodotConfig`
  - Such as volume, display, and graphics settings.

### 🔮 Planned

And more to come!

## 🛠️ Built with

Yolk is built using .NET 8 and heavily relies on [Chickensoft tools](https://chickensoft.games/), such as `Chickensoft.LogicBlocks` and `Chickensoft.SaveFileBuilder`.

The showcase game is built using Godot 4.4.

## Attributions

To showcase the template, I'm using assets by the amazing [Kenney](https://kenney.nl). specifically:

- [1-Bit Platformer Pack](https://kenney.nl/assets/1-bit-platformer-pack)
- [1-Bit Input Prompts Pixel 16×](https://kenney.nl/assets/1-bit-input-prompts-pixel-16)

Give their website a look, their assets are great! **And free - consider donating!**

## License

_Yolk_ is dual-licensed under the MIT license and the Apache 2.0 license.
