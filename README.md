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

# Resources

https://opengameart.org/ is a great place to find simple 2D graphics. Most of what you find require attribution, but some don't.

https://unity.com/how-to/beginner/game-development-terms Good description of various terms - most of which you probably know. But a good glossery if you can't think of a specific term.

* https://opengameart.org/content/zelda-like-tilesets-and-sprites
* https://opengameart.org/content/48-animated-old-school-rpg-characters-16x16
* https://opengameart.org/content/tiny-characters-set
* 