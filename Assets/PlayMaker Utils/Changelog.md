#PlayMaker Utils Change log

###1.6.0
**Release Date:** 26/07/2019  

**New**: New method `PlayMakerEditorUtils.CreateGlobalEventsIfNeeded(...)` to create global asset, stored in PlayMaker Globals asset

###1.5.9
**Release Date:** 05/04/2019  

**Update**: switched from LGPL to MIT license

###1.5.8
**Release Date:** 17/01/2019  

**fixed**: Updated Enum Creator Wizard to work on Unity 2018

###1.5.7
**Release Date:** 18/12/2018  

**fixed**: Removed Obsolete EventType.mouseDown api usage


###1.5.6
**Release Date:** 18/09/2018  

**fixed**: Brought back `GetFsmOnGameObject` method to compliment `FindFsmOnGameObject` and return an Fsm instead of PlayMakerFSM object, this is to ease updates of old projects.

###1.5.5
**Release Date:** 30/08/2018  

**new**: `PlayMakerUtils.LogFullPathToAction(FsmStateAction)`  
**new**: `PlayMakerUtils.LogFullPathToAction.GetGameObjectPath(GameObject)`  


###1.5.4
**Release Date:** 20/07/2018  

**new**: VersionInfo class to help finding out addons versions and misc

###1.5.3
**Release Date:** 18/07/2018  

**fixed**: removed unwanted Editor reference that prevented builds

**fixed**: leveraged global event creation with better checks


###1.5.2
**Release Date:** 27/06/2018  

**new**: new `Current Event Data` for viewing live the current event data being used.

###1.5.1
**Release Date:** 11/06/2018  

**Improvement** `EventProxyWizard` now can accepts a single parameter passed as `EventData` to the playmaker event


###1.5
**Release Date:** 20/03/2018  

**new**: new `PlayMakerTimelineEventTarget` for creating convenient proxies for timeline

**new**: debug optional parameter for `PlayMakerEvent` `sendevent()` call

**new**: Editor Utils to copy string to clipboard `PlayMakerEditorUtils.CopyTextToClipboard()`

**Fix**: Fixed mounting and unmounting scripting symbols, properly based on PlaymakerDefines.cs. 

**Fix**: ProceduralMaterial support rule for 2017 and 2018

**new**: `PLAYMAKER_UTILS_1_5_OR_NEWER` symbol

###1.4.2
**Release Date:** 22/06/2017  

**new**: new `PlayMakerUtils_conversions` to convert non nullable objects like rect, quaternions, vectors, from an object pointer.  

###1.4.1
**Release Date:** 05/06/2017  

**new**: new `PlayMakerUtils.SendEventToTarget` to leverage sending events


###1.4
**Release Date:** 01/06/2017

**new**: PlayMakerUtilsDefine class to feature `PLAYMAKER_UTILS` and `PLAYMAKER_UTILS_1_4_OR_NEWER`  

**new**: MainCamerTarget Public Class with property drawer like the owner class for proxy component creation  

**Fix**: owner public class and property drawer to work as expected

**Fix**: `SetEventProperties.Properties` is now declared by default to always be there  

###1.3.2
**Release Date:** 07/03/2017

**Fix**: Windows Publishing with a better code to check if type is enum  

**new**: Improved `DoesGameObjectImplementsEvent()` to account for `SendToChildren` Option

###1.3.1
**Release Date:** 06/02/2017

**Fix**: `Required` Attribute definition now properly namespaced in `HutongGames.PlayMaker.Ecosystem.Utils`

**Fix**: Enum Creator wizard title  

###1.3
**Release Date:** 12/01/2017

**new**: Enum Creator Wizard  
**new**: Rotorz ReOrderable List Library included 

###1.2.9
**Release Date:** 27/09/2016  

**new**: Conditional Expression addition ( manual for now)  

**Fix**: Windows store publishing  

###1.2.8
**Release Date:** 21/09/2016  

**new**: RequiredAttribute  

**update**: LinkerWizard support for PlayMaker 1.8.3  

**Improvement:** LinkerWizard do not spam logs when published, debug all actions set to false by default 


###1.2.7
**Release Date:** 30/08/2016  

**new**: added `TransformEventsBridge.cs` to forward `OnTransformParentChanged()` and `OnTransformChildrenChanged()` as PlayMaker Events.

###1.2.6
**Release Date:** 17/08/2016  

**new**: added PlayMakerInspectorUtils.SetActionEditorVariableSelectionContext() to properly set context when using VariableEditor.FsmXXXField(). This is for PlayMaker 1.8+ only  

###1.2.5
**Release Date:** 22/06/2016  

**Fix**: PlayMaker 1.8 support

###1.2.4 beta
**Release Date:** 23/03/2016  

**Fix**: Prevent error when parsing a null object as string in ParseFsmVarToString()  

###1.2.3 beta
**Release Date:** 01/02/2016

**Fix**: Linker wizard support for Collider2D inheritance.

###1.2.2 beta
**Release Date:** 12/01/2016  

**Fix:** Unity 4.7 compatibility 

###1.2.1 beta
**Release Date:** 14/12/2015  

**Fix:** Unity 5.3 new SceneManagement obsolete calls  
**Fix:** PlayMaker 1.8f36 change in EventTarget setup for sending events programmatically

###1.2.0 beta
**Release Date:** 11/12/2015  

**New:** New Component public class with related propertyDrawers for `PlayMakerFsmVariableTarget` and `PlayMakerFsmVariable`  
**New:** New utils to list variables by string based on a FsmVariables reference   
**New:**  New Reflections Utils to get the BaseProperty of a SerializedProperty   

###1.1.6
**Release Date:** 19/09/2015  

**New:** New function to create global events  
**Fix:** Added support for PlayMakerEvent propertyDrawer to work within stateMachine context  
**Improvement:** Better description for auto generated "PlayMaker sent even proxy"  
  
###1.1.5
**Release Date:**  16/09/2015  

**New:** Event Proxy Creator wizard  

###1.1.4
**Release Date:**  10/09/2015  

**New:** Custom Sample for linker wizard  
**Improvement:** Moved LinkerWizard package into U4 github rep to centralize content  
**Improvement:** moved all menus into *Addons* section  


###1.1.3
**Release Date:**  26/08/2015  

**New:** LinkerData introspection system  
**New:** new PlayMakerUtils tools for Assets( creation and search)  
**New:** PlayMakerFsmTarget PropertyDrawer  
**New:** StateSynchronizer Component  


###1.1.2
**New:** Added automatic byte conversion case

**Fix:** removed logs  

###1.1.1
**Fix:** Fixed PlayMakeEvent Broadcasting call  
**Fix:** Fixed PlayMakerEvent PropertyDrawer when EventTarget is undefined  
**Fix:** Fixed Missing StringComparision enum for windows mobile


###1.1.0
**New:** Added versioning and change long following Ecosystem convention for future distribution  
**New:** Merge new public variables dedicated for PlayMaker integration in proxy Components
**New:** Explicit opensource licensing under [LGPL-3.0](http://opensource.org/licenses/LGPL-3.0) (see README.md)

**Fix:** Support for Unity 5 WebGL target
**Fix:** included generic attributes inside HutongGames.PlayMaker.Ecosystem.Utils namespace
  

###1.0.0
**New:** Initial release

