using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Windows.Forms;
using Multicad;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
using Multicad.Mc3D;
using Multicad.Runtime;

namespace Loft
{
    // Класс для хранения полей ввода пользователя
    public static class Parametric
    {
        public static bool flagCmd1 = true;
        public static int height;
        public static int width;
        public static int radius;
    }

    [ContainsCommands]
    public class Commands
    {
        [CommandMethod("loft_param", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ShowParamDialog()
        {
            LoftForm form = new LoftForm();
            form.ShowDialog();
        }

        [CommandMethod("loft_3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void BuildLoft()
        {
            McDocument activeSheet = McDocumentsManager.GetActiveSheet();

            Mc3dSolid Loft = new Mc3dSolid();
            McObjectManager.Add2Document(Loft.DbEntity, activeSheet);

            // Квадратное основание
            PlanarSketch sketch = Loft.AddPlanarSketch();
            if (sketch != null)
            {
                Point3d[] points = new Point3d[] {
                    new Point3d(-Parametric.width / 2, -Parametric.width / 2, 0),
                    new Point3d(Parametric.width / 2, -Parametric.width / 2, 0),
                    new Point3d(Parametric.width / 2, Parametric.width / 2, 0),
                    new Point3d(-Parametric.width / 2, Parametric.width / 2, 0),
                    new Point3d(-Parametric.width / 2, -Parametric.width / 2, 0)
                };
                Polyline3d pline = new Polyline3d(points);

                DbPolyline dbPline = new DbPolyline();
                dbPline.Polyline = pline;
                dbPline.DbEntity.AddToCurrentDocument();
                sketch.AddObject(dbPline.ID);

                SketchProfile profile = sketch.CreateProfile();
                if (profile != null)
                {
                    profile.AutoProcessExternalContours();
                 
                    ExtrudeFeature EF1 = Loft.AddExtrudeFeature(profile.ID, 30, 0, FeatureExtentDirection.Negative);
                }
            }

            // Лофт
            PlanarSketch sketch1 = Loft.AddPlanarSketch();
            if (sketch1 != null)
            {
                DbCircle circleBase = new DbCircle()
                {
                    Center = new Point3d(0, 0, 0),
                    Radius = Parametric.radius
                };
                circleBase.DbEntity.AddToCurrentDocument();
                sketch1.AddObject(circleBase.ID);

                SketchProfile profile1 = sketch1.CreateProfile();
                if (profile1 != null)
                {
                    profile1.AutoProcessExternalContours();

                    Plane3d planeTop = new Plane3d(new Point3d(0, 0, Parametric.height), Vector3d.ZAxis);
                    PlanarSketch sketch2 = Loft.AddPlanarSketch();
                    sketch2.SetPlane(planeTop);
                    if (sketch2 != null)
                    {
                        DbCircle circleTop = new DbCircle()
                        {
                            Center = new Point3d(0, 0, Parametric.height),
                            Radius = 30
                        };
                        circleTop.DbEntity.AddToCurrentDocument();
                        sketch2.AddObject(circleTop.ID);

                        SketchProfile profile2 = sketch2.CreateProfile();
                        if (profile2 != null)
                        {
                            profile2.AutoProcessExternalContours();

                            List<McObjectId> profileIds = new List<McObjectId>() { profile2.ID };
                            LoftFeature LF1 = Loft.AddLoftFeature(profile1.ID, profileIds);

                            if (LF1 != null)
                            {
                                DbPolyline dbPlineForLoft = new Polyline3d(new List<Point3d>
                                {
                                    new Point3d(0, 0, 0),
                                    new Point3d(0, 0, Parametric.height)
                                });
                                dbPlineForLoft.DbEntity.AddToCurrentDocument();
                                McGeomParam center = new McGeomParam() { ID = dbPlineForLoft.ID };
                                LF1.LoftType = LoftType.WithCenterLine;
                                LF1.Path = center;
                            }
                        }
                    }
                }
            }

            // Отверстие
            PlanarSketch sketch3 = Loft.AddPlanarSketch();
            if (sketch3 != null)
            {
                DbCircle circleHole = new DbCircle()
                {
                    Center = new Point3d(0, 0, 0),
                    Radius = 20
                };
                circleHole.DbEntity.AddToCurrentDocument();
                sketch3.AddObject(circleHole.ID);

                SketchProfile profile3 = sketch3.CreateProfile();
                if (profile3 != null )
                {
                    profile3.AutoProcessExternalContours();

                    ExtrudeFeature EF2 = Loft.AddExtrudeFeature(profile3.ID, 2000, 0, FeatureExtentDirection.Symmetric);
                    EF2.PartOperation = PartFeatureOperation.Cut;
                }

                McObjectManager.UpdateAll();
            }
        }

        public static void cmd_Loft()
        {
            McContext.ExecuteCommand("loft_3d");
        }
    }
}
