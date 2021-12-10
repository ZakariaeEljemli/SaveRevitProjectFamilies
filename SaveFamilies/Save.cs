using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.Runtime.InteropServices;
using System.Runtime;
using AdWin = Autodesk.Windows;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace SaveFamilies
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Save : IExternalApplication
    {
     
        public Result OnStartup(UIControlledApplication application)
        {
            foreach (AdWin.RibbonTab tableau in AdWin.ComponentManager.Ribbon.Tabs)
            {
                if (tableau.Id == "Insert")
            {
            ;
            foreach (AdWin.RibbonPanel panel in tableau.Panels)
            {
                //Autodesk.Revit.UI.TaskDialog.Show("hh", panel.Source.Id);

                if (panel.Source.Id == "CustomCtrl_%Insert%LoadFromLibrary")
                {
                            
                            AdWin.RibbonButton button = new AdWin.RibbonButton();
                            button.Name = "FamilySaver";
                            button.Image = new BitmapImage(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"save.png"));
                            button.LargeImage = new BitmapImage(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"save.png"));
                            button.Id = "FamilySaver_Button";
                            button.AllowInStatusBar = true;
                            button.AllowInToolBar = true;
                            button.GroupLocation = Autodesk.Private.Windows.RibbonItemGroupLocation.Middle;
                            button.IsEnabled = true;
                            button.IsToolTipEnabled = true;
                            button.IsVisible = true;
                            button.ShowImage = true;
                            button.ShowText = true;
                            button.ShowToolTipOnDisabled = true;
                            button.Text = "Save Families as you want";
                            button.ToolTip = "Choose your project families to save";
                            button.MinHeight = 0;
                            button.MinWidth = 0;
                            button.Size = Autodesk.Windows.RibbonItemSize.Large;
                            button.ResizeStyle = Autodesk.Windows.RibbonItemResizeStyles.HideText;
                            button.IsCheckable = true;
                            button.Orientation = System.Windows.Controls.Orientation.Vertical;
                            button.KeyTip = "TBC";
                            AdWin.ComponentManager.UIElementActivated
                              += new EventHandler<
                                Autodesk.Windows.UIElementActivatedEventArgs>(ComponentManager_UIElementActivated);
                            panel.Source.Items.Add(button);


            }
            }
            }
            }
            return Result.Succeeded;
        }
        void ComponentManager_UIElementActivated(object sender, Autodesk.Windows.UIElementActivatedEventArgs e)
        {
            if (e != null
              && e.Item != null
              && e.Item.Id != null
              && e.Item.Id == "FamilySaver_Button")
            {
                //Interface boite = new Interface();
                //boite.ShowDialog();

                string path = Assembly
                  .GetExecutingAssembly().Location;
                Document doc = revit.application.ActiveUIDocument.Document;
                IList<Element> FamilyCollector = new FilteredElementCollector(doc).OfClass(typeof(Family)).ToElements();
                foreach (Family elem in FamilyCollector)
                {
                    if (elem.IsEditable == true)
                    {
                        Document FamDoc = doc.EditFamily(elem);
                        if (FamDoc.IsModifiable == true)
                        {
                            using (Transaction tr = new Transaction(doc))
                            {
                                tr.Start("hhh");
                                FamDoc.SaveAs(@"C:\Users\ELJEMLI Zakariae\Downloads\Familles\" + elem.Name.ToString() + ".rfa");
                                FamDoc.Close(false);
                                tr.Commit();
                            }
                        }
                    }
                }

            }
        }
        /*public class SaveFamily : IExternalCommand
        {
            // Code exécuté en cas de clic
            public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit, ref string message, ElementSet elements)
            {
                Document doc = revit.Application.ActiveUIDocument.Document;
                IList<Element> FamilyCollector = new FilteredElementCollector(doc).OfClass(typeof(Family)).ToElements();
                foreach (Family elem in FamilyCollector)
                {
                    if (elem.IsEditable == true)
                    {
                        Document FamDoc = doc.EditFamily(elem);
                        if (FamDoc.IsModifiable == true)
                        {
                            using (Transaction tr = new Transaction(doc))
                            {
                                tr.Start("hhh");
                                FamDoc.SaveAs(@"C:\Users\ELJEMLI Zakariae\Downloads\Familles\" + elem.Name.ToString() + ".rfa");
                                FamDoc.Close(false);
                                tr.Commit();
                            }
                        }
                    }
                }
                return Autodesk.Revit.UI.Result.Succeeded;

            }

        }*/

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
