using System;
using System.Collections.Generic;
using Multicad;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
using Multicad.Mc3D;
using Multicad.Runtime;

namespace MySample1
{
    [ContainsCommands]
    public class Sample3d
    {
        [CommandMethod("Param", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void myForm()
        {
            Form1 frm = new Form1();
            frm.Show();
        }

        [CommandMethod("build", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void Build3d()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet();

            Mc3dSolid solid = new Mc3dSolid();
            McObjectManager.Add2Document(solid.DbEntity, activeSheet);


            PlanarSketch sketchBase = solid.AddPlanarSketch();
            if (sketchBase != null)
            {
                DbPolyline square = new DbPolyline()
                {
                    Polyline = new Polyline3d(new List<Point3d>
                    {
                        new Point3d(-Parametric.width/2, -Parametric.width/2, 0),
                        new Point3d( Parametric.width/2, -Parametric.width/2, 0),
                        new Point3d( Parametric.width/2,  Parametric.width/2, 0),
                        new Point3d(-Parametric.width/2,  Parametric.width/2, 0),
                        new Point3d(-Parametric.width/2, -Parametric.width/2, 0)
                    })
                };
                square.DbEntity.AddToCurrentDocument();
                sketchBase.AddObject(square.ID);

                SketchProfile profileBase = sketchBase.CreateProfile();
                if (profileBase != null)
                {
                    profileBase.AutoProcessExternalContours();
                    solid.AddExtrudeFeature(profileBase.ID, 30, 0, FeatureExtentDirection.Negative);
                }
            }


            PlanarSketch sketchCircle1 = solid.AddPlanarSketch();
            SketchProfile profile1 = null;
            if (sketchCircle1 != null)
            {
                DbCircle circle1 = new DbCircle()
                {
                    Center = new Point3d(0, 0, 0),
                    Radius = Parametric.radius
                };
                circle1.DbEntity.AddToCurrentDocument();
                sketchCircle1.AddObject(circle1.ID);

                profile1 = sketchCircle1.CreateProfile();
                if (profile1 != null)
                    profile1.AutoProcessExternalContours();
            }


            PlanarSketch sketchCircle2 = solid.AddPlanarSketch();
            SketchProfile profile2 = null;
            if (sketchCircle2 != null)
            {
                Plane3d planeTop = new Plane3d(new Point3d(0, 0, Parametric.height), Vector3d.ZAxis);
                sketchCircle2.SetPlane(planeTop);

                DbCircle circle2 = new DbCircle()
                {
                    Center = new Point3d(0, 0, Parametric.height),
                    Radius = 30
                };
                circle2.DbEntity.AddToCurrentDocument();
                sketchCircle2.AddObject(circle2.ID);

                profile2 = sketchCircle2.CreateProfile();
                if (profile2 != null)
                    profile2.AutoProcessExternalContours();
            }


            if (profile1 != null && profile2 != null)
            {
                List<McObjectId> profiles = new List<McObjectId>() { profile2.ID };
                LoftFeature loft = solid.AddLoftFeature(profile1.ID, profiles);

                if (loft != null)
                {
                    DbPolyline path = new Polyline3d(new List<Point3d>
                    {
                        new Point3d(0, 0, 0),
                        new Point3d(0, 0, Parametric.height)
                    });
                    path.DbEntity.AddToCurrentDocument();

                    McGeomParam center = new McGeomParam() { ID = path.ID };
                    loft.LoftType = LoftType.WithCenterLine;
                    loft.Path = center;
                }
            }


            PlanarSketch sketchHole = solid.AddPlanarSketch();
            if (sketchHole != null)
            {
                DbCircle holeCircle = new DbCircle()
                {
                    Center = new Point3d(0, 0, 0),
                    Radius = 20
                };
                holeCircle.DbEntity.AddToCurrentDocument();
                sketchHole.AddObject(holeCircle.ID);

                SketchProfile profileHole = sketchHole.CreateProfile();
                if (profileHole != null)
                {
                    profileHole.AutoProcessExternalContours();
                    ExtrudeFeature cut = solid.AddExtrudeFeature(profileHole.ID, 2000, 0, FeatureExtentDirection.Symmetric);
                    cut.PartOperation = PartFeatureOperation.Cut;
                }
            }

            McObjectManager.UpdateAll();
        }

        public static void cmd_Loft()
        {
            McContext.ExecuteCommand("build");
        }
    }
}

