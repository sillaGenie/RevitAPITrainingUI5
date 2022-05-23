using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingUI5._1
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand PipesCount { get; }
        public DelegateCommand WallsValue { get; }
        public DelegateCommand DoorsCount { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            PipesCount = new DelegateCommand(GetPipesCount);
            WallsValue = new DelegateCommand(GetWallsVolume);
            DoorsCount = new DelegateCommand(GetDoorsCount);
        }

        private void GetDoorsCount()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            List<FamilyInstance> fInstances = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .ToList();
            TaskDialog.Show("Doors count", fInstances.Count.ToString());
        }

        private void GetWallsVolume()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            double Sum = 0;
            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();
            foreach (Wall wall in walls)
            {
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                    Sum += volumeValue;
                }
                TaskDialog.Show("volume", Sum.ToString());
            }
        }

        private void GetPipesCount()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Pipe> pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();
            TaskDialog.Show("Windows info", pipes.Count.ToString());
        }

    }
}
