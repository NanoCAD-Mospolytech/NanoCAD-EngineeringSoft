# Методическое пособие по NanoCAD NRX API

## Содержание

- [Регистрация команд](#регистрация-команд)
- [Добавление объектов в пространство модели](#добавление-объектов-в-пространство-модели)
- [Базовые команды 2D модуля](#базовые-команды-2d-модуля)
  - [Создание отрезков](#создание-отрезков)
  - [Создание дуг](#создание-дуг)
  - [Создание окружностей](#создание-окружностей)
  - [Создание полилиний](#создание-полилиний)

## Регистрация команд

Точка входа в NRX-приложение - это функция `acrxEntryPoint`, которая вызывается при загрузке и выгрузке модуля:

```cpp
extern "C" __declspec(dllexport) AcRx::AppRetCode
acrxEntryPoint(AcRx::AppMsgCode msg, void* appId)
{
    switch (msg)
    {
    case AcRx::kInitAppMsg:
        acrxDynamicLinker->unlockApplication(appId);
        acrxDynamicLinker->registerAppMDIAware(appId);
        acedRegCmds->addCommand(L"MYNRXCOMMANDS_GROUP",
            L"_MYNRXCOMMAND",
            L"MYNRXCOMMAND",
            ACRX_CMD_TRANSPARENT,
            helloNrxCmd);
    break;
case AcRx::kUnloadAppMsg:
    acedRegCmds->removeGroup(L"MYNRXCOMMANDS_GROUP");
    break;
    }
    
    return AcRx::kRetOK;
}
```

Простейшее объявление команды:

```cpp
void helloNrxCmd()
{
    acutPrintf(L"\nHello, NRX!\n");
}
```

## Добавление объектов в пространство модели

Вспомогательная функция для добавления объектов в пространство модели:

```cpp
void addToModelSpace(AcDbObjectId& objId, AcDbEntity* pEntity)
{
    AcDbBlockTable* pBlockTable;
    AcDbBlockTableRecord* pBlock;
    acdbHostApplicationServices()->workingDatabase()
        ->getSymbolTable(pBlockTable, AcDb::kForRead);
    pBlockTable->getAt(ACDB_MODEL_SPACE, pBlock, AcDb::kForWrite);
    pBlockTable->close();
    pBlock->appendAcDbEntity(objId, pEntity);
    pBlock->close();
}
```

## Базовые команды 2D модуля

### Создание отрезков

Для создания отрезка используется класс `NcDbLine`. Отрезок определяется двумя точками в пространстве:

```cpp
void helloNrxLine() {
    NcDbObjectId lineId;
    NcDbLine* Line = new NcDbLine(NcGePoint3d(0, 0, 0), NcGePoint3d(100, 100, 0));
    addToModelSpace(lineId, Line);
    Line->close();
    acutPrintf(L"\nЛиния создана!\n");
}
```

Разбор функции:

1. `NcDbObjectId lineId` — создание переменной для хранения идентификатора созданного объекта.
2. `NcDbLine* Line = new NcDbLine(NcGePoint3d(0, 0, 0), NcGePoint3d(100, 100, 0))` — создание нового отрезка с начальной точкой в координатах (0, 0, 0) и конечной точкой в координатах (100, 100, 0).
3. `addToModelSpace(lineId, Line)` — добавление отрезка в пространство модели.
4. `Line->close()` — закрытие объекта после завершения работы с ним.
5. `acutPrintf(L"\nЛиния создана!\n")` — вывод сообщения в командную строку о том, что линия создана.

### Создание дуг

Для создания дуги используется класс `NcDbArc`. Дуга может быть определена несколькими способами, наиболее распространенный — через центр, радиус и начальный/конечный угол:

```cpp
void helloNrxArc()
{
    NcDbObjectId ArcId;
    NcDbArc* Arc = new NcDbArc(NcGePoint3d(0, 0, 0), 50, 0.0, 1.309);
    addToModelSpace(ArcId, Arc);
    Arc -> close();
    acutPrintf(L"\nДуга создана!\n");
}
```

Разбор функции:

1. `NcDbObjectId ArcId` — создание переменной для хранения идентификатора созданного объекта.
2. `NcDbArc* Arc = new NcDbArc(NcGePoint3d(0, 0, 0), 50, 0.0, 1.309)` — создание новой дуги с центром в координатах (0, 0, 0), радиусом 50 единиц, начальным углом 0.0 радиан и конечным углом 1.309 радиан (примерно 75 градусов).
3. `addToModelSpace(ArcId, Arc)` — добавление дуги в пространство модели.
4. `Arc->close()` — закрытие объекта после завершения работы с ним.
5. `acutPrintf(L"\nДуга создана!\n")` — вывод сообщения в командную строку о том, что дуга создана.

### Создание окружностей

Для создания окружности используется класс `NcDbCircle`. Окружность определяется центром, вектором нормали к плоскости окружности и радиусом:

```cpp
void helloNrxCircle()
{
    NcDbObjectId CircleId;
    NcDbCircle* Circle = new NcDbCircle(NcGePoint3d(0, 0, 0), NcGeVector3d(1,0,0), 50);
    addToModelSpace(CircleId, Circle);
    Circle->close();
    acutPrintf(L"\nОкружность создана!\n");
}
```

Разбор функции:

1. `NcDbObjectId CircleId` — создание переменной для хранения идентификатора созданного объекта.
2. `NcDbCircle* Circle = new NcDbCircle(NcGePoint3d(0, 0, 0), NcGeVector3d(1,0,0), 50)` — создание новой окружности с центром в координатах (0, 0, 0), вектором нормали (1, 0, 0) (окружность будет лежать в плоскости YZ) и радиусом 50 единиц.
3. `addToModelSpace(CircleId, Circle)` — добавление окружности в пространство модели.
4. `Circle->close()` — закрытие объекта после завершения работы с ним.
5. `acutPrintf(L"\nОкружность создана!\n")` — вывод сообщения в командную строку о том, что окружность создана.

### Создание полилиний

Для создания полилинии используется класс `AcDbPolyline`. Полилиния представляет собой последовательность связанных отрезков или дуг:

```cpp
void helloNrxPolyLine3D() {
    AcDbPolyline* pPoly = new AcDbPolyline();
    AcGePoint3d pt;
    ads_point adsPt;
    int rc;
    int ptCount = 0;
    acutPrintf(_T("\nУкажите точки полилинии (нажмите Enter для завершения): "));
    while ((rc = acedGetPoint(NULL, ptCount == 0 ?
        _T("\nПервая точка: ") :
        _T("\nСледующая точка или Enter для завершения: "),
        adsPt)) == RTNORM)
    {
        pPoly->addVertexAt(ptCount, AcGePoint2d(adsPt[0], adsPt[1]), 0, 0, 0);
        ptCount++;
    }
    if (ptCount < 2)
    {
        delete pPoly;
        acutPrintf(_T("\nДля полилинии требуется минимум 2 точки."));
        return;
    }
    AcDbObjectId polyId;
    addToModelSpace(polyId, pPoly);
    pPoly->close();
    acutPrintf(_T("\nПолилиния создана успешно."));
}
```

Разбор функции:

1. `AcDbPolyline* pPoly = new AcDbPolyline()` — создание новой полилинии.
2. Объявление переменных:
   - `AcGePoint3d pt` — для хранения 3D точки.
   - `ads_point adsPt` — для хранения точки в формате, возвращаемом функцией acedGetPoint.
   - `int rc` — для хранения кода возврата функции acedGetPoint.
   - `int ptCount = 0` — счетчик точек.
3. `acutPrintf(_T("\nУкажите точки полилинии (нажмите Enter для завершения): "))` — вывод инструкции пользователю.
4. Цикл для получения точек от пользователя:
   ```cpp
   while ((rc = acedGetPoint(NULL, ptCount == 0 ?
       _T("\nПервая точка: ") :
       _T("\nСледующая точка или Enter для завершения: "),
       adsPt)) == RTNORM)
   {
       pPoly->addVertexAt(ptCount, AcGePoint2d(adsPt[0], adsPt[1]), 0, 0, 0);
       ptCount++;
   }
   ```
   - `acedGetPoint` — функция для получения точки от пользователя.
   - `RTNORM` — константа, указывающая на успешное получение точки.
   - `pPoly->addVertexAt(ptCount, AcGePoint2d(adsPt[0], adsPt[1]), 0, 0, 0)` — добавление вершины к полилинии. Параметры: индекс точки, координаты точки (X, Y), выпуклость (0 для прямой линии), начальная и конечная ширина линии.
5. Проверка на минимальное количество точек:
   ```cpp
   if (ptCount < 2)
   {
       delete pPoly;
       acutPrintf(_T("\nДля полилинии требуется минимум 2 точки."));
       return;
   }
   ```
6. `addToModelSpace(polyId, pPoly)` — добавление полилинии в пространство модели.
7. `pPoly->close()` — закрытие объекта после завершения работы с ним.
8. `acutPrintf(_T("\nПолилиния создана успешно."))` — вывод сообщения об успешном создании полилинии.
