VR-Object-Manipulation
======================
Manipulating objects in VR can be difficult - you can't see your keyboard or mouse, which means you want to minimize the amount of hand movements the player needs to make while maximizing their control over an object's motion, orientation, and size.  Hand-trackers like the leap motion controller have huge potential in making our interactions with virtual objects more intuitive, but their current device is not precise enough to give you maximum control over the object you are manipulating. For these reasons, I felt that an Xbox controller would work best for 3D object manipulation in VR, and built a small unity app to demonstrate its capabilities.

In this demo, you are choosing how to arrange furniture in some empty floor space. Companies like Ikea might be interested in letting their customers try out different pieces of furniture in a virtual room of the same dimensions as their own. There are two ways to select a piece of furniture - either browse through the large room and "grab" one of them by pressing in the right analog stick OR press start to navigate a menu of all possible furniture items, separated by category.

There are three distinct input modes you will use to arrange furniture and explore the space. In the Player Mode, the controller is set up to allow you to move through the virtual world the same way you would in any other first-person virtual reality experience. In the Menu Mode, X and B will let you cycle through objects in a given category while LB and RB allow you to change categories. Press A to select an item to bring into your floorspace or press Start to return to Player Mode.

The last mode (and the one that directly relates to the prompt I was given for this project), is the Object Manipulation Mode. The object manipulation mode gives you full control over a gameObject's transform. You can simultaneously translate, rotate, and scale your selected object. Whatever changes you make to the scene in the object manipulation mode will persist across multiple runs of the app! This will allow you to create an increasingly interesting floor plan as you start and stop the application while looking through the code.


Getting Started
===============
Once you open the project in Unity, find the FurnitureManipulation scene in the Scenes folder. Plug in your OculusRift and Xbox360 controller and press play.


Input Modes
===========
Player mode:
![player mode controls](https://github.com/johnshaughnessy/VR-Object-Manipulation/blob/master/player_mode_360.png)

Menu mode:
![menu mode controls](https://github.com/johnshaughnessy/VR-Object-Manipulation/blob/master/menu_mode_360.png)

Object mode:
![object mode controls](https://github.com/johnshaughnessy/VR-Object-Manipulation/blob/master/object_mode_360.png)


Code Overview
=============
The GameController object is the most significant object in the app. Open the GameController script to see how button presses are fed to the appropriate classes depending on which input mode is active. (Note: Given more time, I would separate collecting button presses in a different class. That way, the game controller wouldn't have to care where its input comes from, and I could more easily support mouse/keyboard input or leap motion input without having to change much. Since this wasn't a goal for this project, I collected button presses in the GameController.)

The ObjectMovementController interfaces with the GameObject's transform to produce smooth movement, rotation, and scaling. Both movement and rotation are computed relative to the direction you face, so that (for instance) pushing up on the left joystick will always mean pushing an object forward from your perspective.

The ObjectMenu script provides the logic behind the Menu. The Menu GameObject is set up in the scene so that a camera captures objects being created and displays them to the user via a Render Texture. See the "OVRPlayerController/Menu Display" item to for the placement of the menu (this is not unlike the placement of altspace's "personal browser"). (Note: I rushed to get this class written so that I could finish this README and the rest of the documentation before my time was up. It relies too heavily on the GameObject hierarchy AND file layout (specifically, the "Furniture" gameObject must match the "Resources/Prefabs" asset folder). This was the most immediate way I could think to get this accomplished.)

Back in the GameController object, the SceneSerializer is responsible for persisting your data through multiple runs of the application. An important Debug message should be logged to the console each time you run the app from the editor. This message explains how to delete your changes should you want to start over or should you encounter a bug. Serialization is not as straight forward for Unity as I'd like - for instance, you cannot simply mark a gameObject as serializable and call it a day. In order to save and load gameObject states, I serialized the transform components as well as the name, parent, and unique instanceID (to prevent duplicates and errors while loading) individually.

I would be happy to answer any questions about the layout of my code or why I certain design decisions. I also invite criticism as I'm always looking for ways to improve my skills.


Shortcomings / Where to go from here
====================================

I hope that my demo convinces the player that intuitive object manipulation in VR is absolutely possible with existing tools and techniques. The controls of this demo are not quite as intuitive as I'd like, however. Specifically, I don't like that going from mode to mode requires different button presses depending on the mode you're in. Expecting a player to understand 3 different input modes is difficult enough (I learned to use vim over the last 4 months and learned that input modes are CHALLENGING to pick up but POWERFUL when mastered).

There are almost none of the key gameplay elements that can transform a small tech demo into a compelling experience. The player should have visual queues to help her identify when she is manipulating an object, hovering over one with her "face cursor", and choosing between different menu options. She should also be given auditory feedback when picking up or putting down an object, since it's not always clear when the raycast has hit the object you are looking at. Auditory feedback would help bring to life tranlations, rotations, and scale operations. More attention to lighting, the environment, and a million other things would help enhance the core experience.

The reason I didn't focus or spend time on the items above are not because I don't think they are important. (In fact, adding the right "JUICE" is like a force multiplier - it can make your app 10 to 100x as enjoyable and popular.) However, perfecting these things takes a lot of time and repitition, and it is easy to spend HOURS making small tweaks trying to get them just right. Instead, I focused on the more technical aspects of the project to better show my capabilities.

In the menu mode image, there is a note that the DPad was going to be used to change the appearance and behaviour of gameObjects. This, I think, is a powerful idea in terms of what we will manipulate in virtual spaces. In real life, we can place a cube anywhere in our living room, but we can't tell the cube to spin or flash different colors without significant effort. In VR, this is easy. Manipulating 3D objects isn't just about placing, throwing, launching, dissecting, and linking them (or any other "classic" manipulations), it's about merging those things with intelligent code that knows how to operate on an arbitrary (or nearly arbitrary) gameObject.

There are honestly so many things to explore and so many ways to improve what I've done that it's hard to turn in. I hope I have the opportunity to tackle some of these challenges together with your team at altspace. Thank you for the opportunity to participate!

Other Libaries/Assets Included
==============================

Big Furniture Pack v1.21 by Vertex Studios. (After I had my code work properly with a cube I wanted to use assets that would suggest a real-world use case such as Ikea selling furniture. Unfortunately this also meant spending an hour organizing the hierarchy (because the demo scene was an absolute mess) and manually adding/adjusting box colliders on every single object).

Oculus Plugin (It's hard to tell what will feel nice in VR without trying it in VR.)

"Beut Tree" from Korea native trees vfirst by Yongho Kim. (Honestly this doesn't have to be in here. It's just my favorite asset and I was trying to find an excuse to use it.)


