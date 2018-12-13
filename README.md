# BeFit | fitNessMod | Beat Saber v0.12.x+ supported
An IPA Plugin for BeatSaber to add a calorie counter/ tracker

## BeFit v0.2.0 coming soon! 
v0.1.x will continue to be updated until completed

### What to expect with the new update?
**The plugin will ask for you to enter your:**
- [x] weight *choose between metric or lbs*
- [x] Remove Menu Calorie counts from displaying
Expect new calorie counts to be around half of version 0.1.x
The legacy counting algorithm will still be included and be able to be switch on in the new menu.



**Calorie counts may be *higher* or *lower* than what the users actually using.**

*All calorie counting software/ devices are estimates and do not accurately represents calories burned!*

## Current Features
1.  [x] Count Song Calories    - Displays calories adding up as you play beat saber songs
2.  [x] Count Session Calories - Everytime you open beat saber, a new session starts from 0
3.  [x] Count Daily Calories   - Based on the current date. *Known Bug with playing during date changes*
4.  [x] Count All Calories     - Counts calories since mod has been installed. *Will most likely be removed, or by default hidden from main scren at later date.*
5.  [x] Last Song Played       - Shows calories from last song played.

## Future Improvments
Things to add:
* [x] Calories based on vertical and horizontal movments of headset
* [ ] ~~Larger distance between blocks has higher coefficient.~~
* [x] A seperate menu composed of:
  * [x] Toggle individual labels visible on menu screen
  * [ ] Setting daily Calorie goals
  * [ ] Setting Weekly Calorie Goals
  * [ ] Implement weightloss playlists
  * [ ] View Calorie Statistics
  * [ ] Include a pounds to calories value, 3500 calories ~= 1 lbs
* [ ] List calorie estimates for songs when selected. *Will be an option to toggle off in the menu*

## Important!
There is no way that this mod is accurate! However, I have been trying to set the amount of calories calculated to be as accurate as possible. Using your actual weight will make the value more accurate.



## Install
Make sure your game is patched with IPA and CustumUI is installed, [Beat Saber Mod Manager](https://github.com/Umbranoxio/BeatSaberModInstaller/releases) will do this for you. Then,
1.  Download the [BeFit Mod latest release here](https://github.com/viscoci/BeFit/releases)
2.  Copy the fitNessMod.dll file to the Beat Saber Plugins folder
3.  Go to settings and scroll to the BeFit Mod Submenu
4. Enter your weight and toggle what displays you would like to see in the menu, then hit Ok or Apply
5. That's it! Start the game and beat some calories to death! (I am aware that's not how that works)


<img src="https://visco.city/external/images/bfit.PNG" width="600" alt="Menu Layout"/>

## How it works
Users headset, and hand controller velocities are acquired and relative. A METS value is then determined based on the velocity and sent to the calculator to add a tenth of a seconds worth of imformation to be added into the total and be displayed.

Take a look at the source code. It is fairly simple, and it won't take long to understand.

### How do the calories get calculated?
METS values. [VR Health Institute](https://vrhealth.institute/portfolio/beat-saber/) found beat saber to have a physical activity equivalent of playing tennis (6 - 8 METS).
<a href="https://vrhealth.institute/methodology/"><img src="https://vrhealth.institute/wp-content/uploads/2017/08/Tennis-Pre-300-dpi.png" align="left" width="78" ></a>
The METS value combined with the users weight can give close representation of the calories being burned. In order to improve accuracy, I have been [looking into using age, height, and sex](https://sites.google.com/site/compendiumofphysicalactivities/corrected-mets) in order to find more accurate calories per user. 

Currently, calories are calculated using:
~~~
((METS * 3.5 * userWEight)/200) * 0.1
~~~
METS values are currently determined by the velocity at 5 different intensities. I'm hoping to find a ratio to replace this.


### Feedback
Find me on the [Beat Saber Modding Community Discord](https://discordapp.com/invite/beatsabermods) or DM me directly, @Viscoci
  

