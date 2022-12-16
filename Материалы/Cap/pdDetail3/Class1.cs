using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multicad;
using Multicad.Runtime;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
using Multicad.Mc3D;
using System.Security.Cryptography;
using System.Runtime.Serialization;

namespace pdDetail3
{
    [ContainsCommands]
    public class pdDetail3
    {


        [CommandMethod("Cap", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Cap()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); // получили активную страницу
            Mc3dSolid solid = new Mc3dSolid(); // создаем пустой солид
            bool addingSolidResult = McObjectManager.Add2Document(solid.DbEntity, activeSheet); // добавление солида в документ
            PlanarSketch sketch1 = solid.AddPlanarSketch(); // создание эскиза
            //создаем модель крышки
            Polyline3d cap = new Polyline3d(new List<Point3d>() {
                new Point3d(5,0,0),
                new Point3d(15,0,0),
                new Point3d(15,25,0),
                new Point3d(20,35,0),
                new Point3d(20,52,0),
                new Point3d(15,52,0),
                new Point3d(15,41,0),

                new Point3d(5,41,0),
                new Point3d(5,32,0),
                new Point3d(0,32,0),
                new Point3d(0,25,0),
                new Point3d(5,25,0),
                new Point3d(5,0,0)
               });
            cap.Vertices.MakeFilletAtVertex(8, 1);
            DbPolyline cap01 = new DbPolyline()
            {
                Polyline = cap
            };
            cap01.DbEntity.AddToCurrentDocument(); //добавление к текущему документу
            sketch1.AddObject(cap01.ID);//создаем эскиз
            DbLine axisLine = new DbLine() { Line = new LineSeg3d(new Point3d(0, 75, 0), new Point3d(1, 75, 0)) };//создаем ось вращен ия
            axisLine.DbEntity.AddToCurrentDocument();//добавляем ось вращения к документу
            McGeomParam axisGP = new McGeomParam() { ID = axisLine.ID };//присваеваем значению параметра значение оси
            SketchProfile profile1 = sketch1.CreateProfile(); // получаем профиль для создания тела
            if (profile1 != null)
            {
                profile1.AutoProcessExternalContours(); // Добавляет или удаляет из набора все внешние замкнутые регионы эскиза
                RevolveFeature revolve = solid.AddRevolveFeature(profile1.ID, axisGP, 2 * Math.PI);//создаем объектвращением
            }

            //добавляем отверстие
            PlanarSketch sketch2 = solid.AddPlanarSketch();
            Plane3d ps1_plane = new Plane3d(new Point3d(0, 75, 0), new Vector3d(0, 1, 0), new Vector3d(0, 0, 1));
            sketch2.DbEntity.AddToCurrentDocument();
            DbCircle circle1 = new DbCircle()
            {
                Center = new Point3d(0, 0, 0),
                Radius = 23,
            };
            //добавляем оркужность в документ
            circle1.DbEntity.AddToCurrentDocument();
            //добавляем окружность в эскиз
            sketch2.AddObject(circle1.ID);
            sketch2.DbEntity.Update();
            //выдавливаем окружность
            SketchProfile profile2 = sketch2.CreateProfile();
            profile2.AutoProcessExternalContours();
            ExtrudeFeature EF5 = solid.AddExtrudeFeature(profile2.ID, 20, 0, FeatureExtentDirection.Positive);
            EF5.Operation = PartFeatureOperation.Cut;
            sketch2.SetPlane(ps1_plane);
            McObjectManager.UpdateAll();

            //krugovoy massiv
            PlanarSketch sketch3 = solid.AddPlanarSketch();
            sketch3.DbEntity.AddToCurrentDocument();
            DbCircle circle2 = new DbCircle()
            {
                Center = new Point3d(0, 31.25, 0),
                Radius = 4.5,
            };

            circle2.DbEntity.AddToCurrentDocument();
            sketch3.AddObject(circle2.ID);
            sketch3.DbEntity.Update();

            SketchProfile profile3 = sketch3.CreateProfile();
            profile3.AutoProcessExternalContours();
            ExtrudeFeature EF1 = solid.AddExtrudeFeature(profile3.ID, 25, 0, FeatureExtentDirection.Positive);
            EF1.Operation = PartFeatureOperation.Cut;

            CircularPatternFeature circArray = solid.AddCircularPatternFeature(new McObjectId[] { EF1.ID }, axisGP, 6, 1.0471975423);
            circArray.DbEntity.AddToCurrentDocument();
            sketch3.SetPlane(ps1_plane);
            McObjectManager.UpdateAll();

        }

    }

}
