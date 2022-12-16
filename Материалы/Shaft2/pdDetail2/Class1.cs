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

namespace pdDetail2
{
    [ContainsCommands]
    public class pdDetail2
    {


        [CommandMethod("Val", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Sample3d()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); // получили активную страницу

            Mc3dSolid solid = new Mc3dSolid(); // создаем пустой солид
            bool addingSolidResult = McObjectManager.Add2Document(solid.DbEntity, activeSheet); // добавление солида в документ
            PlanarSketch sketch = solid.AddPlanarSketch(); // создание эскиза

            //создаем первый цилиндр
            Polyline3d poly1 = new Polyline3d(new List<Point3d>() {
             new Point3d(0, 0, 0),
             new Point3d(200, 0, 0),
             new Point3d(200, 200, 0),
             new Point3d(0, 200, 0),
             new Point3d(0, 0, 0) });
            poly1.Vertices.MakeChamferAtVertex(3, 20);
            DbPolyline rect = new DbPolyline()
            {
                Polyline = poly1
            };     
            rect.DbEntity.AddToCurrentDocument(); //добавление к текущему документу
            sketch.AddObject(rect.ID);
            DbLine axisLine = new DbLine() { Line = new LineSeg3d(new Point3d(0, 0, 0), new Point3d(1000, 0, 0)) };
            axisLine.DbEntity.AddToCurrentDocument();
            McGeomParam axisGP = new McGeomParam() { ID = axisLine.ID };
            SketchProfile profile = sketch.CreateProfile(); // получаем профиль для создания тела
            if (profile != null)
            {
                profile.AutoProcessExternalContours(); // Добавляет или удаляет из набора все внешние замкнутые регионы эскиза
                RevolveFeature revolve = solid.AddRevolveFeature(profile.ID, axisGP, 2 * Math.PI);
            }
          
            

            //создаем 2 цилиндр
            DbPolyline rect2 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(200, 0, 0), new Point3d(400, 0, 0), new Point3d(400, 250, 0), new Point3d(200, 250, 0), new Point3d(200, 0, 0) }) };
            rect2.DbEntity.AddToCurrentDocument(); //добавление к текущему документу
            PlanarSketch sketch2 = solid.AddPlanarSketch();
            sketch2.AddObject(rect2.ID);
            SketchProfile profile2 = sketch2.CreateProfile();
            profile2.AutoProcessExternalContours();
            RevolveFeature revolve2 = solid.AddRevolveFeature(profile2.ID, axisGP, 2 * Math.PI);



            //создаем 3 цилиндр
            Polyline3d poly2 = new Polyline3d(new List<Point3d>() {
            new Point3d(400, 0, 0), 
            new Point3d(600, 0, 0), 
            new Point3d(600, 200, 0), 
            new Point3d(400, 200, 0), 
            new Point3d(400, 0, 0)});
            poly2.Vertices.MakeChamferAtVertex(2, 20);
            DbPolyline rect3 = new DbPolyline()
            {
                Polyline = poly2
            };
            rect3.DbEntity.AddToCurrentDocument(); //добавление к текущему документу
            PlanarSketch sketch3 = solid.AddPlanarSketch();
            sketch3.AddObject(rect3.ID);
            SketchProfile profile3 = sketch3.CreateProfile();
            profile3.AutoProcessExternalContours();
            RevolveFeature revolve3 = solid.AddRevolveFeature(profile3.ID, axisGP, 2 * Math.PI);



            //добавляем отверстие
            PlanarSketch sketch4 = solid.AddPlanarSketch();
            DbCircle circle4 = new DbCircle()
            {
                Center = new Point3d(100, 0, 0),
                Radius = 50,
            };
            circle4.DbEntity.AddToCurrentDocument();
            sketch4.AddObject(circle4.ID);
            SketchProfile profile4 = sketch4.CreateProfile();
            profile4.AutoProcessExternalContours();
            ExtrudeFeature EF4 = solid.AddExtrudeFeature(
                profile4.ID,
                400,
                0,
                FeatureExtentDirection.Symmetric);
            EF4.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();



            //добавляем отверстие
            PlanarSketch sketch5 = solid.AddPlanarSketch();
            Plane3d ps1_plane = new Plane3d(new Point3d(0, 0, 0), new Vector3d(0, 1, 0), new Vector3d(0, 0, 1));
            sketch5.DbEntity.AddToCurrentDocument();
            DbCircle circle5 = new DbCircle()
            {
                Center = new Point3d(0, 0, 0),
                Radius = 150,
            };
            //добавляем оркужность в документ
            circle5.DbEntity.AddToCurrentDocument();
            //добавляем окружность в эскиз
            sketch5.AddObject(circle5.ID);
            sketch5.DbEntity.Update();
            //выдавливаем окружность
            SketchProfile profile5 = sketch5.CreateProfile();
            profile5.AutoProcessExternalContours();
            ExtrudeFeature EF5 = solid.AddExtrudeFeature(profile5.ID, 600, 0, FeatureExtentDirection.Positive);
            EF5.Operation = PartFeatureOperation.Cut;
            sketch5.SetPlane(ps1_plane);
            McObjectManager.UpdateAll();
        }


    }


}

