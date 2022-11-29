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

namespace cube
{
    [ContainsCommands]
    public class Shaft3D
    {
        //shaft - вал (не знаем перевод, идем в переводчик)

        [CommandMethod("shaft_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Shaft()
        {
            // получили активную страницу
            var activeSheet = McDocumentsManager.GetActiveSheet();
            // создаем пустой солид
            Mc3dSolid Shaft = new Mc3dSolid();

            // Создаем первую солид-образующую фичу, это будет выдавливание
            ExtrudeFeature EF1 = new ExtrudeFeature();
            Shaft = EF1.Cast<Mc3dSolid>(); // первая фича является настоящим 3D-солидом в документе
            // добавление солида в документ
            bool addingSolidResult1 = McObjectManager.Add2Document(Shaft.DbEntity, activeSheet);
            Shaft.DbEntity.AddToCurrentDocument(); // в документ все объекты желательно добавлять сразу после создания
            PlanarSketch sketchDetail = Shaft.AddPlanarSketch(); // создаём эскиз для контура первого выдавливания



            //добавляем цилиндр1
            //добавляем окружность с центром (0,0,0) и радиусом 40
            DbCircle circle1 = new DbCircle()
            {
                Center = new Point3d(0, 0, 0),
                Radius = 20,
            };

            circle1.DbEntity.AddToCurrentDocument();
            sketchDetail.AddObject(circle1.ID);


            SketchProfile profile1 = sketchDetail.CreateProfile();
            if (profile1 != null)
            {
                profile1.AutoProcessExternalContours();
                EF1.ProfileID = profile1.ID;
                EF1.Distance = 40;
                EF1.Angle = 0;
                EF1.Operation = PartFeatureOperation.Join;
                EF1.Direction = FeatureExtentDirection.Positive;
                McObjectManager.UpdateAll();
            }

            //добавляем цилиндр 2
            PlanarSketch sketchDetail2 = Shaft.AddPlanarSketch();

            DbCircle circle2 = new DbCircle()
            {
                Center = new Point3d(0, 0, 40),
                Radius = 30,
            };


            List<McObjectId> endFacesIds1 = EF1.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail2.PlanarEntityID = endFacesIds1[0];
            sketchDetail2.DbEntity.Visibility = 0;

            circle2.DbEntity.AddToCurrentDocument();
            sketchDetail2.AddObject(circle2.ID);
            SketchProfile profile2 = sketchDetail2.CreateProfile();

            profile2.AutoProcessExternalContours();
            ExtrudeFeature EF2 = new ExtrudeFeature();
            EF2 = Shaft.AddExtrudeFeature(
                profile2.ID,
                50,
                0,
                FeatureExtentDirection.Positive);
            EF2.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();

            //добавление цилиндра 3
            PlanarSketch sketchDetail3 = Shaft.AddPlanarSketch();

            DbCircle circle3 = new DbCircle()
            {
                Center = new Point3d(0, 0, 90),
                Radius = 20,
            };

            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds2 = EF2.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail3.PlanarEntityID = endFacesIds2[0];
            sketchDetail3.DbEntity.Visibility = 0;


            circle3.DbEntity.AddToCurrentDocument();
            sketchDetail3.AddObject(circle3.ID);

            SketchProfile profile3 = sketchDetail3.CreateProfile();

            profile3.AutoProcessExternalContours();
            ExtrudeFeature EF3 = Shaft.AddExtrudeFeature(
                profile3.ID,
                25,
                0,
                FeatureExtentDirection.Positive);
            EF3.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();

            //добавление отверстия
            PlanarSketch sketchDetail4 = Shaft.AddPlanarSketch();

            DbCircle circle4 = new DbCircle()
            {
                Center = new Point3d(0, 0, 115),
                Radius = 15,
            };

            // получаем конечные грани выдавливания
            // в нашем построении это всегда одна грань, в общем случае их там может быть много
            List<McObjectId> endFacesIds3 = EF3.GetEndFEV(EntityGeomType.kSurfaceEntities);
            sketchDetail4.PlanarEntityID = endFacesIds3[0];
            sketchDetail4.DbEntity.Visibility = 0;


            circle4.DbEntity.AddToCurrentDocument();
            sketchDetail4.AddObject(circle4.ID);

            SketchProfile profile4 = sketchDetail4.CreateProfile();

            profile4.AutoProcessExternalContours();
            ExtrudeFeature EF4 = Shaft.AddExtrudeFeature(
                profile4.ID,
                115,
                0,
                FeatureExtentDirection.Negative);
            EF4.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();

            // добавление второго отверстия
            PlanarSketch sketchDetail5 =Shaft.AddPlanarSketch();
            //добавляем новую плоскость и строим на ней окружность
            Plane3d ps1_plane = new Plane3d(new Point3d(0, 0, 105), new Vector3d(0, 1, 0), new Vector3d(0, 0, 1)); // плоскость первого контура YZ (не XY)
            sketchDetail5.DbEntity.AddToCurrentDocument();

            DbCircle circle5 = new DbCircle()
            {
                Center = Point3d.Origin,
                Radius = 7
            };

            circle5.DbEntity.AddToCurrentDocument();
            sketchDetail5.AddObject(circle5.ID);
            sketchDetail5.DbEntity.Update();

            SketchProfile profile5 = sketchDetail5.CreateProfile();
            profile5.AutoProcessExternalContours();

            ExtrudeFeature EF5 = Shaft.AddExtrudeFeature(profile5.ID, 100, 0, FeatureExtentDirection.Symmetric);
            EF5.Operation = PartFeatureOperation.Cut;

            sketchDetail5.SetPlane(ps1_plane);
            McObjectManager.UpdateAll();
            
        }

    }
}
