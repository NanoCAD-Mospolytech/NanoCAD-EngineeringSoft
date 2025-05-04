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
using System.Runtime.Serialization;

namespace detail3
{
    [ContainsCommands]
    public class detail3
    {
        [CommandMethod("detail3", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Sample3d()
        {
            // получили активную страницу
            var activeSheet = McDocumentsManager.GetActiveSheet();
            // создаем пустой солид
            Mc3dSolid Detail3d = new Mc3dSolid();

            // Создаем первую солид-образующую фичу, это будет выдавливание
            ExtrudeFeature EF1 = new ExtrudeFeature();
            Detail3d = EF1.Cast<Mc3dSolid>(); // первая фича является настоящим 3D-солидом в документе
            // добавление солида в документ
            bool addingSolidResult1 = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument(); // в документ все объекты желательно добавлять сразу после создания
            PlanarSketch sketchDetail = Detail3d.AddPlanarSketch(); // создаём эскиз для контура первого выдавливания

            // создаем полилинией прямоугольник 80*130 в плоскости XY
            DbPolyline rect1 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(80, 0, 0), new Point3d(80, 130, 0), new Point3d(0, 130, 0), new Point3d(0, 0, 0) }) };
            //добавляем окружности с центрами (20,110,0) и (60,110,0) и радиусом 10
            DbCircle circle01 = new DbCircle()
            {
                Center = new Point3d(40, 20, 0),
                Radius = 10,
            };
            DbCircle circle02 = new DbCircle()
            {
                Center = new Point3d(40, 110, 0),
                Radius = 10,
            };
            DbCircle circle03 = new DbCircle()
            {
                Center = new Point3d(40, 65, 0),
                Radius = 15,
            };

            //добавляем фигуры в документ
            rect1.DbEntity.AddToCurrentDocument();
            circle01.DbEntity.AddToCurrentDocument();
            circle02.DbEntity.AddToCurrentDocument();
            circle03.DbEntity.AddToCurrentDocument();

            //добавляем фигуры в эскиз
            sketchDetail.AddObject(rect1.ID);
            sketchDetail.AddObject(circle01.ID);
            sketchDetail.AddObject(circle02.ID);
            sketchDetail.AddObject(circle03.ID);

            //добавляем новое твердое тело путем выдавливания
            SketchProfile profile1 = sketchDetail.CreateProfile();
            if (profile1 != null)
            {
                profile1.AutoProcessExternalContours();
                EF1.ProfileID = profile1.ID;
                EF1.Distance = 20;
                EF1.Angle = 0;
                EF1.PartOperation = PartFeatureOperation.Join;
                EF1.Direction = FeatureExtentDirection.Positive;
                McObjectManager.UpdateAll();
            }

            // добавление цилиндра
            PlanarSketch sketchDetail2 = Detail3d.AddPlanarSketch();
            DbCircle circle2 = new DbCircle()
            {
                Center = new Point3d(40, 65, 20),
                Radius = 25,
            };
            DbCircle circle3 = new DbCircle()
            {
                Center = new Point3d(40, 65, 20),
                Radius = 15,
            };
            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds = EF1.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail2.PlanarEntityID = endFacesIds[0];
            sketchDetail2.DbEntity.Visibility = 0;
            //добавляем фигуры в документ
            circle2.DbEntity.AddToCurrentDocument();
            circle3.DbEntity.AddToCurrentDocument();
            //добавляем фигуры в эскиз
            sketchDetail2.AddObject(circle2.ID);
            sketchDetail2.AddObject(circle3.ID);

            //выдавливам цилиндр
            SketchProfile profile2 = sketchDetail2.CreateProfile();
            profile2.AutoProcessExternalContours();
            ExtrudeFeature EF2 = new ExtrudeFeature();
            EF2 = Detail3d.AddExtrudeFeature(
                profile2.ID,
                60,
                0,
                FeatureExtentDirection.Positive);
            EF2.PartOperation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();

            //добавляем прямоугольное отверстие
            //создаем новую плоскость
            PlanarSketch sketchDetail3 = Detail3d.AddPlanarSketch();
            Plane3d ps1_plane = new Plane3d(new Point3d(0, 35, 0), new Vector3d(1, 0, 0), new Vector3d(0, 1, 0));
            sketchDetail3.DbEntity.AddToCurrentDocument();

            DbPolyline rect2 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0,0,0), new Point3d(80,0,0),
            new Point3d(80,60,0), new Point3d(0,60,0), new Point3d(0,0,0) })
            };
            //добавляем прямоугольник в документ
            rect2.DbEntity.AddToCurrentDocument();
            //добавляем прямоугольник в эскиз
            sketchDetail3.AddObject(rect2.ID);
            sketchDetail3.DbEntity.Update();
            //выдавливаем прямоугольник
            SketchProfile profile3 = sketchDetail3.CreateProfile();
            profile3.AutoProcessExternalContours();
            ExtrudeFeature EF3 = Detail3d.AddExtrudeFeature(profile3.ID, 10, 0, FeatureExtentDirection.Positive);
            EF3.PartOperation = PartFeatureOperation.Cut;

            sketchDetail3.SetPlane(ps1_plane);
            McObjectManager.UpdateAll();

        }
    }
}