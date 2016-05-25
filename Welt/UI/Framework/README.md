Welt UI Framework V2
--------------------
This is the second take on a UI Framework. Originally, all UI was nested within the scene. Instead of
this, we should create a UI manager that detects scenes or key events and renders UI based on this. 
Using a singleton UI system renderer would allow us to create and update new UI components and 
allow for most components to be linked and aware of each other. This can be useful for things like:
- TextInputComponents updating each other 
- Live update processing 
- Better layout system that could grid-set each component rather than runtime adjustments.
- Could perhaps allow for an easier AI? Idk. 

Things to work on:
==================
- A dedicated UI file loading system (by using either JSON, XAML, or HTML).
- Scrollbars because those are hard as fuck to think about.