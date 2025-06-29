# Architecture Overview

Yolk is a game structured around my interpretation on how to best use the Chickensoft architecture. It all starts at the top level with the `App`.

# App

The App is responsible for:

* Acting as a game container
* Shutting down the app

Typically, the App has few direct children, in this case the following nodes:

* Options, such as
  * Input mapping
  * Audio levels
  * Global screen effects

* Game - the game being played
  * Main Menu, providing ways to:
  * Start a new game
  * Load a saved game
  * Delete a saved game
  * Accessing the option interface
  * Showing game credits and acknowledgments
