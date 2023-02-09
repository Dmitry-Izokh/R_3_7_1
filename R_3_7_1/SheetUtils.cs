using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_7_1
{
   public class SheetUtils
    {
        //Метод для выбора семейства основной надписи
        public static List<FamilySymbol> GetTitleBlocks(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var titleBlocks = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .ToList();

            return titleBlocks;
        }

        // Метод для выбора семейства заголовка вида на листе (видового экрана)
        public static List<FamilySymbol> GetViewTitles(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var viewTitles = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_ViewportLabel)
                .Cast<FamilySymbol>()
                .ToList();

            return viewTitles;
        }

        // Метод для выбора семейства видового экрана
        public static List<FamilySymbol> GetVievports(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var vievports = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_Viewports)
                .Cast<FamilySymbol>()
                .ToList();

            return vievports;
        }
    }
}
