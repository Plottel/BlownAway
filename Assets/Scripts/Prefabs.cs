using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefabs
{
    public static IslandGrid Grid_Tutorial = (IslandGrid)Resources.Load("Grid_Tutorial", typeof(IslandGrid));
    public static IslandGrid Grid_Factory = (IslandGrid)Resources.Load("Grid_FactoryWorld", typeof(IslandGrid));
    public static IslandGrid Grid_Ballista = (IslandGrid)Resources.Load("Grid_BallistaWorld", typeof(IslandGrid));
    public static IslandGrid Grid_TerrainPark = (IslandGrid)Resources.Load("Grid_TerrainPark", typeof(IslandGrid));
    public static IslandGrid Grid_VolcanoRun = (IslandGrid)Resources.Load("Grid_VolcanoRun", typeof(IslandGrid));
    public static IslandGrid Grid_WinterFortress = (IslandGrid)Resources.Load("Grid_WinterFortress", typeof(IslandGrid));
    public static IslandGrid Grid = (IslandGrid)Resources.Load("Grid", typeof(IslandGrid));
    public static Cell Cell = (Cell)Resources.Load("Cell", typeof(Cell));
    public static IslandPiece IslandPiece = (IslandPiece)Resources.Load("IslandPiece", typeof(IslandPiece));
    public static AreaAttack AreaAttack = (AreaAttack)Resources.Load("AreaAttack", typeof(AreaAttack));
    public static DirectAttack DirectAttack = (DirectAttack)Resources.Load("DirectAttack", typeof(DirectAttack));
    public static SpikyBush SpikyBush = (SpikyBush)Resources.Load("SpikyBush", typeof(SpikyBush));
    public static Tree Tree = (Tree)Resources.Load("Tree", typeof(Tree));
    public static Ballista Ballista = (Ballista)Resources.Load("Ballista", typeof(Ballista));
    public static BallistaBolt BallistaBolt = (BallistaBolt)Resources.Load("BallistaBolt", typeof(BallistaBolt));
    public static Piston Piston = (Piston)Resources.Load("Piston", typeof(Piston));
    public static Fan Fan = (Fan)Resources.Load("Fan", typeof(Fan));
    public static PressurePlate PressurePlate = (PressurePlate)Resources.Load("PressurePlate", typeof(PressurePlate));
    public static Lava Lava = (Lava)Resources.Load("Lava", typeof(Lava));
    public static Wall Wall = (Wall)Resources.Load("Wall", typeof(Wall));
    public static LavaPipe LavaPipe = (LavaPipe)Resources.Load("LavaPipe", typeof(LavaPipe));
    public static Volcano Volcano = (Volcano)Resources.Load("Volcano", typeof(Volcano));
    public static GameObject magicEarth = (GameObject)Resources.Load("magicEarth", typeof(GameObject));
    public static GameObject cannonBlast = (GameObject)Resources.Load("cannonBlast", typeof(GameObject));
    public static GameObject DeathPE = (GameObject)Resources.Load("DeathPE", typeof(GameObject));
    public static GameObject TempAttack = (GameObject)Resources.Load("TempAttack", typeof(GameObject));
    public static GameObject OnFirePE = (GameObject)Resources.Load("OnFirePE", typeof(GameObject));
    public static Ultimate Ultimate = (Ultimate)Resources.Load("Ultimate", typeof(Ultimate));
    public static SpawnPointer SpawnPointer = (SpawnPointer)Resources.Load("SpawnPointer", typeof(SpawnPointer));


    // Winter assets
    public static GameObject WinterIsland = (GameObject)Resources.Load("WinterIsland", typeof(GameObject));
    public static GameObject WinterTree = (GameObject)Resources.Load("WinterTree", typeof(GameObject));
    public static GameObject WinterFan = (GameObject)Resources.Load("WinterFan", typeof(GameObject));
    public static GameObject WinterPiston = (GameObject)Resources.Load("WinterPiston", typeof(GameObject));
    public static GameObject WinterSpikyBush = (GameObject)Resources.Load("WinterSpikyBush", typeof(GameObject));
    public static GameObject WinterLavaPipe = (GameObject)Resources.Load("WinterLavaPipe", typeof(GameObject));
    public static GameObject WinterWall = (GameObject)Resources.Load("WinterWall", typeof(GameObject));
    public static GameObject WinterPressurePlate = (GameObject)Resources.Load("winterPressurePlate", typeof(GameObject));
}
