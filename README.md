# PushApp
PUSHAPP

Persist Settings

-----------------------------------

USE CASE

-----------------------------------

Persist user settings and window properties.

The user should be able to persist any window property from one session to the next, These properties include the app window size, position on the screen and control UI properties. Each time the app is run the previous window properties will be restored.

The user should be able to persist any configuration setting from one session to the next. These settings include source and target folders, file extension filters, and duplicate file action.  Each time the app is run the previous settings will be restored.

Configuration and window property settings must be persisted in the same file. 

The current configuration setting persistence must be upgraded to support window property persistence.

-----------------------------------

DEVELOPER COMMENT:

-----------------------------------

The current configuration settings code will be migrated to a more flexible "User Application Data Path" persistence.  This technique is built using Application.UserAppDataPath Property which is built into .NET.  See the link for more information: 

https://msdn.microsoft.com/en-us/library/system.windows.forms.application.userappdatapath.aspx

This code is actively under development.  Early check-ins may contain un-factored code and large blocks of commented-out code.  Early check-ins may contain undiscovered bugs, and constants that will eventually be removed.

Early development check-ins serve the purpose of providing 'real' code samples for potential clients to review.  Under normal circumstances, un-factored, commented-out code blocks and bugs would NOT be checked-in to the remote repository (GitHub). 

-----------------------------------

DEVELOPMENT STATUS

-----------------------------------

October 15, 2015 - Work-In-Progress

Create README

-----------------------------------
