using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Mc3dSolid solid = new Mc3dSolid(); // создание пустого солида
            bool addingSolidResult = McObjectManager.Add2Document(solid.DbEntity, activeSheet); //

            PlanarSketch sketch = solid.AddPlanarSketch();

            if (sketch != null)
            {
                DbPolyline poline = new DbPolyline()
                {
                    Polyline = new Polyline3d(new List<Point3d>//рисуем эскиз линиями по точкам
                    { 
                      new Point3d(-Parametric.width/2, -Parametric.width/2, 0),
                      new Point3d(Parametric.width/2, -Parametric.width/2, 0),
                      new Point3d(Parametric.width/2, Parametric.width/2, 0),
                      new Point3d(-Parametric.width/2, Parametric.width/2, 0),
                      new Point3d(-Parametric.width/2, -Parametric.width/2, 0)
                    })
                };

                poline.DbEntity.AddToCurrentDocument();
                sketch.AddObject(poline.ID);

                SketchProfile profile = sketch.CreateProfile();
                if (profile != null)
                {
                    profile.AutoProcessExternalContours();

                    ExtrudeFeature extrude = solid.AddExtrudeFeature(profile.ID, 30, 0, FeatureExtentDirection.Negative);
                }

            } 
;
            PlanarSketch sketch1 = solid.AddPlanarSketch();

            if (sketch1 != null)
            {
                DbCircle circle = new DbCircle()
                {
                    Center = new Point3d(0, 0, 0),
                    Radius = Parametric.radius
                };
                circle.DbEntity.AddToCurrentDocument();
                sketch1.AddObject(circle.ID);

                SketchProfile profile1 = sketch1.CreateProfile();
                if (profile1 != null)
                {
                    profile1.AutoProcessExternalContours();

                    DbCircle circle1 = new DbCircle()
                    {
                        Center = new Point3d(0, 0, Parametric.height),
                        Radius = 30
                    };
                    circle1.DbEntity.AddToCurrentDocument();

                    McGeomParam section = new McGeomParam()
                    {
                        ID = circle1.ID
                    };

                    LoftFeature loft = solid.AddLoftFeature(profile1.ID, new McGeomParam[] { section});

                    DbPolyline polyline1 = new Polyline3d(new List<Point3d>
                    {
                        new Point3d(0, 0, 0),
                        new Point3d(0, 0, Parametric.height)
                    });
                    polyline1.DbEntity.AddToCurrentDocument();
                    McGeomParam center = new McGeomParam() { ID = polyline1.ID };
                    loft.LoftType = LoftType.WithCenterLine;
                    loft.Path = center;
                }

                PlanarSketch sketch2 = solid.AddPlanarSketch();

                if (sketch1 != null)
                {
                    DbCircle circle2 = new DbCircle()
                    {
                        Center = new Point3d(0, 0, 0),
                        Radius = 20
                    };
                    circle2.DbEntity.AddToCurrentDocument();
                    sketch2.AddObject(circle2.ID);

                    SketchProfile profile2 = sketch2.CreateProfile();
                    if (profile2 != null)
                    {
                        profile2.AutoProcessExternalContours();

                        ExtrudeFeature extrude1 = solid.AddExtrudeFeature(profile2.ID, 1000, 0, FeatureExtentDirection.Symmetric);
                        extrude1.Operation = PartFeatureOperation.Cut;
                    }

                }


                solid = null;

            }

        }

        public static void cmd_Loft()
        {
            McContext.ExecuteCommand("build");
        }
    }
}