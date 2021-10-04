# AutomateNotepad

## Description

This is an RPA program built in C# to automate the creation of a new .txt file in notepad. This is a **Console Application** in the .NET Framework.

## Challenge Objectives

- [x] Check to see if notepad application is already open
- [x] If not open, Lunch the Notepad application 
- [x] Click on File Menu Item followed by New menu item.
- [X] Enter “Hello World” into the Text Area.
- [X] Click on Save and Save as… Menu Items.
- [X] Detect and handle Windows Save as dialog – Enter file path and Click save on the dialog.
- [X] Check to make sure file was stored successfully in the specified location.
- [X] Report task was completed successfully.

## Notes

- System.Diagnostics was solely used for the Process Class to locate and open instances of notepad.exe

- System.Windows.Automation namespace was used to locate control types on the desktop. This helped me locate the notepad window after it was opened, menu items, submenu items, and buttons.

- The System.Windows.Forms namspace was used specifically for the SendKeys class. I needed a way to send text to the document as well as naming the file. I think there is probably a better way than relying on a namespace usually desginated for a forms app, but I wasn't able to find an alternative at this time.

- System.IO was used solely for the Directory class to traverse files in the Desktop directory for the save file.

- There is a 5-10 second delay when clicking on the 'Yes' button in the "Confirm Save As" dialog that requires additional work to solve.

- If it is the first time saving "HelloWorld.txt" an exception is thrown in the InvokeClick method because the "Confirm Save As" dialog does not generate. Conditional logic still needs to be added in order to skip this step on the first save.