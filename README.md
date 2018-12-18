# BeFit | fitNessMod | Beat Saber v0.12.1+ supported
An IPA Plugin for BeatSaber to add a calorie counter/ tracker

# Version 0.2.2
## Current Features
-  [x] Count Song Calories    - Displays calories adding up as you play beat saber songs
-  [x] Count Session Calories - Everytime you open beat saber, a new session starts from 0
-  [x] Count Daily Calories   - Based on the current date. *Known Bug with playing during date changes*
-  [x] Count All Calories     - Counts calories since mod has been installed. *Will most likely be removed, or by default hidden from main scren at later date.*
-  [x] Last Song Played       - Shows calories from last song played.

- [x] Calories are now based on METS by weight (there is a submenu called Fitness Properties to enter your weight in Kilo Grams or Pounds)
         - [x] Default weight is 132lbs or 60kg. Lower weight =  < calories burned over time.
- [x] Hide/Show certain elements (BeFit Mod Submenu)
- [x] Use legacy calorie counter or not (This won't be in next version)
- [x] Adjust the accuracy of the counter (Higher Numbers can prevent frame rate drops if any occur, default is a high number. Difference in calories is +/-1 )
- [x] Vertical and Horizontal body movements now accounted for (still working on accuracy however)
- [x] Calorie counts are much lower than initial release as the algorithm used is much more accurate and more like an algorithm
**Calorie counts may be *higher* or *lower* than what the users actually using.**

*All calorie counting software/ devices are estimates and do not accurately represents calories burned!*


## Future Improvments
Things to add:
* [x] Calories based on vertical and horizontal movments of headset
* [x] A seperate menu composed of:
  * [x] Toggle individual labels visible on menu screen
  * [ ] Setting daily Calorie goals
  * [ ] Setting Weekly Calorie Goals
  * [ ] Implement weightloss playlists
  * [ ] View Calorie Statistics
  * [ ] Include a pounds to calories value, 3500 calories ~= 1 lbs
* [ ] List calorie estimates for songs when selected. *Will be an option to toggle off in the sub menu*

## Important!
There is no way that this mod is accurate! However, I have been trying to set the amount of calories calculated to be as accurate as possible. Using your actual weight will make the value more accurate.



## Install
Make sure your game is patched with IPA and CustumUI is installed, [Beat Saber Mod Manager](https://github.com/Umbranoxio/BeatSaberModInstaller/releases) will do this for you. Then,
1.  Download the [BeFit Mod latest release here](https://github.com/viscoci/BeFit/releases)
2.  Copy the fitNessMod.dll file to the Beat Saber Plugins folder
3.  Go to settings and scroll to the BeFit Mod Submenu
4. Enter your weight and toggle what displays you would like to see in the menu, then hit Ok or Apply
5. That's it! Start the game and beat some calories to death! (I am aware that's not how that works)


<img src="https://visco.city/external/images/beFitv020.JPG" width="600" alt="Menu Layout"/>

## How it works
Users headset, and hand controller velocities are acquired and relative. A METS value is then determined based on the velocity and sent to the calculator to add a tenth of a seconds worth of imformation to be added into the total and be displayed.

Take a look at the source code. It is fairly simple, and it won't take long to understand.

### How do the calories get calculated?
METS values. [VR Health Institute](https://vrhealth.institute/portfolio/beat-saber/) found beat saber to have a physical activity equivalent of playing tennis (6 - 8 METS).
<a href="https://vrhealth.institute/methodology/"><img src="https://vrhealth.institute/wp-content/uploads/2017/08/Tennis-Pre-300-dpi.png" align="left" width="78" ></a>
The METS value combined with the users weight can give close representation of the calories being burned. In order to improve accuracy, I have been [looking into using age, height, and sex](https://sites.google.com/site/compendiumofphysicalactivities/corrected-mets) in order to find more accurate calories per user. 

Currently, calories are calculated using:
~~~
METS * userWEight * runTimeinHours
~~~
METS values are currently determined by the velocity at 5 different intensities. I'm hoping to find a ratio to replace this.


### Feedback
Find me on the [Beat Saber Modding Community Discord](https://discordapp.com/invite/beatsabermods) or DM me directly, @Viscoci
  

