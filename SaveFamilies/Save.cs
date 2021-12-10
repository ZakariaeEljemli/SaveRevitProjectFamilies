using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;


namespace SaveFamilies
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Save : IExternalApplication
    {
        static void AddRibbonPanel(UIControlledApplication application)
        {
            application.CreateRibbonTab("Save Families");

            try
            {
                List<RibbonPanel> panels = application
                  .GetRibbonPanels("Add-Ins");
            }
            catch
            {
                // Tab "TBC" does not yet exist, 
                // so create new

                application.CreateRibbonTab("SaveFamilies");
            }

            RibbonPanel panel = application
              .CreateRibbonPanel("Add-Ins", "Save Families");
            // Ajoute un ruban associé à l'onglet
            //RibbonPanel ArchiPanel = application.CreateRibbonPanel("Save Families", "Tester le help");

            // Création des paramètres du bouton associé aux différentes macros
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // associé à "VCTCommand" --> La méthode a appelé lors du click bouton
            PushButtonData buttonData1 = new PushButtonData("Archi",
               "Bouton Simple", thisAssemblyPath, "SaveFamilies.SaveFamily");

  
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddRibbonPanel(application);
            // Obligatoire ici
            return Result.Succeeded;
        }

        public class SaveFamily : IExternalCommand
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
                                FamDoc.SaveAs(@"C:\Users\ELJEMLI\Downloads\Elements Electriques Test\" + elem.Name.ToString() + ".rfa");
                                FamDoc.Close(false);
                                tr.Commit();
                            }
                        }
                    }
                }
                return Autodesk.Revit.UI.Result.Succeeded;

            }

        }

    }
}
