using System;
using System.Collections.Generic;
using Multicad;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
using Multicad.Mc3D;
using Multicad.Runtime;

namespace Create_Flange
{
    [ContainsCommands]
    public class CreateFlange
    {

        [CommandMethod("create_flange", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public static void flange()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet();
            Mc3dSolid solid = new Mc3dSolid();
            RevolveFeature RF1 = new RevolveFeature();
            solid = RF1.Cast<Mc3dSolid>();
            bool addingSolidResult1 = McObjectManager.Add2Document(solid.DbEntity, activeSheet);

            solid.DbEntity.AddToCurrentDocument();
            PlanarSketch ps1 = solid.AddPlanarSketch();
            Plane3d testPlane = new Plane3d(new Point3d(0, 0, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, 1));

            Polyline3d poly3d = new Polyline3d(new List<Point3d>() {
                    new Point3d(46.5, 0, 0), new Point3d(97, 0, 0), new Point3d(97, 21, 0),
                    new Point3d(70.5, 21, 0), new Point3d(70.5, 25, 0), new Point3d(46.5, 25, 0),
                    new Point3d(46.5, 0, 0)
            });
            poly3d.Vertices.MakeChamferAtVertex(0, 3);
            poly3d.Vertices.MakeChamferAtVertex(5, 4);
            DbPolyline poly = new DbPolyline()
            {
                Polyline = poly3d
            };

            poly.DbEntity.AddToCurrentDocument();
            ps1.AddObject(poly.ID);


            DbLine axisLine = new DbLine() { Line = new LineSeg3d(new Point3d(0, 0, 0), new Point3d(0, 20, 0)) };
            axisLine.DbEntity.AddToCurrentDocument();
            McGeomParam axisGP = new McGeomParam() { ID = axisLine.ID };
            ps1.AddObject(axisLine.ID);

            ps1.SetPlane(testPlane);

            SketchProfile profile1 = ps1.CreateProfile();
            profile1.AutoProcessExternalContours();
            RF1.ProfileID = profile1.ID;
            RF1.Axis = axisGP;
            RF1.Angle = 360;
            McObjectManager.UpdateAll();
            ps1.DbEntity.AddToCurrentDocument();
            ps1.DbEntity.Visibility = 0;
            profile1.DbEntity.Visibility = 0;
            PlanarSketch ps2 = solid.AddPlanarSketch();
            ps2.DbEntity.AddToCurrentDocument();

            DbCircle circle2 = new DbCircle()
            {
                Center = new Point3d(80.5, 0, 21),
                Radius = 9,
            };
            List<McObjectId> FacesIds0 = RF1.GetFEV(EntityGeomType.kPlaneSegment);
            ps2.PlanarEntityID = FacesIds0[2];

            circle2.DbEntity.AddToCurrentDocument();
            ps2.AddObject(circle2.ID);
            ps2.DbEntity.Visibility = 0;

            SketchProfile profile2 = ps2.CreateProfile();

            profile2.AutoProcessExternalContours();
            profile2.DbEntity.Visibility = 0;

            ExtrudeFeature EF1 = solid.AddExtrudeFeature(profile2.ID, 60, 0, FeatureExtentDirection.Negative);
            EF1.Operation = PartFeatureOperation.Cut;

            //Фаска не строится
            //???????????????????
            //List<McObjectId> endFacesIds1 = EF1.GetEndFEV(EntityGeomType.kCircArc);
            //List<McObjectId> endFaceEdgesIds = Service.GetLinkedFEVsToObject(endFacesIds1[0], EntityGeomType.kAllEntities, false);
            //McObjectId endEdgeID = endFaceEdgesIds[0];

            //ChamferFeature CF1 = solid.AddChamferFeature(new McObjectId[] { endEdgeID }, 2);
            //CF1.DbEntity.AddToCurrentDocument();
            //???????????????????
            CircularPatternFeature circArray = solid.AddCircularPatternFeature(new McObjectId[] { EF1.ID }, axisGP, 4, 1.57079631345);
            circArray.DbEntity.AddToCurrentDocument();

            McObjectManager.UpdateAll();
        }

    }
}
