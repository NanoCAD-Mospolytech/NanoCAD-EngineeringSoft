using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Multicad;
using Multicad.AplicationServices;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
using Multicad.Mc3D;
using Multicad.Objects;
using Multicad.Runtime;
using Multicad.Symbols;
using Multicad.Symbols.Tables;
using Multicad.Text;
using static System.Collections.Specialized.BitVector32;

namespace MultiCAD
{
    [ContainsCommands]
    public class Class1
    {
        [CommandMethod("MCreateLineByTwoPoints", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void MCreateLineByTwoPoints()
        {
            // Создание начальной точки с координатами (0,0,0)
            Point3d startPoint = new Point3d(0, 0, 0);

            // Создание конечной точки с координатами (100,50,0)
            Point3d endPoint = new Point3d(100, 50, 0);

            // Создание объекта линии
            DbLine line = new DbLine();

            // Установка начальной и конечной точек для линии
            line.Set(startPoint, endPoint);

            // Установка слоя для линии
            line.DbEntity.Layer = "Lines";

            // Установка цвета линии
            line.DbEntity.Color = System.Drawing.Color.Red;

            // Добавление линии в текущий документ
            line.DbEntity.AddToCurrentDocument();
        }

        [CommandMethod("MCreateLineByPointAndAngle", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void MCreateLineByPointAndAngle()
        {
            // Создание начальной точки с координатами (0,0,0)
            Point3d startPoint = new Point3d(0, 0, 0);

            // Задание длины отрезка
            double length = 100;

            // Задание угла отрезка в радианах
            double angle = Math.PI / 4; // 45 градусов

            // Вычисление координат конечной точки
            Point3d endPoint = new Point3d(
                startPoint.X + length * Math.Cos(angle),
                startPoint.Y + length * Math.Sin(angle),
                0);

            // Создание объекта линии
            DbLine line = new DbLine();

            // Установка начальной и конечной точек для линии
            line.Set(startPoint, endPoint);

            // Установка слоя для линии
            line.DbEntity.Layer = "Lines";

            // Установка цвета линии
            line.DbEntity.Color = System.Drawing.Color.Yellow;

            // Добавление линии в текущий документ
            line.DbEntity.AddToCurrentDocument();
        }

        [CommandMethod("MCreateLineByLengthAndDirection", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void MCreateLineByLengthAndDirection()
        {
            // Создание начальной точки с координатами (0,0,0)
            Point3d startPoint = new Point3d(0, 0, 0);

            // Задание длины отрезка
            double length = 100;

            // Создание вектора направления
            Vector3d direction = new Vector3d(1, 1, 0);

            // Нормализация вектора
            direction = direction.GetNormal();

            // Вычисление координат конечной точки
            Point3d endPoint = startPoint + direction * length;

            // Создание объекта линии
            DbLine line = new DbLine();

            // Установка начальной и конечной точек для линии
            line.Set(startPoint, endPoint);

            // Установка слоя для линии
            line.DbEntity.Layer = "Lines";

            // Установка цвета линии
            line.DbEntity.Color = System.Drawing.Color.Green;

            // Добавление линии в текущий документ
            line.DbEntity.AddToCurrentDocument();
        }

        [CommandMethod("MCreateCircle", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void MCreateCircle()
        {
            // Создание центра окружности
            Point3d center = new Point3d(0, 0, 0);

            // Задание радиуса окружности
            double radius = 50;

            // Создание окружности
            DbCircle circle = new DbCircle();
            circle.Center = center;
            circle.Radius = radius;

            // Установка слоя для окружности
            circle.DbEntity.Layer = "Circles";

            // Установка цвета окружности
            circle.DbEntity.Color = System.Drawing.Color.Green;

            // Добавление окружности в текущий документ
            circle.DbEntity.AddToCurrentDocument();
        }

        // Пример с установкой дополнительных параметров линии
        [CommandMethod("MCreateCircleWithParams", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void MCreateCircleWithParams()
        {
            // Создание центра окружности
            Point3d center = new Point3d(0, 0, 0);

            // Задание радиуса окружности
            double radius = 50;

            // Создание окружности
            DbCircle circle = new DbCircle();
            circle.Center = center;
            circle.Radius = radius;

            // Установка слоя для окружности
            circle.DbEntity.Layer = "Circles";

            // Установка цвета окружности
            circle.DbEntity.Color = System.Drawing.Color.Blue;

            // Установка веса линии
            circle.DbEntity.LineWeight = 5;

            // Установка типа линии
            circle.DbEntity.LineType = 2;

            // Установка масштаба линии
            circle.DbEntity.LineTypeScale = 2.0;

            // Добавление окружности в текущий документ
            circle.DbEntity.AddToCurrentDocument();
        }

        [CommandMethod("Arc3pts", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void Arc3pts()
        {
            Point3d pt1 = new Point3d(0, 0, 0); // Начальная точка дуги
            Point3d pt2 = new Point3d(50, 50, 0); // Точка на центральной части дуги
            Point3d pt3 = new Point3d(100, 0, 0); // Конечная точка дуги
            CircArc3d arc = new CircArc3d(pt1,pt2,pt3); // Создание дуги по 3 точкам
            // Создаем объект типа DBCircArc на основе CircArc3d
            DbCircArc dbarc = new DbCircArc()
            {
                Arc = arc
            };
            dbarc.DbEntity.AddToCurrentDocument(); // Добавляем дугу в текущий документ
        }

        [CommandMethod("ArcCenterVecNorm", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ArcCenterVecNorm()
        {
            Point3d center = new Point3d(0, 0, 0); // Центр дуги
            double radius = 10; // Радиус Дуги
            Vector3d normal = new Vector3d(1,0,0); // Вектор нормали вдоль оси х 
            CircArc3d arc = new CircArc3d(center,normal,radius); // Создание дуги по 3 точкам
            // Создаем объект типа DBCircArc на основе CircArc3d
            DbCircArc dbarc = new DbCircArc()
            {
                Arc = arc
            };
            dbarc.DbEntity.AddToCurrentDocument(); // Добавляем дугу в текущий документ
        }
        [CommandMethod("ArcCenterVecNormRefVecStAngEndAng", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ArcCenterVecNormRefVecStAngEndAng()
        {

            Point3d center = new Point3d(0, 0, 0); // Центр дуги
            double radius = 10; // Радиус Дуги
            Vector3d normal = new Vector3d(0, 0, 1); // Вектор нормали вдоль оси х 
            Vector3d refVector = new Vector3d(1, 0, 0); // Задаем направляющий вектор
            double startAngle = Math.PI / 4; // Задаем начальный угол дуги
            double endAngle = 5 * Math.PI / 4; // Задаем конечный угол дуги
            CircArc3d arc = new CircArc3d(center,normal,refVector,radius, startAngle, endAngle); // Создание дуги по 3 точкам
            // Создаем объект типа DBCircArc на основе CircArc3d
            DbCircArc dbarc = new DbCircArc()
            {
                Arc = arc
            };
            dbarc.DbEntity.AddToCurrentDocument(); // Добавляем дугу в текущий документ
        }
        [CommandMethod("ArcWithoutCircArc3d", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ArcWithoutCircArc3d()
        {
            Point3d center = new Point3d(0, 0, 0); // Центр дуги
            double radius = 10; // Радиус Дуги
            // Создаем объект типа DBCircArc на основе радиуса и центра
            DbCircArc dbarc = new DbCircArc()
            {
                Center = center,
                Radius = radius
            };
            dbarc.DbEntity.AddToCurrentDocument(); // Добавляем дугу в текущий документ
        }
        [CommandMethod("ArcMethods", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ArcMethods()
        {
            Point3d pt1 = new Point3d(0, 0, 0); // Начальная точка дуги
            Point3d pt2 = new Point3d(50, 50, 0); // Точка на центральной части дуги
            Point3d pt3 = new Point3d(100, 0, 0); // Конечная точка дуги
            CircArc3d arc = new CircArc3d(pt1, pt2, pt3); // Создание дуги по 3 точкам
            double startAngle = Math.PI / 4; // Задаем начальный угол дуги
            double endAngle = 5 * Math.PI / 4; // Задаем конечный угол дуги

            // Создаем объект типа DBCircArc на основе CircArc3d
            DbCircArc dbarc = new DbCircArc()
            {
                Arc = arc
            };
            dbarc.DbEntity.AddToCurrentDocument(); // Добавляем дугу в текущий документ
            CircArc3d arc2 = dbarc.Arc; // Получаем объект CircArc3d 
            double radius2 = dbarc.Radius; // Получаем радиус
            Point3d center2 = dbarc.Center; // Получаем точку-центр

            dbarc.Arc = arc2; // Устанавливаем в параметр Arc объект CircArc3d
            dbarc.Radius = radius2; // Устанавливаем радиус
            dbarc.Center = center2; // Устанавливаем точку-центр

            dbarc.Set(center2, radius2, startAngle, endAngle); // Устанавливаем параметры дуги
            dbarc.DbEntity.ID.ToGuid(); // Получаем айди дуги.
            dbarc.DbEntity.Color = System.Drawing.Color.Red; // Устанавливаем красный цвет для дуги
            dbarc.DbEntity.LineWeight = 5; // Вес линии
            dbarc.DbEntity.LineType = 2; // Тип линии
            dbarc.DbEntity.LineTypeScale = 2;  // Масштаб линии
            Plane3d plane;
            dbarc.DbEntity.GetPlane(out plane);
        }
        [CommandMethod("SketchOffsetPlane", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void SketchOffsetPlane()
        {
            Plane3d basePlane = new Plane3d(Point3d.Origin, Vector3d.ZAxis); // Создаем базовую плоскость XY
            double offset = 100; // Задаем смещение
            // Создаем смещенную плоскость
            Point3d newOrigin = basePlane.Origin + basePlane.Normal * offset;
            Plane3d offsetPlane = new Plane3d(newOrigin, basePlane.Normal);

            PlanarSketch sketch1 = new PlanarSketch(); // Создаем эскиз
            sketch1.SetPlane(offsetPlane); // Устанавливаем для эскиза смещенную плоскость

            DbCircle circle = new DbCircle(); // Создаем окружность
            circle.Radius = 100; // Устанавливаем радиус окружности
            circle.Center = new Point3d(0, 0, 0); // Устанавливаем центр окружности

            circle.DbEntity.AddToCurrentDocument(); // Добавляем окружность в документ

            sketch1.AddObject(circle.ID); // Добавляем окружность в эскиз

            sketch1.DbEntity.AddToCurrentDocument(); // Добавляем эскиз в документ
        }
        [CommandMethod("ExtrudeSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ExtrudeSample()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); 
            Mc3dSolid Detail3d = new Mc3dSolid();

            ExtrudeFeature EF1 = new ExtrudeFeature();
            Detail3d = EF1.Cast<Mc3dSolid>();

            bool addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument();
            PlanarSketch sketch1 = Detail3d.AddPlanarSketch();

            DbPolyline pline1 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-10, 10, 0),
                    new Point3d(10, 10, 0),
                    new Point3d(10, -10, 0),
                    new Point3d(-10, -10, 0),
                    new Point3d(-10, 10, 0)
                })
            };
            pline1.DbEntity.AddToCurrentDocument();
            sketch1.AddObject(pline1.ID);
            SketchProfile profile1 = sketch1.CreateProfile();
            profile1.AutoProcessExternalContours();
            EF1.ProfileID = profile1.ID;
            EF1.Distance = 20;
            EF1.Angle = 0;
            EF1.PartOperation = PartFeatureOperation.Join;
            EF1.Direction = FeatureExtentDirection.Positive;
            McObjectManager.UpdateAll();
        }

        [CommandMethod("RevolveSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void RevolveSample()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet();
            Mc3dSolid Detail3d = new Mc3dSolid();

            RevolveFeature RF1 = new RevolveFeature();
            Detail3d = RF1.Cast<Mc3dSolid>();

            bool addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument();
            PlanarSketch sketch1 = Detail3d.AddPlanarSketch();

            DbPolyline pline1 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-50, 10, 0),
                    new Point3d(-10, 10, 0),
                    new Point3d(-10, -10, 0),
                    new Point3d(-50, -10, 0),
                    new Point3d(-50, 10, 0)
                })
            };
            DbLine Axis = new DbLine()
            {
                StartPoint = new Point3d(0, 0, 0),
                EndPoint = new Point3d(0, 100, 0)

            };

            Axis.DbEntity.AddToCurrentDocument();

            pline1.DbEntity.AddToCurrentDocument();
            sketch1.AddObject(pline1.ID);
            SketchProfile profile1 = sketch1.CreateProfile();
            profile1.AutoProcessExternalContours();
            RF1.ProfileID = profile1.ID;
            RF1.Angle = 2 * Math.PI;
            RF1.PartOperation = PartFeatureOperation.Join;
            RF1.Direction = FeatureExtentDirection.Positive;
            RF1.Axis = new McGeomParam() {
                ID = Axis.ID
            };
            
            McObjectManager.UpdateAll();
        }

        [CommandMethod("LoftExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void LoftExample()
        {
            // Получение активного документа
            var activeSheet = McDocumentsManager.GetActiveSheet();

            // Создание объекта 3D-геометрии и объекта Loft
            Mc3dSolid Detail3d = new Mc3dSolid();
            LoftFeature LF1 = new LoftFeature();

            // Приведение объекта Loft к типу Mc3dSolid
            Detail3d = LF1.Cast<Mc3dSolid>();

            // Настройка параметров Loft
            LF1.Closed = true;
            LF1.LoftType = LoftType.Regular;
            LF1.PartOperation = PartFeatureOperation.Join;

            // Добавление 3D-объекта Loft в документ
            McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument();

            // Создание первой плоскости для сечения
            Plane3d plane1 = new Plane3d(new Point3d(0, 0, 0), Vector3d.ZAxis);

            // Создание первого эскиза и окружности для сечения
            PlanarSketch sketch1 = new PlanarSketch();
            sketch1.SetPlane(plane1);
            sketch1.DbEntity.AddToCurrentDocument();

            DbCircle circle1 = new DbCircle();
            circle1.Center = new Point3d(0, 0, 0);
            circle1.Radius = 50;
            circle1.DbEntity.AddToCurrentDocument();

            sketch1.AddObject(circle1.ID);

            // Создание второй плоскости для сечения
            Plane3d plane2 = new Plane3d(new Point3d(0, 0, 120), Vector3d.ZAxis);

            // Создание второго эскиза и окружности для сечения
            PlanarSketch sketch2 = new PlanarSketch();
            sketch2.SetPlane(plane2);
            sketch2.DbEntity.AddToCurrentDocument();

            DbCircle circle2 = new DbCircle();
            circle2.Center = new Point3d(0, 0, 120);
            circle2.Radius = 25;
            circle2.DbEntity.AddToCurrentDocument();

            sketch2.AddObject(circle2.ID);

            // Создание профилей для Loft
            SketchProfile profile1 = sketch1.CreateProfile();
            profile1.AutoProcessExternalContours();

            SketchProfile profile2 = sketch2.CreateProfile();
            profile2.AutoProcessExternalContours();

            // Добавление сечений в Loft
            LF1.AddSection(profile1.ID);
            LF1.AddSection(profile2.ID);

            // Обновление документа
            McObjectManager.UpdateAll();
        }

        [CommandMethod("FilletSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void FilletSample()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); // Получаем активную страницу
            Mc3dSolid Detail3d = new Mc3dSolid(); // Создаем объект тела
            ExtrudeFeature EF1 = new ExtrudeFeature(); // Создаем выдавливание
            Detail3d = EF1.Cast<Mc3dSolid>(); // Добавляем выдавливание в тело
            bool addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet); 
            Detail3d.DbEntity.AddToCurrentDocument(); // Добавляем в документ

            PlanarSketch sketch1 = Detail3d.AddPlanarSketch(); // Создаем эскиз
           
            //Создаем полилинию
            DbPolyline pline1 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-10, 10, 0),
                    new Point3d(10, 10, 0),
                    new Point3d(10, -10, 0),
                    new Point3d(-10, -10, 0),
                    new Point3d(-10, 10, 0)
                })
            };
            pline1.DbEntity.AddToCurrentDocument(); // Добавляем в документ
            sketch1.AddObject(pline1.ID); // Добавляем в эскиз
            SketchProfile profile1 = sketch1.CreateProfile(); // Создаем эскизный профиль
            profile1.AutoProcessExternalContours();

            EF1.ProfileID = profile1.ID; // Задаем эскизный профиль для выдавливания
            EF1.Distance = 20; // Задаем дистанцию выдавливания
            EF1.Angle = 0; // Задаем угол
            EF1.PartOperation = PartFeatureOperation.Join; // Выбираем операцию
            EF1.Direction = FeatureExtentDirection.Positive; // Выбираем направление

            McObjectManager.UpdateAll(); // Перерисовка объектов
            List<McObjectId> ids = EF1.GetFEV(EntityGeomType.kLine); // Получаем все ребра объекта
            Detail3d.AddFilletFeature(ids, 5); // Создаем скругление на полученных ребрах
        }
        [CommandMethod("ChamferSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void ChamferSample()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); // Получаем активную страницу
            Mc3dSolid Detail3d = new Mc3dSolid(); // Создаем объект тела
            ExtrudeFeature EF1 = new ExtrudeFeature(); // Создаем выдавливание
            Detail3d = EF1.Cast<Mc3dSolid>(); // Добавляем выдавливание в тело
            bool addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            Detail3d.DbEntity.AddToCurrentDocument(); // Добавляем в документ

            PlanarSketch sketch1 = Detail3d.AddPlanarSketch(); // Создаем эскиз
            //Создаем полилинию
            DbPolyline pline1 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-10, 10, 0),
                    new Point3d(10, 10, 0),
                    new Point3d(10, -10, 0),
                    new Point3d(-10, -10, 0),
                    new Point3d(-10, 10, 0)
                })
            };
            pline1.DbEntity.AddToCurrentDocument(); // Добавляем в документ
            sketch1.AddObject(pline1.ID); // Добавляем в эскиз
            SketchProfile profile1 = sketch1.CreateProfile(); // Создаем эскизный профиль
            profile1.AutoProcessExternalContours();

            EF1.ProfileID = profile1.ID; // Задаем эскизный профиль для выдавливания
            EF1.Distance = 20; // Задаем дистанцию выдавливания
            EF1.Angle = 0; // Задаем угол
            EF1.PartOperation = PartFeatureOperation.Join; // Выбираем операцию
            EF1.Direction = FeatureExtentDirection.Positive; // Выбираем направление

            McObjectManager.UpdateAll(); // Перерисовка объектов
            List<McObjectId> ids = EF1.GetFEV(EntityGeomType.kLine); // Получаем все ребра объекта
            Detail3d.AddChamferFeature(ids, 5); // Создаем фаски на полученных ребрах
        }

        [CommandMethod("TestDetail", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
        public void TestDetail()
        {
            var activeSheet = McDocumentsManager.GetActiveSheet(); 
            Mc3dSolid Detail3d = new Mc3dSolid(); 
            ExtrudeFeature EF1 = new ExtrudeFeature(); 
            Detail3d = EF1.Cast<Mc3dSolid>(); 
            bool addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);       
            Detail3d.DbEntity.AddToCurrentDocument(); 
            PlanarSketch sketch1 = new PlanarSketch();
            Plane3d basePlane = new Plane3d(Point3d.Origin, -Vector3d.YAxis);
            sketch1.SetPlane(basePlane);
            sketch1.DbEntity.AddToCurrentDocument(); 
            DbPolyline pline1 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-15, 0, 0),
                    new Point3d(-55, 0, 0),
                    new Point3d(-55, 0, 15),
                    new Point3d(55, 0, 15),
                    new Point3d(55, 0, 0),
                    new Point3d(15, 0, 0),
                    new Point3d(15, 0, 7.5),
                    new Point3d(-15, 0, 7.5),
                    new Point3d(-15, 0, 0)
                })
            };
            pline1.DbEntity.AddToCurrentDocument(); 
            sketch1.AddObject(pline1.ID); 
            SketchProfile profile1 = sketch1.CreateProfile(); 
            profile1.AutoProcessExternalContours();

            EF1.ProfileID = profile1.ID; 
            EF1.Distance = 40; 
            EF1.Angle = 0; 
            EF1.PartOperation = PartFeatureOperation.Join; 
            EF1.Direction = FeatureExtentDirection.Positive; 
            McObjectManager.UpdateAll();

            ExtrudeFeature EF2 = new ExtrudeFeature(); 
            Detail3d = EF2.Cast<Mc3dSolid>();
            addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);
            PlanarSketch sketch2 = new PlanarSketch();
            sketch2.SetPlane(basePlane);
            sketch2.DbEntity.AddToCurrentDocument(); 

            DbPolyline pline2 = new DbPolyline()
            {
                Polyline = new Polyline3d(new List<Point3d>
                {
                    new Point3d(-25, 0, 45),
                    new Point3d(-25, 0, 15),
                    new Point3d(25, 0, 15),
                    new Point3d(25, 0, 45)
                })
            };
            pline2.DbEntity.AddToCurrentDocument(); 
            sketch2.AddObject(pline2.ID); 

            DbCircArc dbArc = new DbCircArc()
            {
                Arc = new CircArc3d(new Point3d(-25, 0, 45), new Point3d(0, 0, 70), new Point3d(25, 0, 45))
            };
            dbArc.DbEntity.AddToCurrentDocument();
            sketch2.AddObject(dbArc.ID);
            SketchProfile profile2 = sketch2.CreateProfile(); 
            profile2.AutoProcessExternalContours();
            EF2.ProfileID = profile2.ID; 
            EF2.Distance = 30; 
            EF2.Angle = 0; 
            EF2.PartOperation = PartFeatureOperation.Join; 
            EF2.Direction = FeatureExtentDirection.Positive; 

            McObjectManager.UpdateAll();

            ExtrudeFeature EF3 = new ExtrudeFeature(); 
            Detail3d = EF3.Cast<Mc3dSolid>();
            addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);

            PlanarSketch sketch3 = new PlanarSketch();
            sketch3.DbEntity.AddToCurrentDocument();

            List<McObjectId> FacesIds0 = EF1.GetFEV(EntityGeomType.kPlaneSegment);
            sketch3.PlanarEntityID = FacesIds0[1];

            DbCircle dbCirc = new DbCircle()
            {
                Center = new Point3d(40, -20, 0),
                Radius = 7,
            };
            
            dbCirc.DbEntity.AddToCurrentDocument();
            sketch3.AddObject(dbCirc.ID);
            
            SketchProfile profile3 = sketch3.CreateProfile();
            profile3.AutoProcessExternalContours();
            EF3.ProfileID = profile3.ID; 
            EF3.Distance = 15; 
            EF3.Angle = 0;
            EF3.Direction = FeatureExtentDirection.Negative; 
            EF3.PartOperation = PartFeatureOperation.Cut; 
            McObjectManager.UpdateAll();

            ExtrudeFeature EF4 = new ExtrudeFeature();
            Detail3d = EF4.Cast<Mc3dSolid>();
            addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);

            PlanarSketch sketch4 = new PlanarSketch();
            sketch4.DbEntity.AddToCurrentDocument();
            sketch4.PlanarEntityID = FacesIds0[1];
            DbCircle dbCirc2 = new DbCircle()
            {
                Center = new Point3d(-40, -20, 0),
                Radius = 7,
            };

            dbCirc2.DbEntity.AddToCurrentDocument();
            sketch4.AddObject(dbCirc2.ID);

            SketchProfile profile4 = sketch4.CreateProfile(); 
            profile4.AutoProcessExternalContours();
            EF4.ProfileID = profile4.ID; 
            EF4.Distance = 15;
            EF4.Angle = 0;
            EF4.Direction = FeatureExtentDirection.Negative; 
            EF4.PartOperation = PartFeatureOperation.Cut;

            McObjectManager.UpdateAll();

            ExtrudeFeature EF5 = new ExtrudeFeature();
            Detail3d = EF5.Cast<Mc3dSolid>();
            addingSolidResult = McObjectManager.Add2Document(Detail3d.DbEntity, activeSheet);

            PlanarSketch sketch5 = new PlanarSketch();
            sketch5.DbEntity.AddToCurrentDocument();

            List<McObjectId> FacesIds1 = EF2.GetFEV(EntityGeomType.kPlaneSegment);
            sketch5.PlanarEntityID = FacesIds1[4]; 

            DbCircArc dbArc2 = new DbCircArc()
            {
                Arc = new CircArc3d(new Point3d(0, 0, 45), -Vector3d.YAxis, 12.5)
            };

            dbArc2.DbEntity.AddToCurrentDocument();
            sketch5.AddObject(dbArc2.ID);

            SketchProfile profile5 = sketch5.CreateProfile(); 
            profile5.AutoProcessExternalContours();
            EF5.ProfileID = profile5.ID; 
            EF5.Distance = 30; 
            EF5.Angle = 0; 
            EF5.Direction = FeatureExtentDirection.Negative; 
            EF5.PartOperation = PartFeatureOperation.Cut; 
            McObjectManager.UpdateAll();
        }
    }

}