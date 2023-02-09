using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_3_7_1
{
    public class ViewsUtils
    {
        // Метод для выбора всех типов видов (https://www.revitapidocs.com/2017/fb92a4e7-f3a7-ef14-e631-342179b18de9.htm)
        public static List<View> GetViews(ExternalCommandData commandData)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            List<View> views = new FilteredElementCollector(doc)
                .OfClass(typeof(View))                
                .Cast<View>()
                .ToList();

            return views;
        }
       

        
    }
}
