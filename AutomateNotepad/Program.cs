using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using System.Windows.Forms;
using UIAutomationClientsideProviders;

namespace AutomateNotepad
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName("notepad").Length == 0)
            {
                Process.Start("notepad.exe");

                try
                {
                    CreateHelloWorldFile();
                }
                catch
                {
                    Console.WriteLine("Error. Notepad window not found.");
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled. Notepad is already open.");
            }

            Console.ReadKey();

        }

        public static void CreateHelloWorldFile()
        {
            ClientSettings.RegisterClientSideProviders(UIAutomationClientSideProviders.ClientSideProviderDescriptionTable);

            AutomationElement root = AutomationElement.RootElement;

            // windowCondition is true if it is a window AND is named "Untitled - Notepad"
            AndCondition windowCondition = new AndCondition(
                                           new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                                           new PropertyCondition(AutomationElement.NameProperty, "Untitled - Notepad")
                                           );

            // Search root element for window that matches the conditions of windowCondition.
            var rootChild = root.FindFirst(TreeScope.Subtree, windowCondition);
            if (rootChild == null)
            {
                Console.WriteLine("Cannot find window");
                return;
            };

            // windowCondition is true if it is a menu item named "File"
            AndCondition fileCondition = new AndCondition(
                                         new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem),
                                         new PropertyCondition(AutomationElement.NameProperty, "File")
                                         );
            AutomationElement fileMenuElement = rootChild.FindFirst(TreeScope.Subtree, fileCondition);
            if (fileMenuElement == null)
            {
                Console.WriteLine("Cannot find File menu item");
                return;
            };

            //Find submenu item: File > New
            FindSubmenuItem(fileMenuElement, "New");

            // Write Hello World
            SendKeys.SendWait("Hello World");

            // Find submenu item: File Menu > Save As
            FindSubmenuItem(fileMenuElement, "Save As...");

            // Enter file name - NOTE: DESIGNATE A DIRECORY PATH ON LINE 78 BEFORE RUNNING THIS PROGRAM
            string directoryPath = "D:\\Users\\...";
            SendKeys.SendWait(directoryPath + "HelloWorld.txt");

            // Save file
            AndCondition saveWindowCondition = new AndCondition(
                                               new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                                               new PropertyCondition(AutomationElement.NameProperty, "Save as")
                                               );
            rootChild.FindFirst(TreeScope.Subtree, saveWindowCondition);

            AndCondition clickSave = new AndCondition(
                                     new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                                     new PropertyCondition(AutomationElement.NameProperty, "Save")
                                     );
            AutomationElement saveButton = rootChild.FindFirst(TreeScope.Subtree, clickSave);
            InvokeClick(saveButton);


            // Confirm Save As
            AndCondition confirmSaveWindowCondition = new AndCondition(
                                                      new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                                                      new PropertyCondition(AutomationElement.NameProperty, "Confirm Save as")
                                                      );
            rootChild.FindFirst(TreeScope.Subtree, confirmSaveWindowCondition);

            AndCondition clickConfirmSave = new AndCondition(
                                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
                                            new PropertyCondition(AutomationElement.NameProperty, "Yes")
                                            );
            AutomationElement confirmSaveButton = rootChild.FindFirst(TreeScope.Subtree, clickConfirmSave);
            InvokeClick(confirmSaveButton);

            // Search directory for existance of HelloWorld.txt / Report if successful or unsuccessful
            SearchDirForFile(directoryPath);



        }

        public static void FindSubmenuItem(AutomationElement fileElement, string submenuItem)
        {
            var expandPattern = fileElement.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            if (expandPattern.Current.ExpandCollapseState != ExpandCollapseState.Expanded)
                expandPattern.Expand();

            PropertyCondition submenuCondition = new PropertyCondition(AutomationElement.NameProperty, submenuItem);
            AutomationElement submenuElement = fileElement.FindFirst(TreeScope.Subtree, submenuCondition);
            if (submenuElement == null)
            {
                Console.WriteLine($"Cannot find {submenuItem} submenu item");
                return;
            };

            InvokeClick(submenuElement);
        }

        public static void InvokeClick(AutomationElement element)
        {
            InvokePattern clickPattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            clickPattern.Invoke();
        }

        public static void SearchDirForFile(string directoryPath)
        {
            bool helloWorldExists = false;
            string[] listFiles = Directory.GetFiles(directoryPath);
            foreach (string fileName in listFiles)
            {
                if (fileName.Contains("HelloWorld.txt"))
                {
                    helloWorldExists = true;
                }
            }

            // Report if save was successful
            if (helloWorldExists)
            {
                Console.WriteLine("HelloWorld.txt successfully saved");
            }
            else
            {
                Console.WriteLine("Save Unsuccessful");
            }
        }
    }

}

