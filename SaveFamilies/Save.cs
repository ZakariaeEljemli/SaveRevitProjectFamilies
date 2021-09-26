using System;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using System.Runtime.InteropServices;
using System.IO;


namespace SaveFamilies
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Save : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
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


            return Result.Succeeded;
        }

        
    }
}
