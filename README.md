# PushApp
PUSHAPP – Configuration Feature Branch

For general information on PUSHAPP see the MASTER branch.

USE CASE

The user needs to be able to configure various settings such as source and target path, file extension filter, and default duplicate behavior. 

DEVELOPMENT STATUS

October 9, 2015 - Work-InProgress: Continued to develop the functionality related to configuring the Push application.
1) Created a default settings file when the user settings file does not exist. The file is used to create a 'seed' user settings file.  When the seed file is used, the user will be forced to add the required values like source and target folders.
2) Created a new application folder for the default settings file, file extension filter file and application configuration file (future).
3) Rearrange Configuration UI controls.
4) Change how the Duplicate Action radio buttons are displayed when the "Hide Duplicate Message" check box is selected.
5) Modified the Duplicate action model dialog so it would display in the middle of the main application windows. Previously, it was displaying in the center of the monitor.
6) Redesign existing code to handle "Don't show this message again" check box.
7) Rename files so code is easier to understand.
8) Fixed bugs.
9) Clean up code.


October 8, 2015 - Work-In-Progress:
Completed work on settings related to source and target paths. This includes validation, failure notification, clear values and load saved values.
Completed work on file extension filtering. This includes load saved values, parsing, hydrating file extension array.
Did a lot of code clean up and some refactoring.

October 5, 2015 - Work-In-Progress:
Add Button Bar with Run, Refresh, and Configure buttons to Form1 (main application form).
Add Form2 (configuration form).
Add radio buttons, check boxes, text boxes to form2.
Add FolderBrowserDialog control for choosing source & target path to form2.
Add OpenFileDialog control for loading external file extension filter to form2. 

Comment: This is a work-in-progress.  This code has not been refactored.  The names of all controls will be renamed to something meaningful before release.
--Kevin

DISCLAIMER:
This is un-factored code.  There are many duplicate blocks of code that will be refactored before release. This code has not been heavy tested but does work properly in XP.  This code is available to review and comment.  This code is not ready for release.

