# Методическое пособие по MultiCAD API

## Содержание

- [Базовые команды 2D модуля](#базовые-команды-2d-модуля)
  - [Создание отрезков](#создание-отрезков)
  - [Создание дуг](#создание-дуг)
  - [Создание окружности](#создание-окружности)
  - [Создание эскиза в смещенной плоскости](#создание-эскиза-в-смещенной-плоскости)
- [Базовые команды 3D модуля](#базовые-команды-3d-модуля)
  - [Операция выдавливания](#операция-выдавливания)
  - [Операция выдавливания вращение](#операция-выдавливания-вращение)
  - [Операция выдавливания по траектории](#операция-выдавливания-по-траектории)
  - [Выдавливание по сечениям](#выдавливание-по-сечениям)
  - [Создание скруглений и фасок](#создание-скруглений-и-фасок)
- [Создание детали с использованием MultiCAD API](#создание-детали-с-использованием-multicad-api)

## Базовые команды 2D модуля

### Создание отрезков

1. Перед созданием отрезков подключим блок пространств имён:

```csharp
using Multicad;
using Multicad.Runtime;
using Multicad.DatabaseServices;
using Multicad.DatabaseServices.StandardObjects;
using Multicad.Geometry;
```

Пояснения к пространствам имен:
- **Multicad** — основное пространство имен для работы с MultiCAD API.
- **Multicad.Runtime** — используется для регистрации и выполнения команд в MultiCAD.
- **Multicad.DatabaseServices** — предоставляет классы для работы с объектами чертежа, такими как линии, полилинии и другие графические элементы.
- **Multicad.DatabaseServices.StandardObjects** — предоставляет доступ к стандартным объектам MultiCAD, таким как линии, полилинии и пр.
- **Multicad.Geometry** — содержит классы для работы с геометрией, включая точки, векторы, отрезки и другие примитивы.

2. Описание класса и метода:

```csharp
[ContainsCommands]
public partial class MultiCADSketch
{
    // Методы класса
}
```

- `[ContainsCommands]` — атрибут, указывающий, что класс содержит команды для выполнения в среде MultiCAD.
- `public partial class MultiCADSketch` — объявление класса, который будет содержать команды для работы с графикой (отрезками и полилиниями) в MultiCAD.

3. Создание отрезка между двумя фиксированными точками:

```csharp
[CommandMethod("MCreateLineByTwoPoints", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateLineByTwoPoints()
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
    line.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(1);
    
    // Добавление линии в текущий документ
    line.DbEntity.AddToCurrentDocument();
}
```

- `[CommandMethod("MCreateLineByTwoPoints", CommandFlags.NoCheck | CommandFlags.NoPrefix)]` — регистрирует метод как команду MultiCAD с именем MCreateLineByTwoPoints. Пользователь может вызвать её из командной строки программы.
- `Point3d startPoint = new Point3d(0, 0, 0)` — создается объект начальной точки в координатах (0, 0, 0).
- `Point3d endPoint = new Point3d(100, 50, 0)` — создается конечная точка с координатами (100, 50, 0).
- `DbLine line = new DbLine()` — создается новый объект линии.
- `line.Set(startPoint, endPoint)` — устанавливает начальную и конечную точки для линии.
- `line.DbEntity.Layer = "Lines"` — присваивает слой, на который будет добавлена линия.
- `line.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(1)` — задает цвет линии через индекс цвета AutoCAD, где 1 — красный.
- `line.DbEntity.AddToCurrentDocument()` — добавляет линию в активный чертеж.

4. Создание отрезка по длине и углу:

```csharp
[CommandMethod("MCreateLineByPointAndAngle", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateLineByPointAndAngle()
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
    line.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(2);
    
    // Добавление линии в текущий документ
    line.DbEntity.AddToCurrentDocument();
}
```

- `double length = 100` — задается длина отрезка.
- `double angle = Math.PI / 4` — угол в радианах (45 градусов).
- `startPoint.X + length * Math.Cos(angle), startPoint.Y + length * Math.Sin(angle)` — вычисление координат конечной точки отрезка с учетом длины и угла.
- `line.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(2)` — задает цвет линии через индекс цвета AutoCAD, где 2 — желтый.

5. Создание отрезка по длине и направлению (вектору):

```csharp
[CommandMethod("MCreateLineByLengthAndDirection", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateLineByLengthAndDirection()
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
    line.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(3);
    
    // Добавление линии в текущий документ
    line.DbEntity.AddToCurrentDocument();
}
```

- `Vector3d direction = new Vector3d(1, 1, 0)` — создание вектора направления, который указывает на диагональ по оси X и Y.
- `direction.GetNormal()` — нормализует вектор, преобразуя его в единичный вектор.
- `startPoint + direction * length` — вычисление конечной точки на основе длины отрезка и нормализованного вектора направления.

### Создание дуг

Создавать дугу мы можем с помощью класса DBCircArc. Рассмотрим параметры этого класса:

- **Point3d Center** — параметр, который принимает объект типа Point3d, служащий центром для дуги.
- **double radius** — параметр, принимающий данные типа double для установки радиуса дуги.
- **CircArc3d Arc** — параметр, принимающий объект типа CircArc3d, который также является классом дуги и имеет свои параметры для создания.

Свойства, используемые при создании CircArc3d:
- Point3d cent - центр дуги.
- Vector3d nrm - нормальный вектор к плоскости дуги.
- Vector3d refVec - вектор отсчета дуги.
- Point3d startPoint, Point3d pnt, Point3d endPoint - точки, задающие дугу.
- double radius - радиус дуги.
- double startAngle, double endAngle - начальный и конечный углы дуги в радианах.

Отличие DBCircArc от CircArc3d:

**DbCircArc (Database Circular Arc):**
- Это класс, представляющий дугу в базе данных nanoCAD.
- Наследуется от DbGeometry, что означает, что он может быть сохранен в базе данных и имеет дополнительные свойства, такие как слой, цвет и т.д.
- Содержит свойство Arc типа CircArc3d, которое представляет геометрию дуги.

**CircArc3d (3D Circular Arc):**
- Это класс, представляющий геометрию дуги в трехмерном пространстве.
- Является частью геометрической библиотеки и используется для математических операций и расчетов.
- Не имеет свойств, связанных с базой данных (слой, цвет и т.д.).

Способы создания дуги:

1. По трём точкам:

```csharp
[CommandMethod("MCreateArcByThreePoints", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateArcByThreePoints()
{
    // Создание трех точек для построения дуги
    Point3d startPoint = new Point3d(0, 0, 0);
    Point3d midPoint = new Point3d(50, 50, 0);
    Point3d endPoint = new Point3d(100, 0, 0);
    
    // Создание дуги по трем точкам
    DbCircArc arc = new DbCircArc();
    arc.Arc = new CircArc3d(startPoint, midPoint, endPoint);
    
    // Добавление дуги в текущий документ
    arc.DbEntity.AddToCurrentDocument();
}
```

2. По центру, радиусу и нормали:

```csharp
[CommandMethod("MCreateArcByCenterRadiusNormal", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateArcByCenterRadiusNormal()
{
    // Создание центра дуги
    Point3d center = new Point3d(0, 0, 0);
    
    // Задание радиуса дуги
    double radius = 50;
    
    // Создание нормали для дуги
    Vector3d normal = new Vector3d(0, 0, 1);
    
    // Создание вектора отсчета
    Vector3d refVector = new Vector3d(1, 0, 0);
    
    // Задание начального и конечного углов
    double startAngle = 0;
    double endAngle = Math.PI;
    
    // Создание дуги
    DbCircArc arc = new DbCircArc();
    arc.Arc = new CircArc3d(center, normal, refVector, radius, startAngle, endAngle);
    
    // Добавление дуги в текущий документ
    arc.DbEntity.AddToCurrentDocument();
}
```

Нормаль задается с помощью Vectord3d и в зависимости от значений меняет ориентацию дуги:

- **Нормаль (0, 0, 1)**:
  - Дуга лежит в плоскости XY
  - Положительное направление оси Z смотрит "вверх" от дуги
  - Углы отсчитываются против часовой стрелки, если смотреть сверху

- **Нормаль (0, 0, -1)**:
  - Дуга лежит в плоскости XY
  - Отрицательное направление оси Z смотрит "вверх" от дуги
  - Углы отсчитываются по часовой стрелке, если смотреть сверху

- **Нормаль (1, 0, 0)**:
  - Дуга лежит в плоскости YZ
  - Положительное направление оси X смотрит "вверх" от дуги
  - Углы отсчитываются против часовой стрелки, если смотреть вдоль оси X

- **Нормаль (-1, 0, 0)**:
  - Дуга лежит в плоскости YZ
  - Отрицательное направление оси X смотрит "вверх" от дуги
  - Углы отсчитываются по часовой стрелке, если смотреть вдоль оси X

- **Нормаль (0, 1, 0)**:
  - Дуга лежит в плоскости XZ
  - Положительное направление оси Y смотрит "вверх" от дуги
  - Углы отсчитываются против часовой стрелки, если смотреть вдоль оси Y

- **Нормаль (0, -1, 0)**:
  - Дуга лежит в плоскости XZ
  - Отрицательное направление оси Y смотрит "вверх" от дуги
  - Углы отсчитываются по часовой стрелке, если смотреть вдоль оси Y

3. По центру, радиусу, нормали, направляющему вектору, начальному углу дуги и конечному углу дуги:

```csharp
[CommandMethod("MCreateArcWithRefVector", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateArcWithRefVector()
{
    // Создание центра дуги
    Point3d center = new Point3d(0, 0, 0);
    
    // Задание радиуса дуги
    double radius = 40;
    
    // Создание нормали для дуги
    Vector3d normal = new Vector3d(0, 0, 1);
    
    // Создание направляющего вектора
    Vector3d refVector = new Vector3d(1, 1, 0);
    
    // Задание начального и конечного углов
    double startAngle = 0;
    double endAngle = Math.PI * 1.5;
    
    // Создание дуги
    DbCircArc arc = new DbCircArc();
    arc.Arc = new CircArc3d(center, normal, refVector, radius, startAngle, endAngle);
    
    // Добавление дуги в текущий документ
    arc.DbEntity.AddToCurrentDocument();
}
```

Начальный и конечный углы дуги задаются в радианах.

Направляющий вектор свойства:
- Определяет ориентацию дуги в ее плоскости.
- Влияет на то, где будут находиться начальная и конечная точки дуги при заданных углах.
- Изменение RefVector приводит к повороту дуги вокруг ее нормали, сохраняя при этом заданные углы.

4. Без использования параметра Arc у DbCircArc:

```csharp
[CommandMethod("MCreateArcWithoutArcParam", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateArcWithoutArcParam()
{
    // Создание центра дуги
    Point3d center = new Point3d(0, 0, 0);
    
    // Задание радиуса дуги
    double radius = 30;
    
    // Задание начального и конечного углов
    double startAngle = 0;
    double endAngle = Math.PI;
    
    // Создание дуги без использования свойства Arc
    DbCircArc arc = new DbCircArc();
    arc.Set(center, radius, startAngle, endAngle);
    
    // Добавление дуги в текущий документ
    arc.DbEntity.AddToCurrentDocument();
}
```

В зависимости от заданных данных, некоторые параметры можно опускать, и они будут вычисляться на основе других:

**StartPoint (начальная точка)**:
- Вычисляется на основе Center, Radius, StartAng, RefVector и Normal.
- Формула: Center + Radius * (cos(StartAng) * RefVector + sin(StartAng) * (Normal × RefVector))

**EndPoint (конечная точка)**:
- Вычисляется аналогично StartPoint, но использует EndAng вместо StartAng.

**RefVector**:
- Хотя RefVector можно задать явно, если он не задан, он может быть вычислен автоматически на основе Normal.
- Обычно выбирается так, чтобы быть перпендикулярным Normal и иметь простую ориентацию относительно мировых осей.

**Направление дуги**:
- Определяется порядком StartAng и EndAng.
- Если EndAng > StartAng, дуга идет против часовой стрелки.
- Если EndAng < StartAng, дуга идет по часовой стрелке.

Рассмотрим свойства и методы объекта типа DBCircArc:

```csharp
// Получение и установка параметров дуги
Point3d center = arc.Arc.Center;
double radius = arc.Arc.Radius;
double startAngle = arc.Arc.StartAngle;
double endAngle = arc.Arc.EndAngle;
```

Метод .Set для установки параметров:
```csharp
arc.Set(center, radius, startAngle, endAngle);
```

Другие полезные методы:
```csharp
// Добавление дуги в документ
arc.DbEntity.AddToCurrentDocument();

// Получение ID объекта
McObjectId id = arc.DbEntity.Id;

// Установка цвета для дуги
arc.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(5);

// Установка веса, типа, масштаба линии
arc.DbEntity.LineWeight = McLineWeight.LineWeight050;
arc.DbEntity.LineType = "DASHED";
arc.DbEntity.LineScale = 2.0;

// Получение плоскости, в которой находится дуга
Plane plane = arc.Arc.GetPlane();
```

### Создание окружности

Создавать окружность мы можем с помощью класса DBCircle. Рассмотрим параметры этого класса:

1. **Point3d Center** — параметр, который принимает объект типа Point3d, служащий центром для окружности.
2. **double radius** — параметр, принимающий данные типа double для установки радиуса окружности.

**DbCircle**:
- Это класс, представляющий окружность в базе данных nanoCAD.
- Наследуется от DbGeometry, что означает, что он может быть сохранен в базе данных и имеет дополнительные свойства, такие как слой, цвет и т.д.

Способ создания окружности по центру и радиусу:

```csharp
[CommandMethod("MCreateCircle", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateCircle()
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
    circle.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(3);
    
    // Добавление окружности в текущий документ
    circle.DbEntity.AddToCurrentDocument();
}
```

Основные методы объекта DbCircle:

```csharp
// Получение и установка параметров окружности
Point3d center = circle.Center;
double radius = circle.Radius;

// Метод .Set для установки параметров
circle.Set(center, radius);

// Добавление окружности в чертеж
circle.DbEntity.AddToCurrentDocument();

// Получение ID объекта
McObjectId id = circle.DbEntity.Id;

// Установка цвета, масштаба, типа и веса линии для окружности
circle.DbEntity.Color = McNativeGate.GetRgbByAcadColorIndex(4);
circle.DbEntity.LineScale = 2.0;
circle.DbEntity.LineType = "DASHED";
circle.DbEntity.LineWeight = McLineWeight.LineWeight050;
```

### Создание эскиза в смещенной плоскости

```csharp
[CommandMethod("MCreateOffsetSketch", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateOffsetSketch()
{
    // Создание смещенной плоскости
    // Базовая точка для плоскости
    Point3d basePoint = new Point3d(0, 0, 50);
    
    // Вектор нормали к плоскости
    Vector3d normal = new Vector3d(0, 0, 1);
    
    // Создание плоскости
    Plane plane = new Plane(basePoint, normal);
    
    // Создание эскиза на этой плоскости
    McSketch sketch = new McSketch();
    sketch.Plane = plane;
    
    // Создание окружности на эскизе
    DbCircle circle = new DbCircle();
    circle.Center = new Point3d(0, 0, 50);
    circle.Radius = 30;
    
    // Добавление окружности в документ
    circle.DbEntity.AddToCurrentDocument();
    
    // Связывание окружности с эскизом
    sketch.AddObject(circle.DbEntity.Id);
    
    // Добавление эскиза в документ
    McObjectManager.Add2Document(sketch);
}
```

## Базовые команды 3D модуля

### Операция выдавливания

```csharp
[CommandMethod("ExtrudeExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void ExtrudeExample()
{
    // Получение активной страницы документа
    McDocument doc = McDocumentsManager.GetActiveSheet();
    
    // Создание нового объекта 3D тело
    Mc3dSolid Detail3d = new Mc3dSolid();
    
    // Создание объекта выдавливания
    ExtrudeFeature EF1 = new ExtrudeFeature();
    
    // Приведение элемента к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    
    // Подготовка механизма создания 3D формы из 2D профиля
    solidEF1.PrepareExtrusion();
    
    // Добавление 3D-тела в активный документ
    McObjectManager.Add2Document(Detail3d);
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза для выдавливания
    // Точки создают прямоугольную форму в плоскости XY
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(0, 0, 0),
        new Point3d(100, 0, 0),
        new Point3d(100, 100, 0),
        new Point3d(0, 100, 0),
        new Point3d(0, 0, 0)
    });
    
    // Добавление полилинии в текущий документ
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза и добавление полилинии в него
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля на основе эскиза
    SketchProfile profile = new SketchProfile(sketch.Id);
    
    // Автоматическая обработка внешних контуров эскиза
    profile.AutoDetectMode = true;
    
    // Установка профиля для выдавливания
    EF1.Profile = profile.ID;
    
    // Определение расстояния выдавливания
    EF1.Distance = 50;
    
    // Установка угла равным 0 (прямое выдавливание)
    EF1.TaperAngle = 0;
    
    // Выбор типа выдавливания (объединение с существующей геометрией)
    EF1.Operation = PartFeatureOperation.Join;
    
    // Выбор положительного направления (по нормали плоскости эскиза)
    EF1.Direction = ExtrudeDirection.Positive;
    
    // Обновление всех объектов в документе
    McObjectManager.UpdateAll();
}
```

**Типы операций экструзии**:
- **Join**: Объединяет новую геометрию с существующими объектами
- **Intersect**: Создает геометрию на пересечении существующих объектов
- **NewBody**: Создание совершенно отдельного тела
- **Surface**: Создает поверхность вместо твердого тела

**Параметры направления выдавливания**:
1. **Positive**: выдавливание в положительном направлении от плоскости эскиза
2. **Negative**: Выдавливание в обратном направлении
3. **Symmetric**: Равномерное выдавливание с обеих сторон плоскости эскиза

### Операция выдавливания вращение

```csharp
[CommandMethod("RevolveExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void RevolveExample()
{
    // Получение активной страницы документа
    McDocument doc = McDocumentsManager.GetActiveSheet();
    
    // Создание нового объекта 3D тело
    Mc3dSolid Detail3d = new Mc3dSolid();
    
    // Создание объекта выдавливания вращением
    RevolveFeature EF1 = new RevolveFeature();
    
    // Приведение элемента к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    
    // Подготовка механизма создания 3D формы из 2D профиля
    solidEF1.PrepareExtrusion();
    
    // Добавление 3D-тела в активный документ
    McObjectManager.Add2Document(Detail3d);
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза для выдавливания вращением
    // Точки создают прямоугольную форму
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(20, 0, 0),
        new Point3d(50, 0, 0),
        new Point3d(50, 100, 0),
        new Point3d(20, 100, 0),
        new Point3d(20, 0, 0)
    });
    
    // Добавление полилинии в текущий документ
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    // Создание линии, представляющей ось вращения (вертикальная ось Y)
    Point3d start_y = new Point3d(0, 0, 0);
    Point3d end_y = new Point3d(0, 100, 0);
    DbLine axis_y = new DbLine();
    axis_y.Set(start_y, end_y);
    axis_y.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза и добавление полилинии в него
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля на основе эскиза
    SketchProfile profile = new SketchProfile(sketch.Id);
    profile.AutoDetectMode = true;
    
    // Создание геометрического параметра на основе оси вращения
    EF1.Axis = axis_y.DbEntity.Id;
    EF1.Profile = profile.ID;
    EF1.Angle = 2 * Math.PI; // Полный поворот на 360 градусов
    
    // Обновление всех объектов в документе
    McObjectManager.UpdateAll();
}
```

Полная команда RevolveExample:

```csharp
[CommandMethod("RevolveExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void RevolveExample()
{
    // Получение активной страницы документа
    McDocument doc = McDocumentsManager.GetActiveSheet();
    
    // Создание нового объекта 3D тело
    Mc3dSolid Detail3d = new Mc3dSolid();
    
    // Создание объекта выдавливания вращением
    RevolveFeature EF1 = new RevolveFeature();
    
    // Приведение элемента к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    
    // Подготовка механизма создания 3D формы из 2D профиля
    solidEF1.PrepareExtrusion();
    
    // Добавление 3D-тела в активный документ
    McObjectManager.Add2Document(Detail3d);
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза прямоугольника
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(20, 0, 0),
        new Point3d(50, 0, 0),
        new Point3d(50, 100, 0),
        new Point3d(20, 100, 0),
        new Point3d(20, 0, 0)
    });
    
    // Добавление полилинии в текущий документ
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    // Создание линии, представляющей ось вращения (вертикальная ось Y)
    Point3d start_y = new Point3d(0, 0, 0);
    Point3d end_y = new Point3d(0, 100, 0);
    DbLine axis_y = new DbLine();
    axis_y.Set(start_y, end_y);
    axis_y.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза и добавление полилинии в него
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля на основе эскиза
    SketchProfile profile = new SketchProfile(sketch.Id);
    profile.AutoDetectMode = true;
    
    // Выполнение операции вращения
    EF1.Axis = axis_y.DbEntity.Id;     // Ось вращения
    EF1.Profile = profile.ID;          // Профиль для вращения
    EF1.Angle = 2 * Math.PI;           // Полный оборот (360 градусов)
    
    // Обновление всех объектов в документе
    McObjectManager.UpdateAll();
}
```

### Операция выдавливания по траектории

Рассмотрим данную операцию на примере создания детали:

```csharp
[CommandMethod("SweepExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void SweepExample()
{
    // Получение активного документа
    McDocument doc = McDocumentsManager.GetActiveSheet();
    
    // Создание 3D-объекта и объектов для выдавливания
    Mc3dSolid Detail3d = new Mc3dSolid();
    SweepFeature EF1 = new SweepFeature();
    ExtrudeFeature EF2 = new ExtrudeFeature();
    
    // Приведение EF1 к типу Mc3dSolid
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    
    // Добавление созданных объектов в документ
    McObjectManager.Add2Document(Detail3d);
    Detail3d.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза для траектории на плоскости XY
    McSketch zxSketch = new McSketch();
    
    // Создание первого отрезка траектории
    Point3d startPoint = new Point3d(0, 0, 0);
    Point3d endPoint = new Point3d(0, 0, 100);
    DbLine line = new DbLine();
    line.Set(startPoint, endPoint);
    line.DbEntity.AddToCurrentDocument();
    
    // Создание второго отрезка траектории
    Point3d startPoint1 = new Point3d(0, 0, 100);
    Point3d endPoint1 = new Point3d(100, 0, 100);
    DbLine line1 = new DbLine();
    line1.Set(startPoint1, endPoint1);
    line1.DbEntity.AddToCurrentDocument();
    
    // Создание дуги для траектории
    Point3d startPointArc = new Point3d(100, 0, 100);
    Point3d middlePointArc = new Point3d(125, 0, 75);
    Point3d endPointArc = new Point3d(100, 0, 50);
    
    DbCircArc arc = new DbCircArc();
    arc.Arc = new CircArc3d(startPointArc, middlePointArc, endPointArc);
    arc.DbEntity.AddToCurrentDocument();
    
    // Добавление элементов в эскиз
    zxSketch.AddObject(line.DbEntity.Id);
    zxSketch.AddObject(line1.DbEntity.Id);
    zxSketch.AddObject(arc.DbEntity.Id);
    
    // Создание нового эскиза для профиля выдавливания
    Point3d basePoint = startPoint;
    Vector3d xAxisDir = new Vector3d(1, 0, 0);
    Plane plane = new Plane(basePoint, xAxisDir);
    
    McSketch xySketch = new McSketch();
    xySketch.Plane = plane;
    
    // Создание окружности для профиля выдавливания
    Point3d center = startPoint;
    double radius = 20;
    
    DbCircle circle = new DbCircle();
    circle.Center = center;
    circle.Radius = radius;
    
    // Поворот окружности на 90 градусов
    circle.DbEntity.TransformBy(Matrix3d.Rotation(Math.PI / 2, new Vector3d(0, 1, 0), center));
    circle.DbEntity.AddToCurrentDocument();
    
    // Добавление окружности в эскиз
    xySketch.AddObject(circle.DbEntity.Id);
    
    // Создание коллекции для траектории и профиля выдавливания
    McObjectsCollection geometryParams = new McObjectsCollection();
    geometryParams.Add(line.DbEntity.Id);
    geometryParams.Add(line1.DbEntity.Id);
    geometryParams.Add(arc.DbEntity.Id);
    
    // Создание профиля для выдавливания
    SketchProfile profile = new SketchProfile(xySketch.Id);
    profile.AutoDetectMode = true;
    
    // Выполнение операции выдавливания по траектории
    EF1.Profile = profile.ID;
    EF1.TwistAngle = 0;
    EF1.Orientation = ProfileOrientation.KeepNormalToPath;
    EF1.Path = geometryParams;
    
    // Обновление документа
    McObjectManager.UpdateAll();
    
    // Создание эскиза для отверстия
    McSketch xySketch2 = new McSketch();
    xySketch2.Plane = plane;
    
    // Создание окружности для отверстия
    Point3d center2 = startPoint;
    double radius2 = 10;
    
    DbCircle circle2 = new DbCircle();
    circle2.Center = center2;
    circle2.Radius = radius2;
    
    // Поворот окружности на 90 градусов
    circle2.DbEntity.TransformBy(Matrix3d.Rotation(Math.PI / 2, new Vector3d(0, 1, 0), center2));
    circle2.DbEntity.AddToCurrentDocument();
    
    // Добавление окружности в эскиз
    xySketch2.AddObject(circle2.DbEntity.Id);
    
    // Создание профиля для вырезания
    SketchProfile profile2 = new SketchProfile(xySketch2.Id);
    profile2.AutoDetectMode = true;
    
    // Настройка параметров выдавливания с вырезанием
    Mc3dSolid solidEF2 = EF2 as Mc3dSolid;
    solidEF2.PrepareExtrusion();
    solidEF2.DbEntity.AddToCurrentDocument();
    
    EF2.Profile = profile2.ID;
    EF2.Distance = 200;
    EF2.TaperAngle = 0;
    EF2.Operation = PartFeatureOperation.Cut;
    EF2.Direction = ExtrudeDirection.Positive;
    
    // Обновление документа
    McObjectManager.UpdateAll();
}
```

### Выдавливание по сечениям

Рассмотрим данную операцию на примере создания двух окружностей, которые мы выдавим по сечениям:

```csharp
[CommandMethod("LoftExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void LoftExample()
{
    // Получение активного документа
    McDocument doc = McDocumentsManager.GetActiveSheet();
    
    // Создание объекта 3D-геометрии и объекта Loft
    Mc3dSolid Detail3d = new Mc3dSolid();
    LoftFeature EF1 = new LoftFeature();
    
    // Приведение объекта Loft к типу Mc3dSolid
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    
    // Настройка параметров Loft
    EF1.Closed = true;
    EF1.LoftType = LoftType.Regular;
    EF1.Operation = PartFeatureOperation.Join;
    
    // Добавление 3D-объекта Loft в документ
    McObjectManager.Add2Document(Detail3d);
    Detail3d.DbEntity.AddToCurrentDocument();
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание первой плоскости для сечения
    Plane plane1 = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));
    
    // Создание первого эскиза и окружности для сечения
    McSketch sketch1 = new McSketch();
    sketch1.Plane = plane1;
    
    DbCircle circle1 = new DbCircle();
    circle1.Center = new Point3d(0, 0, 0);
    circle1.Radius = 50;
    circle1.DbEntity.AddToCurrentDocument();
    
    sketch1.AddObject(circle1.DbEntity.Id);
    
    // Создание второй плоскости для сечения
    Plane plane2 = new Plane(new Point3d(0, 0, 120), new Vector3d(0, 0, 1));
    
    // Создание второго эскиза и окружности для сечения
    McSketch sketch2 = new McSketch();
    sketch2.Plane = plane2;
    
    DbCircle circle2 = new DbCircle();
    circle2.Center = new Point3d(0, 0, 120);
    circle2.Radius = 25;
    circle2.DbEntity.AddToCurrentDocument();
    
    sketch2.AddObject(circle2.DbEntity.Id);
    
    // Создание профилей для Loft
    SketchProfile profile1 = new SketchProfile(sketch1.Id);
    profile1.AutoDetectMode = true;
    
    SketchProfile profile2 = new SketchProfile(sketch2.Id);
    profile2.AutoDetectMode = true;
    
    // Добавление сечений в Loft и настройка углов
    LoftSection section1 = new LoftSection();
    section1.Profile = profile1.ID;
    EF1.AddSection(section1);
    
    LoftSection section2 = new LoftSection();
    section2.Profile = profile2.ID;
    EF1.AddSection(section2);
    
    // Настройка углов первого и последнего сечений
    EF1.StartAngle = Math.PI;
    EF1.EndAngle = Math.PI;
    
    // Добавление Loft-операции в документ
    McObjectManager.UpdateAll();
}
```

### Создание скруглений и фасок

Создание скруглений:

```csharp
[CommandMethod("FilletSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void FilletSample()
{
    // Получение активной страницы и создание объекта тела и выдавливания
    McDocument doc = McDocumentsManager.GetActiveSheet();
    Mc3dSolid Detail3d = new Mc3dSolid();
    ExtrudeFeature EF1 = new ExtrudeFeature();
    
    // Приведение выдавливания к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    solidEF1.PrepareExtrusion();
    
    // Добавление объектов в документ
    McObjectManager.Add2Document(Detail3d);
    Detail3d.DbEntity.AddToCurrentDocument();
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза с полилинией (квадрат)
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(-50, -50, 0),
        new Point3d(50, -50, 0),
        new Point3d(50, 50, 0),
        new Point3d(-50, 50, 0),
        new Point3d(-50, -50, 0)
    });
    
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля для выдавливания
    SketchProfile profile = new SketchProfile(sketch.Id);
    profile.AutoDetectMode = true;
    
    // Установка параметров для выдавливания
    EF1.Profile = profile.ID;
    EF1.Distance = 100;
    EF1.TaperAngle = 0;
    EF1.Operation = PartFeatureOperation.Join;
    EF1.Direction = ExtrudeDirection.Positive;
    
    // Обновление объектов
    McObjectManager.UpdateAll();
    
    // Получение всех ребер полученного куба и создание скруглений
    McEdgeFeaturesCollection edges = Detail3d.GetEdges();
    Detail3d.AddFilletFeature(edges.Ids, 10);
    
    // Обновление документа
    McObjectManager.UpdateAll();
}
```

Создание фасок:

```csharp
[CommandMethod("ChamferSample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void ChamferSample()
{
    // Получение активной страницы и создание объекта тела и выдавливания
    McDocument doc = McDocumentsManager.GetActiveSheet();
    Mc3dSolid Detail3d = new Mc3dSolid();
    ExtrudeFeature EF1 = new ExtrudeFeature();
    
    // Приведение выдавливания к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    solidEF1.PrepareExtrusion();
    
    // Добавление объектов в документ
    McObjectManager.Add2Document(Detail3d);
    Detail3d.DbEntity.AddToCurrentDocument();
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза с полилинией (квадрат)
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(-50, -50, 0),
        new Point3d(50, -50, 0),
        new Point3d(50, 50, 0),
        new Point3d(-50, 50, 0),
        new Point3d(-50, -50, 0)
    });
    
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля для выдавливания
    SketchProfile profile = new SketchProfile(sketch.Id);
    profile.AutoDetectMode = true;
    
    // Установка параметров для выдавливания
    EF1.Profile = profile.ID;
    EF1.Distance = 100;
    EF1.TaperAngle = 0;
    EF1.Operation = PartFeatureOperation.Join;
    EF1.Direction = ExtrudeDirection.Positive;
    
    // Обновление объектов
    McObjectManager.UpdateAll();
    
    // Получение всех ребер куба и создание фасок
    McEdgeFeaturesCollection edges = Detail3d.GetEdges();
    Detail3d.AddChamferFeature(edges.Ids, ChamferType.None, ChamferSetbackType.None, 5, 0, 0);
    
    // Обновление документа
    McObjectManager.UpdateAll();
}
```

Параметры команды AddChamferFeature:

- **IEnumerable<McObjectId> edgesIds** — ребра, на которых нужно делать фаску.
- **ChamferType chamferType** — тип создания фаски:
  - С помощью указания одного расстояния
  - С помощью указания расстояния и угла
  - С помощью указания двух расстояний
- **ChamferSetbackType chamferSetbackType** — отступ от края:
  - None - без отступа, фаска создается непосредственно по краю
  - Flat - с плоским равномерным отступом
- **double distance** — расстояние для создания фаски.
- **double distance2** — второе расстояние для создания фаски.
- **double angle** — угол в радианах для создания фаски.

## Создание детали с использованием MultiCAD API

Создадим деталь по чертежу:

```csharp
[CommandMethod("CreateDetailExample", CommandFlags.NoCheck | CommandFlags.NoPrefix)]
public void CreateDetailExample()
{
    // Создание модели и объекта выдавливания
    McDocument doc = McDocumentsManager.GetActiveSheet();
    Mc3dSolid Detail3d = new Mc3dSolid();
    ExtrudeFeature EF1 = new ExtrudeFeature();
    
    // Приведение выдавливания к типу 3D-тела
    Mc3dSolid solidEF1 = EF1 as Mc3dSolid;
    solidEF1.PrepareExtrusion();
    
    // Добавление объектов в документ
    McObjectManager.Add2Document(Detail3d);
    Detail3d.DbEntity.AddToCurrentDocument();
    solidEF1.DbEntity.AddToCurrentDocument();
    
    // Создание эскиза нижней части детали
    Polyline3d pline = new Polyline3d(new Point3d[] {
        new Point3d(-50, -30, 0),
        new Point3d(50, -30, 0),
        new Point3d(50, 30, 0),
        new Point3d(-50, 30, 0),
        new Point3d(-50, -30, 0)
    });
    
    DbPolyline dbPl = new DbPolyline(pline);
    dbPl.DbEntity.AddToCurrentDocument();
    
    McSketch sketch = new McSketch();
    sketch.AddObject(dbPl.DbEntity.Id);
    
    // Создание профиля для выдавливания нижней части
    SketchProfile profile = new SketchProfile(sketch.Id);
    profile.AutoDetectMode = true;
    
    // Выдавливание нижней части детали
    EF1.Profile = profile.ID;
    EF1.Distance = 60;
    EF1.TaperAngle = 0;
    EF1.Operation = PartFeatureOperation.Join;
    EF1.Direction = ExtrudeDirection.Positive;
    
    // Обновление объектов
    McObjectManager.UpdateAll();
    
    // Создание эскиза верхней части детали
    Plane plane2 = new Plane(new Point3d(0, 0, 60), new Vector3d(0, 0, 1));
    McSketch sketch2 = new McSketch();
    sketch2.Plane = plane2;
    
    // Создание полилинии для верхней части
    Polyline3d pline2 = new Polyline3d(new Point3d[] {
        new Point3d(-25, -15, 60),
        new Point3d(25, -15, 60),
        new Point3d(25, 15, 60),
        new Point3d(-25, 15, 60),
        new Point3d(-25, -15, 60)
    });
    
    DbPolyline dbPl2 = new DbPolyline(pline2);
    dbPl2.DbEntity.AddToCurrentDocument();
    
    // Добавление дуги в эскиз
    Point3d arcStart = new Point3d(25, 0, 60);
    Point3d arcMid = new Point3d(37, 0, 60);
    Point3d arcEnd = new Point3d(25, 0, 60);
    
    DbCircArc arc = new DbCircArc();
    arc.Arc = new CircArc3d(arcStart, arcMid, arcEnd);
    arc.DbEntity.AddToCurrentDocument();
    
    // Добавление элементов в эскиз
    sketch2.AddObject(dbPl2.DbEntity.Id);
    sketch2.AddObject(arc.DbEntity.Id);
    
    // Создание профиля для выдавливания верхней части
    SketchProfile profile2 = new SketchProfile(sketch2.Id);
    profile2.AutoDetectMode = true;
    
    // Создание и настройка выдавливания верхней части
    ExtrudeFeature EF2 = new ExtrudeFeature();
    Mc3dSolid solidEF2 = EF2 as Mc3dSolid;
    solidEF2.PrepareExtrusion();
    solidEF2.DbEntity.AddToCurrentDocument();
    
    EF2.Profile = profile2.ID;
    EF2.Distance = 40;
    EF2.TaperAngle = 0;
    EF2.Operation = PartFeatureOperation.Join;
    EF2.Direction = ExtrudeDirection.Positive;
    
    // Обновление объектов
    McObjectManager.UpdateAll();
    
    // Создание отверстий
    // Создание первого отверстия
    Plane plane3 = new Plane(new Point3d(0, 0, 100), new Vector3d(0, 0, 1));
    McSketch sketch3 = new McSketch();
    sketch3.Plane = plane3;
    
    DbCircle circle1 = new DbCircle();
    circle1.Center = new Point3d(0, 0, 100);
    circle1.Radius = 10;
    circle1.DbEntity.AddToCurrentDocument();
    
    sketch3.AddObject(circle1.DbEntity.Id);
    
    SketchProfile profile3 = new SketchProfile(sketch3.Id);
    profile3.AutoDetectMode = true;
    
    ExtrudeFeature EF3 = new ExtrudeFeature();
    Mc3dSolid solidEF3 = EF3 as Mc3dSolid;
    solidEF3.PrepareExtrusion();
    solidEF3.DbEntity.AddToCurrentDocument();
    
    EF3.Profile = profile3.ID;
    EF3.Distance = 40;
    EF3.TaperAngle = 0;
    EF3.Operation = PartFeatureOperation.Cut;
    EF3.Direction = ExtrudeDirection.Negative;
    
    // Обновление объектов
    McObjectManager.UpdateAll();
    
    // Создание боковых отверстий
    // Левое отверстие
    Plane planeLeft = new Plane(new Point3d(-50, 0, 30), new Vector3d(1, 0, 0));
    McSketch sketchLeft = new McSketch();
    sketchLeft.Plane = planeLeft;
    
    DbCircle circleLeft = new DbCircle();
    circleLeft.Center = new Point3d(-50, 0, 30);
    circleLeft.Radius = 8;
    circleLeft.DbEntity.AddToCurrentDocument();
    
    sketchLeft.AddObject(circleLeft.DbEntity.Id);
    
    SketchProfile profileLeft = new SketchProfile(sketchLeft.Id);
    profileLeft.AutoDetectMode = true;
    
    ExtrudeFeature EF4 = new ExtrudeFeature();
    Mc3dSolid solidEF4 = EF4 as Mc3dSolid;
    solidEF4.PrepareExtrusion();
    solidEF4.DbEntity.AddToCurrentDocument();
    
    EF4.Profile = profileLeft.ID;
    EF4.Distance = 25;
    EF4.TaperAngle = 0;
    EF4.Operation = PartFeatureOperation.Cut;
    EF4.Direction = ExtrudeDirection.Positive;
    
    // Правое отверстие
    Plane planeRight = new Plane(new Point3d(50, 0, 30), new Vector3d(-1, 0, 0));
    McSketch sketchRight = new McSketch();
    sketchRight.Plane = planeRight;
    
    DbCircle circleRight = new DbCircle();
    circleRight.Center = new Point3d(50, 0, 30);
    circleRight.Radius = 8;
    circleRight.DbEntity.AddToCurrentDocument();
    
    sketchRight.AddObject(circleRight.DbEntity.Id);
    
    SketchProfile profileRight = new SketchProfile(sketchRight.Id);
    profileRight.AutoDetectMode = true;
    
    ExtrudeFeature EF5 = new ExtrudeFeature();
    Mc3dSolid solidEF5 = EF5 as Mc3dSolid;
    solidEF5.PrepareExtrusion();
    solidEF5.DbEntity.AddToCurrentDocument();
    
    EF5.Profile = profileRight.ID;
    EF5.Distance = 25;
    EF5.TaperAngle = 0;
    EF5.Operation = PartFeatureOperation.Cut;
    EF5.Direction = ExtrudeDirection.Positive;
    
    // Обновление документа
    McObjectManager.UpdateAll();
}
```

Этот пример показывает создание полной детали, состоящей из:
1. Прямоугольного основания
2. Верхней части меньшего размера
3. Вертикального отверстия, проходящего сквозь верхнюю часть
4. Двух горизонтальных сквозных отверстий в боковых гранях

В итоге получается деталь с основанием в форме параллелепипеда, верхней надстройкой и тремя отверстиями - одним вертикальным и двумя горизонтальными.

При создании такой детали мы использовали различные техники:
- Работа с плоскостями в разных ориентациях
- Создание профилей на основе эскизов
- Операции выдавливания как для создания твердых тел, так и для вырезания
- Управление направлением выдавливания
- Комбинирование нескольких объектов для создания сложной формы

Все эти техники являются основополагающими при создании 3D-моделей в MultiCAD и могут быть адаптированы для создания более сложных деталей.
