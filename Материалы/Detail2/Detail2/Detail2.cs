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

namespace Detail2
{
    [ContainsCommands]
    public class Detail2
    {
        [CommandMethod("detail1_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
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
            DbPolyline po = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(80, 0, 0), new Point3d(80, 130, 0), new Point3d(0, 130, 0), new Point3d(0, 0, 0) }) };
            //добавляем окружности с центрами (20,110,0) и (60,110,0) и радиусом 10
            DbCircle circle01 = new DbCircle()
            {
                Center = new Point3d(20, 110, 0),
                Radius = 10,
            };
            DbCircle circle02 = new DbCircle()
            {
                Center = new Point3d(60, 110, 0),
                Radius = 10,
            };



            po.DbEntity.AddToCurrentDocument();
            circle01.DbEntity.AddToCurrentDocument();
            circle02.DbEntity.AddToCurrentDocument();

            sketchDetail.AddObject(po.ID);
            sketchDetail.AddObject(circle01.ID);
            sketchDetail.AddObject(circle02.ID);

            SketchProfile profile1 = sketchDetail.CreateProfile();
            if (profile1 != null)
            {
                profile1.AutoProcessExternalContours();
                EF1.ProfileID = profile1.ID;
                EF1.Distance = 20;
                EF1.Angle = 0;
                EF1.Operation = PartFeatureOperation.Join;
                EF1.Direction = FeatureExtentDirection.Positive;
                McObjectManager.UpdateAll();
            }

            // добавление цилиндра
            PlanarSketch sketchDetail2 = Detail3d.AddPlanarSketch();

            DbCircle circle2 = new DbCircle()
            {
                Center = new Point3d(40, 40, 20),
                Radius = 30,
            };

            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds = EF1.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail2.PlanarEntityID = endFacesIds[0];
            sketchDetail2.DbEntity.Visibility = 0;

            circle2.DbEntity.AddToCurrentDocument();
            sketchDetail2.AddObject(circle2.ID);
            SketchProfile profile2 = sketchDetail2.CreateProfile();

            //CircArc3d projCA_wcs = skProj.ProjectedGeometry.CircArc;
            // profile2.AddOrRemoveRegionByLine(new Line3d(projCA_wcs.Center + projCA_wcs.Normal.GetPerpendicularVector() * projCA_wcs.Radius * 0.9, projCA_wcs.Normal));
            profile2.AutoProcessExternalContours();
            ExtrudeFeature EF2 = new ExtrudeFeature();
            EF2 = Detail3d.AddExtrudeFeature(
                profile2.ID,
                30,
                0,
                FeatureExtentDirection.Positive);
            EF2.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();




            // добавление усеченной пирамиды
            PlanarSketch sketchDetail3 = Detail3d.AddPlanarSketch();

            DbPolyline po2 = new Polyline3d(new List<Point3d>() { new Point3d(20, 20, 50), new Point3d(60, 20, 50), new Point3d(60, 60, 50), new Point3d(20, 60, 50), new Point3d(20, 20, 50) });



            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds1 = EF2.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail3.PlanarEntityID = endFacesIds1[0];
            sketchDetail3.DbEntity.Visibility = 0;


            po2.DbEntity.AddToCurrentDocument();
            sketchDetail3.AddObject(po2.ID);

            SketchProfile profile3 = sketchDetail3.CreateProfile();

            //CircArc3d projCA_wcs = skProj.ProjectedGeometry.CircArc;
            // profile2.AddOrRemoveRegionByLine(new Line3d(projCA_wcs.Center + projCA_wcs.Normal.GetPerpendicularVector() * projCA_wcs.Radius * 0.9, projCA_wcs.Normal));
            profile3.AutoProcessExternalContours();
            ExtrudeFeature EF3 = Detail3d.AddExtrudeFeature(
                profile3.ID,
                15,
                15 / 180.0 * Math.PI,
                FeatureExtentDirection.Positive);
            EF3.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();


            // добавление отверстия
            PlanarSketch sketchDetail4 = Detail3d.AddPlanarSketch();

            DbCircle circle3 = new DbCircle()
            {
                Center = new Point3d(40, 40, 65),
                Radius = 13,
            };

            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds2 = EF3.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail4.PlanarEntityID = endFacesIds2[0];
            sketchDetail4.DbEntity.Visibility = 0;

            circle3.DbEntity.AddToCurrentDocument();
            sketchDetail4.AddObject(circle3.ID);
            SketchProfile profile4 = sketchDetail4.CreateProfile();

            //CircArc3d projCA_wcs = skProj.ProjectedGeometry.CircArc;
            // profile2.AddOrRemoveRegionByLine(new Line3d(projCA_wcs.Center + projCA_wcs.Normal.GetPerpendicularVector() * projCA_wcs.Radius * 0.9, projCA_wcs.Normal));
            profile4.AutoProcessExternalContours();
            ExtrudeFeature EF4 = new ExtrudeFeature();
            EF4 = Detail3d.AddExtrudeFeature(
                profile4.ID,
                65,
                0,
                FeatureExtentDirection.Negative);
            EF4.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();


        }
    }
}
