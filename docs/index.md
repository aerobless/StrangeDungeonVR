# Strange Dungeon VR

Discover an endless dungeon full of monsters, treasure and secrets. Battle through enemies while collecting power-ups and gold. Strange Dungeon is a rogue-lite experience with a randomised, endless level.

## Changelog

### 0.4 (next version)
* ...
* New tile: Goblin Cave
* New trap: Falling stepping stone. This trap triggers when the player stands on a stone for to long, causing them to fall to their death.
* Jail cell tile got some destructible torture instrument variants.

### 0.3 (current live version)
* Implemented leaderboard. Your best score is now recorded when you end a dungeon run.
* Improved analytics. Implemented custom unity event that tracks users progress during a dungeon run.
* Health is now displayed in a small HUD on the "stump" of the players left hand.. we'll have to see if that works or if players find it too unsetteling :P
* Added collision information to most of the environment. Walls, floors and various objects now have impact sounds.
* Wood table and chairs are now destructible.
* Added system for edible items. Health potions are now in the game.
* Added logic for dynamic lights that emulates a flickering flame. First use is a interactable candle stand.
* The prison tile now has two additional doors & changing prison cell interiors.
* Added persistence information to player items. Valueable loot will now no longer disappear so quickly.
* Added mini skybox to starting tile. Removed the main skybox and set camera background to black. This improves performance and reduces visbility of geometry placement errors.
* Added lockable doors. The doors in the prison room can now be opened with a key.
* Added 5 types of chests that can contain loot bundle rewards. Chests types are equally, randomly spawned for now.
* Added logic for chests that can be opened with a key to find the sweet loot inside.
* Enemies now have footstep sounds. This makes them less likely to sneak up on you.
* Added soul shard that increases the players base sword damage by 5 permanently.
* Added soul shard that increases player HP by 10 permanently;
* Modularized enemy agent AI. 
* Added logic to register interactive ui panels with game manager. This fixes the bug where the settings menu wouldn't work after dieing the first time.
* Fixed bug where passing from the first tile back to start would lock off the dungeon
* New trap type: Blade wheel that accelerates towards the player when triggered

### 0.2
* Fixed various nasty bugs :D
* Added statistics panel to graveyard. Now you can see how many enemies you've killed, how many coins you collected and so on
* Loot is now managed via a central LootManager. There's currently a 25% chance for a soul shard to spawn from a small loot bundle.
* Soul Shards now vibrate when in range for activation.
* Added consumable system. Consumables can now be added to the game. The first consumable is the soul shard/effect.
* Added first iteration of effects system. When a player discovers a effect they can choose to activate it by pulling it towards them.
* Added VariabilityManager to store difficulty variables in a central location. Effects affect the variables in the VariabilityManager.
* Added ItemInfo UI panel. Items can now have a UI tooltip that shows up when hovering over and item or gripping it in your hands.
* Enabled holsters. Sword and torch can now be holstered.
* Added usable torch with point light. Although the sword/flashlight idea looked interesting it was a pain to fight in close quarters when not properly seeing the enemies.
* Humanoid agents now have a wandering capability when they're not tracking a player. This makes them appear more life-like from the distance.
* Further improved navmeshes for all tiles. Passing spike traps has now a higher cost so AI will try to avoid them.
* New lighting system. The players main weapon now provides a spotlight capability. The main directional light was removed. This makes the dungeon more scary and dark.
* Added ambient sound logic to offset the sound starting point, making e.g. multiple torches sound less uniform
* Improved HVR teleport/dash ability to properly indicate the teleport destination
* Updated Unity to latest version, includes various performance fixes for XR

### 0.1.1
* Integrated unity analytics platform to track player metrics
* Restricted navmesh creation for walkable/colliding game objects. This improves AI navigation and performance.

### 0.1
* Initial alpha release on AppLab (for invited testers only)
* Announcement board with play instructions
* Player can die after taking too much damage
* Player can respawn on the graveyard of a new starting tile
* Player can kill skeletons
* Player can destroy bookshelfs and vases to obtain coins
* Logic to randomize game objects. E.g. vases spawn as one of 8 vase varients.
* Logic to randomize props. The reilings in the big hall can change their appearance.
* Dungeon generates random tiles when player progresses through dungeon
* Dungeon randomizes tiles with predefined decorations
* Dungeon tracks created tiles and deletes them when no longer needed

## Privacy Policy

Sixty Meters built the Strange Dungeon VR app as a Commercial app. This SERVICE is provided by Sixty Meters and is intended for use as is.

This page is used to inform visitors regarding our policies with the collection, use, and disclosure of Personal Information if anyone decided to use our Service.

If you choose to use our Service, then you agree to the collection and use of information in relation to this policy. The Personal Information that we collect is used for providing and improving the Service. We will not use or share your information with anyone except as described in this Privacy Policy.

The terms used in this Privacy Policy have the same meanings as in our Terms and Conditions, which are accessible at Strange Dungeon VR unless otherwise defined in this Privacy Policy.

**Information Collection and Use**

For a better experience, while using our Service, we may require you to provide us with certain personally identifiable information. The information that we request will be retained by us and used as described in this privacy policy.

The app does use third-party services that may collect information used to identify you.

Link to the privacy policy of third-party service providers used by the app

*   [Oculus](https://www.oculus.com/legal/privacy-policy-for-oculus-account-users/)
*   [Unity](https://unity3d.com/legal/privacy-policy)

**Log Data**

We want to inform you that whenever you use our Service, in a case of an error in the app we collect data and information (through third-party products) on your phone called Log Data. This Log Data may include information such as your device Internet Protocol (“IP”) address, device name, operating system version, the configuration of the app when utilizing our Service, the time and date of your use of the Service, and other statistics.

**Cookies**

Cookies are files with a small amount of data that are commonly used as anonymous unique identifiers. These are sent to your browser from the websites that you visit and are stored on your device's internal memory.

This Service does not use these “cookies” explicitly. However, the app may use third-party code and libraries that use “cookies” to collect information and improve their services. You have the option to either accept or refuse these cookies and know when a cookie is being sent to your device. If you choose to refuse our cookies, you may not be able to use some portions of this Service.

**Service Providers**

We may employ third-party companies and individuals due to the following reasons:

*   To facilitate our Service;
*   To provide the Service on our behalf;
*   To perform Service-related services; or
*   To assist us in analyzing how our Service is used.

We want to inform users of this Service that these third parties have access to their Personal Information. The reason is to perform the tasks assigned to them on our behalf. However, they are obligated not to disclose or use the information for any other purpose.

**Security**

We value your trust in providing us your Personal Information, thus we are striving to use commercially acceptable means of protecting it. But remember that no method of transmission over the internet, or method of electronic storage is 100% secure and reliable, and we cannot guarantee its absolute security.

**Links to Other Sites**

This Service may contain links to other sites. If you click on a third-party link, you will be directed to that site. Note that these external sites are not operated by us. Therefore, we strongly advise you to review the Privacy Policy of these websites. We have no control over and assume no responsibility for the content, privacy policies, or practices of any third-party sites or services.

**Children’s Privacy**

These Services do not address anyone under the age of 13. We do not knowingly collect personally identifiable information from children under 13 years of age. In the case we discover that a child under 13 has provided us with personal information, we immediately delete this from our servers. If you are a parent or guardian and you are aware that your child has provided us with personal information, please contact us so that we will be able to do the necessary actions.

**Changes to This Privacy Policy**

We may update our Privacy Policy from time to time. Thus, you are advised to review this page periodically for any changes. We will notify you of any changes by posting the new Privacy Policy on this page.

This policy is effective as of 2022-02-26

## Support

Strange Dungeon VR is currently released as is. There is no dedicated support channel yet, although there may be in the future. But feel free to contact me on twitter @eletiy or via issues on the GitHub project: https://github.com/aerobless/StrangeDungeonVR
