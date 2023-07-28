# What is this?

This project is something I am building to learn some basic concepts of game design.

It started as me wanting to learn concepts of procedural terrain generation. After doing that for a while in a console app I decided it would be easier to visualize with graphics instead of characters representing the biomes.

# Areas of Focus

This section will contain details about different areas of focus.

## Terrain

Terrain currently consists of 3 variables.

1. Height
2. Moisture
3. Heat

These 3 variables are what make up the different biomes.

* Water
* Grassland
* Forest
* Desert
* Tundra
* Mountains

For example, low elevation becomes water. High elevation becomes mountain. Low moisture is desert, medium moisture is grassland and high moisture is forest. And there is obviously more overlap than that.

## Drawing

Need a struture for drawing any object. A drawable could be an image, test, menu, button, etc. This should be abstract so most methods don't need to know what they are drawing, just that they are drawing something.

## Tiles

The map will consist of drawable tiles. Each tile is drawn at 64x64 pixels, though sourced smaller for a retro look.

## User Interface

While I don't ever plan to turn this into a real game that is released, I want to plan like that is what I will be doing. As such, I want to plan for supporting both mouse and touch interfaces. But, how to define the interface so the platform code can interface with the game engine UI in a clean manner?

One option is to just pass along both mouse and touch events to the game engine and let it decide. But that kind of ties the game engine into platform logic. In other words, the game engine shouldn't really have to worry about what a "mouse" is.

Another option is to abstract the concept of touch and mouse. For simple things like tapping or clicking on a button, that is pretty straight forward. Things get more complex when interacting with the map.

For example, a phone can't right click. This means you are limited in the type of interactions you can perform. Normally, a long press is converted into a right click. But if we abstract them too much then a long left-click might be considered a long press and bring up the context menu.

Left-clicking on the map might start the player walking to that location. But in abstract terms, a single touch might be the start of a pan operation.

Need to come up with a plan for some kind of input manager. Probably should look at existing code.

## Animation

Animation needs to be handled. Meaning, the character should animate between images to look like it is walking. But we need to be able to define a FPS for an animated sprite. Because the character while walking might animate at one speed and while running might animate at another speed.

Need a interface/provider that allows tracking animation progress for different sprites - at different speeds.

# Resources

https://opengameart.org/ is a great place to find simple 2D graphics. Most of what you find require attribution, but some don't.

https://unity.com/how-to/beginner/game-development-terms Good description of various terms - most of which you probably know. But a good glossery if you can't think of a specific term.

* https://opengameart.org/content/zelda-like-tilesets-and-sprites
* https://opengameart.org/content/48-animated-old-school-rpg-characters-16x16
* https://opengameart.org/content/tiny-characters-set
* 