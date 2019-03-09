# LiveSplit.DarkSouls
This project is a configurable autosplitter for Dark Souls: Prepare To Die edition on PC. It does *not* work with the Remastered version. The autosplitter is designed to be usable for any Dark Souls speedrun, with a variety of split options available. In addition, the autosplitter can optionally serve as a full replacement for the existing IGT tool commonly used by the community to accurately display in-game time.

## Installation
The autosplitter is a LiveSplit component (a DLL file). To install, download the latest release from the [Releases](https://github.com/Grimelios/LiveSplit.DarkSouls/releases) section and place the DLL in your LiveSplit/Components folder (you should see a bunch of DLLs in there already). From there, simply launch LiveSplit, edit layout, and add the autosplitter (under the Control category).

## Configuration

Once you've added the autosplitter to your layout, double-click on the autosplitter to open its configuration tab. The tool was designed to be as intuitive as possible, but I'll run through the options anyway, starting at the top.

![Checkboxes image](https://i.imgur.com/Gs5ofiL.png)

The two checkboxes at the top re-implement behavior from the existing IGT tool. "Use game time" causes the LiveSplit timer to be updated based on the game's internal IGT counter (meaning the timer will pause automatically on quitouts). Make sure your LiveSplit timer is set to display Game Time (rather than Real Time) for this to work. "Reset equipment indexes" does exactly what it describes, by resetting equipment index values to their default state when the LiveSplit timer resets. Without using this setting, your menuing can change unexpectedly on consecutive runs.

![Buttons image](https://i.imgur.com/z3fHE9T.png)

The next two buttons are pretty self-explanatory. "Add Split" appends a new, empty split to the end of your current list. "Clear Splits" empties the list following a confirmation prompt (note that the prompt doesn't appear when there's only one split total). As you add and remove splits, the split count is updated and displayed beside these buttons. There's also a second count of unfinished splits (i.e. splits that require additional values to be set).

Next up is the splits themselves. There are currently seven split types available. Each line is split into "Type" (a single dropdown) and "Details" (a collection of controls that update based on the chosen split type). As these controls change, they're tinted with a red background when the control is unfinished and requires attention. If you save your layout with unfinished splits, everything will save correctly, but all unfinished splits are treated as manual during an actual run (so if you notice that an expected autosplit fails to trigger, that's probably why).

### Bonfire splits

![Bonfire split image](https://i.imgur.com/h8sdXC1.png)

Select the target bonfire, then select split criteria. "On rest" is self-explanatory. Bonfire states (lit or kindled to various levels) split when the target bonfire *reaches* that state (rather than already *being* in that state). Finally, "On warp" splits when you warp *from* the target bonfire (not to it).

### Boss splits

![Boss split image](https://i.imgur.com/SMFfgk3.png)

Select the target boss, then select split criteria. "On victory message" splits when the message appears (not on final hit). "On warp" follows the usual warp rules (detailed below).

### Covenant splits

![Covenant split image](https://i.imgur.com/p1IhKj2.png)

Select the target covenant, then select split criteria. "On join" splits when your covenant is *changed* to the target one (rather than already being in that covenant). "Discovery" refers to viewing the yes/no menu prompting the player to join. The "On warp" variations follow the usual warp rules (detailed below).

### Event splits

![Event split image](https://i.imgur.com/593ftaG.png)

Select the target event, then select split criteria (if enabled). The events chosen for this list are arbitrary, but meant to represent major world events. The "first" bell is the one past the Gargoyles, while the "second" bell is past Quelaag. Selecting either enables the criteria box. "On ring" splits immediately following the bell cutscene, while "On warp" follows the usual warp rules (detailed below).

The two endings split when the ending is activated.

### Flag splits

![Flag split image](https://i.imgur.com/nwN5Xob.png)

Enter the target flag (a numeric ID), then select split criteria. In the context of Dark Souls, a "flag" refers to a boolean (true or false) value representing the state of a specific in-game event. Although some flags are already accounted for in other split types (such as bosses), this type is meant to be a catch-all for other, more obscure events not otherwise tracked by the autosplitter (such as advancing NPC quest lines or dispersing individual fog gates). See https://goo.gl/kyNCD6 for a list of available flags.

As for criteria, "On trigger" splits the moment the target flag becomes true, while "On warp" follows the usual warp rules (detailed below). Also note that the ID you enter isn't validated (apart from verifying that you've entered *something*).

### Item splits

![Item split image](https://i.imgur.com/yMpaOvo.png)

Item splits are the most complex by far (since there are lots of items in Dark Souls). Start by selecting an item type (which refreshes the item list beside it), then select the target item. Based on the item select, the two upgrade dropdowns ("Infusions" and "Reinforcement") become enabled or disabled, and are refreshed with values appropriate to that item. For example, a plain Longsword can receive any infusion, but the maximum reinforcement level is dependent on *which* infusion you choose. Some items (like armor) can be reinforced, but not infused (which results in the Infusions list changing to "Locked" instead). Some items (like the Crystal Straight Sword) can't be infused *or* reinforced. Infusions must match exactly, but reinforcements function on an *at least* basis (e.g. a +6 Divine Battle Axe would also satisfy a +5 split).

Note that the Estus Flask (under Consumables) can also be reinforced.

The small textbox to the right of the Reinforcement list is item count. Count defaults to one, but can be set to any value up to 999 inclusive. Similar to reinforcement, item count is considered satisfied if you've acquired *at least* that number of the target item (rather than *exactly* that amount).

Finally, selecting "On acquisition" from the criteria box splits when you acquire the target item(s) (assuming the infusion, reinforcement, and count have also been met). "On warp" follows the usual warp rules (detailed below), but with the added restriction that the target item(s) must *still* be in your inventory when you warp.

### Manual splits

![Manual split image](https://i.imgur.com/6E83jI1.png)

No settings to configure. This split is manual. Useful for cases when you've added splits that can't be triggered automatically by the autosplitter.

### Zone splits

![Zone split image](https://i.imgur.com/UVI8x3R.png)

Select the target zone (that's it). In this context, "zone" refers to a large-scale, interconnected area of the world (such as the Painted World or all of Lordran). This split type can be useful for detecting when you've entered or finished an entire area (such as completing the Undead Asylum by entering Firelink). The actual split occurs whenever the player is within the target zone (rather than strictly when you move *between* zones).
