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

namespace Detail1
{
    [ContainsCommands]
    public class Detail1
    {
        [CommandMethod("detail1_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Sample3d()
        {

            var activeSheet = McDocumentsManager.GetActiveSheet();

            Mc3dSolid Detail3d = new Mc3dSolid();


            ExtrudeFeature EF1 = new ExtrudeFeature();
            Detail3d = EF1.Cast<Mc3dSolid>();

            bool addingSolidResult1 = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument();
            PlanarSketch sketchDetail = Detail3d.AddPlanarSketch();

            DbPolyline rect1 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(300, 0, 0), new Point3d(300, 110, 0), new Point3d(0, 110, 0), new Point3d(0, 0, 0) }) };

            rect1.DbEntity.AddToCurrentDocument();
            sketchDetail.AddObject(rect1.ID);
            SketchProfile profile1 = sketchDetail.CreateProfile();
            if (profile1 != null)
            {
                profile1.AutoProcessExternalContours();
                EF1.ProfileID = profile1.ID;
                EF1.Distance = 80;
                EF1.Angle = 0;
                EF1.Operation = PartFeatureOperation.Join;
                EF1.Direction = FeatureExtentDirection.Positive;
                McObjectManager.UpdateAll();
            }


            PlanarSketch sketchDetail2 = Detail3d.AddPlanarSketch();

            ExtrudeFeature EF2 = new ExtrudeFeature();

            DbPolyline rect2 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(50, 0, 0), new Point3d(250, 0, 0), new Point3d(250, 50, 0), new Point3d(50, 50, 0), new Point3d(50, 0, 0) }) };

            rect2.DbEntity.AddToCurrentDocument();
            sketchDetail2.AddObject(rect2.ID);
            SketchProfile profile2 = sketchDetail2.CreateProfile();
            profile2.AutoProcessExternalContours();

            EF2 = Detail3d.AddExtrudeFeature(
                profile2.ID,
                80,
                0,
                FeatureExtentDirection.Positive);
            EF2.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();




            PlanarSketch sketchDetail3 = Detail3d.AddPlanarSketch();



            DbPolyline rect3 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 110, 0), new Point3d(300, 110, 0), new Point3d(300, 180, 0), new Point3d(0, 180, 0), new Point3d(0, 0, 0) }) };


            rect3.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF3 = new ExtrudeFeature();


            sketchDetail3.AddObject(rect3.ID);
            SketchProfile profile3 = sketchDetail3.CreateProfile();
            profile3.AutoProcessExternalContours();

            EF3 = Detail3d.AddExtrudeFeature(
                profile3.ID,
                280,
                0,
                FeatureExtentDirection.Positive);
            EF3.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();





            PlanarSketch sketchDetail4 = Detail3d.AddPlanarSketch();

            DbPolyline rect4 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 180, 0), new Point3d(300, 180, 0), new Point3d(300, 480, 0), new Point3d(0, 480, 0), new Point3d(0, 180, 0) }) };


            rect4.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF4 = new ExtrudeFeature();


            sketchDetail4.AddObject(rect4.ID);
            SketchProfile profile4 = sketchDetail4.CreateProfile();
            profile4.AutoProcessExternalContours();

            EF4 = Detail3d.AddExtrudeFeature(
                profile4.ID,
                280,
                0,
                FeatureExtentDirection.Positive);
            EF4.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();


            PlanarSketch sketchDetail5 = Detail3d.AddPlanarSketch();

            DbPolyline rect5 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 180, 0), new Point3d(300, 180, 0), new Point3d(300, 480, 0), new Point3d(0, 480, 0), new Point3d(0, 180, 0) }) };


            rect5.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF5 = new ExtrudeFeature();


            sketchDetail5.AddObject(rect5.ID);
            SketchProfile profile5 = sketchDetail5.CreateProfile();
            profile5.AutoProcessExternalContours();

            EF5 = Detail3d.AddExtrudeFeature(
                profile5.ID,
                200,
                0,
                FeatureExtentDirection.Positive);
            EF5.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();



            PlanarSketch sketchDetail6 = Detail3d.AddPlanarSketch();

            DbCircle circle1 = new DbCircle()
            {
                Center = new Point3d(150, 330, 0),
                Radius = 80,
            };



            circle1.DbEntity.AddToCurrentDocument();
            sketchDetail6.AddObject(circle1.ID);
            SketchProfile profile6 = sketchDetail6.CreateProfile();

            profile6.AutoProcessExternalContours();
            ExtrudeFeature EF6 = new ExtrudeFeature();
            EF6 = Detail3d.AddExtrudeFeature(
                profile6.ID,
                280,
                0,
                FeatureExtentDirection.Positive);
            EF6.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();

            PlanarSketch sketchDetail7 = Detail3d.AddPlanarSketch();

            DbPolyline rect6 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 480, 0), new Point3d(300, 480, 0), new Point3d(300, 550, 0), new Point3d(0, 550, 0), new Point3d(0, 480, 0) }) };


            rect6.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF7 = new ExtrudeFeature();


            sketchDetail7.AddObject(rect6.ID);
            SketchProfile profile7 = sketchDetail7.CreateProfile();
            profile7.AutoProcessExternalContours();

            EF7 = Detail3d.AddExtrudeFeature(
                profile7.ID,
                280,
                0,
                FeatureExtentDirection.Positive);
            EF7.Operation = PartFeatureOperation.Join;
            McObjectManager.UpdateAll();


            PlanarSketch sketchDetail8 = Detail3d.AddPlanarSketch();

            DbPolyline rect7 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(0, 480, 0), new Point3d(300, 480, 0), new Point3d(300, 660, 0), new Point3d(0, 660, 0), new Point3d(0, 480, 0) }) };


            rect7.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF8 = new ExtrudeFeature();


            sketchDetail8.AddObject(rect7.ID);
            SketchProfile profile8 = sketchDetail8.CreateProfile();
            profile8.AutoProcessExternalContours();

            EF8 = Detail3d.AddExtrudeFeature(
                profile8.ID,
                80,
                0,
                FeatureExtentDirection.Positive);
            EF8.Operation = PartFeatureOperation.Join;


            PlanarSketch sketchDetail9 = Detail3d.AddPlanarSketch();

            DbPolyline rect8 = new DbPolyline() { Polyline = new Polyline3d(new List<Point3d>() { new Point3d(50, 610, 0), new Point3d(250, 610, 0), new Point3d(250, 660, 0), new Point3d(50, 660, 0), new Point3d(50, 610, 0) }) };


            rect8.DbEntity.AddToCurrentDocument();
            ExtrudeFeature EF9 = new ExtrudeFeature();


            sketchDetail9.AddObject(rect8.ID);
            SketchProfile profile9 = sketchDetail9.CreateProfile();
            profile9.AutoProcessExternalContours();

            EF9 = Detail3d.AddExtrudeFeature(
                profile9.ID,
                80,
                0,
                FeatureExtentDirection.Positive);
            EF9.Operation = PartFeatureOperation.Cut;
            McObjectManager.UpdateAll();


        }
    }
}
