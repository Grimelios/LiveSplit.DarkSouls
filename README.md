# LiveSplit.DarkSouls
This project is a configurable autosplitter for Dark Souls: Prepare To Die edition on PC. It does *not* work with the Remastered version. The autosplitter is designed to be usable for any Dark Souls speedrun, with a variety of split options available. In addition, the autosplitter can optionally serve as a full replacement for the existing IGT tool commonly used by the community to accurately display in-game time.

Created by [@Grimelios](https://twitter.com/Grimelios). Feel free to ping me on Twitter or Discord (Grimelios#2045) with any questions, concerns, or feedback. Big shoutout to the work done by existing Souls community members in tracking down memory locations, many of which I used in this project.

## Installation
The autosplitter is a LiveSplit component (a DLL file). To install, download the latest release from the [Releases](https://github.com/Grimelios/LiveSplit.DarkSouls/releases) section and place the DLL in your LiveSplit/Components folder (you should see a bunch of DLLs in there already). From there, simply launch LiveSplit, edit your layout, and add the autosplitter (under the Control category).

## Configuration
Once you've added the autosplitter to your layout, double-click on the autosplitter to open its configuration tab. The tool was designed to be as intuitive as possible, but I'll run through the options anyway, starting from the top.

![Checkboxes image](https://i.imgur.com/QgdEwHH.png)

The first two checkboxes here re-implement behavior from the existing IGT tool. "Use game time" causes the LiveSplit timer to be updated based on the game's internal IGT counter (meaning the timer will pause automatically on quitouts). Make sure your LiveSplit timer is set to display Game Time (rather than Real Time) for this to work. "Reset equipment indexes" does exactly what it describes, by resetting equipment index values to their default state when the LiveSplit timer resets. Without enabling this, your menuing can change unexpectedly on consecutive runs.

The third checkbox allows the timer to start automatically when you begin a new game. Specifically, the timer autostarts after the opening cinematic (as the camera begins to crawl through the asylum).

![Buttons image](https://i.imgur.com/z3fHE9T.png)

The next two buttons are pretty self-explanatory. "Add Split" appends a new, empty split to the bottom of your current list. "Clear Splits" empties the list following a confirmation prompt (note that the prompt doesn't appear when there's only one split in total). As you add and remove splits, the split count is updated and displayed beside these buttons. There's also a second count of unfinished splits (i.e. splits that require additional values to be set in order to allow the autosplitter to properly function).

## Splits
Next up is the splits themselves. There are currently seven split types available. Each line is split into "Type" (a single dropdown) and "Details" (a collection of controls that update based on the chosen split type). As these controls change, they're tinted with a red background when the control is unfinished and requires attention. If you save your layout with unfinished splits, everything will save correctly, but all unfinished splits are treated as manual during an actual run (so if you notice that an expected autosplit fails to trigger, check this first).

You can delete individual splits by clicking the red X on the right side of the split. Splits can be re-ordered by clicking and dragging on the left side.

### Bonfire splits

![Bonfire split image](https://i.imgur.com/h8sdXC1.png)

Select the target bonfire, then select split criteria. "On rest" is self-explanatory. "On leave" is the opposite (split when you leave the target bonfire). Bonfire states (lit or kindled to various levels) split when the target bonfire *reaches* that state (rather than already *being* in that state). Finally, "On warp" splits when you warp *from* the target bonfire (not *to* it).

### Boss splits

![Boss split image](https://i.imgur.com/SMFfgk3.png)

Select the target boss, then select split criteria. "On victory message" splits when that message appears (not on final hit). "On warp" follows the usual warp rules (detailed below).

### Covenant splits

![Covenant split image](https://i.imgur.com/p1IhKj2.png)

Select the target covenant, then select split criteria. "On join" splits when your covenant is *changed* to the target one (rather than already *being* in that covenant). "Discovery" refers to viewing the yes/no menu box prompting the player to join. The "On warp" variations follow the usual warp rules (detailed below).

### Event splits

![Event split image](https://i.imgur.com/593ftaG.png)

Select the target event, then select split criteria (if enabled). The events chosen for this list are somewhat arbitrary, but are meant to represent major world events. The "first" bell is the one past the Gargoyles, while the "second" bell is past Quelaag. Selecting either enables the criteria box. "On ring" splits immediately following the bell cutscene, while "On warp" follows the usual warp rules (detailed below).

The remaining options are self-explanatory. The two endings simply split when the ending is activated.

### Flag splits

![Flag split image](https://i.imgur.com/nwN5Xob.png)

Enter the target flag (a numeric ID), then select split criteria. In the context of Dark Souls, a "flag" refers to a boolean (true or false) value representing the state of a specific in-game event. Although some flags are already accounted for in other split types (such as bosses), this split type is meant to be a catch-all for other, more obscure events not otherwise tracked by the autosplitter UI (such as advancing NPC quest lines or dispersing individual fog gates). See https://goo.gl/kyNCD6 for a list of available flags. There are likely more valid flags than that list, but it should be a good start.

As for criteria, "On trigger" splits the moment the target flag becomes true, while "On warp" follows the usual warp rules (detailed below). Also note that the ID you enter isn't validated (apart from verifying that you've entered *something*).

### Item splits

![Item split image](https://i.imgur.com/yMpaOvo.png)

Item splits are the most complex by far (since there are lots of items in Dark Souls). Start by selecting an item type (which refreshes the item list beside it), then select the target item. Based on the item selected, the two upgrade dropdowns ("Infusions" and "Reinforcement") become enabled or disabled, and are refreshed with values appropriate to that item. For example, a plain Longsword can receive any infusion, but the maximum reinforcement level is dependent on *which* infusion you choose. Some items (like armor) can be reinforced, but not infused (which results in the Infusions list changing to "Locked" instead). Some items (like the Crystal Straight Sword) can't be infused *or* reinforced. Infusions must match exactly in order for an autosplit to occur, but reinforcements function on an *at least* basis (e.g. acquiring a +6 Divine Battle Axe would also satisfy a +5 split).

Note that the Estus Flask (under Consumables) can also be reinforced.

The small textbox to the right of the Reinforcement list is item count. Count defaults to one, but can be set to any value up to 999 inclusive. Similar to reinforcement, item count is considered satisfied if you've acquired *at least* that number of the target item (rather than *exactly* that amount).

Selecting "On acquisition" from the criteria box splits when you acquire the target item(s) (assuming the infusion, reinforcement, and count have also been met). "On warp" follows the usual warp rules (detailed below), but with the added restriction that the target item(s) must *still* be in your inventory when you warp.

Finally, a note about the Bottomless Box: from the perspective of the autosplitter, the Bottomless Box is treated exactly the same as the main inventory. In other words, having an item in storage will trigger splits as if you had picked up the item normally. The primary reason for this design is to accomodate item duplication through quantity storage.

### Manual splits

![Manual split image](https://i.imgur.com/6E83jI1.png)

No settings to configure here. This split is manual. Useful for cases when you've added splits that can't be triggered automatically by the autosplitter.

### Zone splits

![Zone split image](https://i.imgur.com/UVI8x3R.png)

Select the target zone. That's it. In this context, "zone" refers to a named area of the world that acts as an entrance or exit between larger, interconnected chunks of the world. For example, the player is dropped into Firelink Shrine when you exit the Undead Asylum, but these areas aren't physically connected. Similarly, touching the painting in Anor Londo warps the player to the Painted World (another zone physically disconnected from the rest of Lordran).

This split type, then, can be useful for detecting those transitions (which would otherwise be difficult to track). Note that zone splits only occur when the player moves *between* physically disconnected areas (rather than simply being *within* the target zone already). For example, an "Anor Londo" split won't trigger when running backwards from the Duke's Archives. It will *only* occur when landing in Anor Londo via a cutscene warp (i.e. being carried by gargoyles from the top of Sen's Fortress or falling out of the Painted World). The split is triggered following the cutscene.

### Warping

Almost done. The last note is about warping. Apart from bonfires, all "On warp" splits behave the same way (by triggering the split after a player has warped away from an objective, rather than when the objective itself is completed). The following types of warps are accounted for:

+ Using a homeward bone or the darksign
+ Warping from a bonfire
+ Dying (assuming you didn't perform a successful fall control quitout)

In all cases, the actual split occurs **when the loading screen appears** (following the fadeout to black). This behavior is intended to mimic the way many current Dark Souls runners split manually. The goal in implementing warp splits in this way is that, if you choose to adopt this autosplitter, you shouldn't have to remake your splits (since the autosplitter will be quite accurate to what you were already doing).

## Good luck!
That's it! My contact information is above (both Twitter and Discord) if you have any questions or concerns. I've tested extensively myself, but of course feel free to reach out if you encounter any bugs or missing features. Otherwise, enjoy your runs and good luck! :)
