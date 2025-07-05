# Architecture Overview

Yolk is a game structured around my interpretation on how to best use the Chickensoft architecture. It all starts at the top level with the `App`.

This game template includes core mechanics that I typically use, along with some example implementations on nodes and logic that can be used in tandem with these core mechanics.

## App

The App is responsible for:

* Acting as a game container
* Shutting down the app
* Showing debug info and obscuring/revealing the screen

Typically, the App has few children, in this case the following nodes:

* [Options](#options)
* [Game](#game)
* AppUI
  * Blackout - for obscuring and revealing the screen
  * DebugPanel - showing state info for all `IStateInfo` nodes added to the `state` group

## Game

The node representing the game. It provides game-wide utilities to:

* Starting a new game
* Loading a saved game
* Deleting a saved game
* Losing a game
* Winning a game
* Quitting the game

Typically, the game has the following children:

* Game UI
  * Screen Effects - viewport shaders covering the game viewport (excluding UI).
  * [Main Menu](#main-menu)
  * [Pause Menu](#pause-menu)
* [Repos](#repos) - amount and sorts depending on your specific game implementation
* [World](#world)
* [SoundSpawner](#SoundSpawner)

### Main Menu

The main menu for the game.

It provides the user easy access to starting, loading, and deleting a game.

It also provides easy access to game options and quitting the application to desktop.

## Options

Options that can be configured by the player.

Core settings include:

* Global pixelation and dithering
* Audio levels
* Input mapping

## World

This node represents the physical game world. 

World reacts to events in the Game, such as starting and loading a game.

It provides utilities to:

* Loading a level (to support streaming)
* Unloading a level (to support streaming)
* Fully transitioning to a new level, clearing other currently loaded levels.

The World typically has the children:

* [Player](#player)

## Player - example



## UI
