using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefabs
{
    public static Grid Grid = (Grid)Resources.Load("Grid", typeof(Grid));
    public static Cell Cell = (Cell)Resources.Load("Cell", typeof(Cell));
    public static IslandPiece IslandPiece = (IslandPiece)Resources.Load("IslandPiece", typeof(IslandPiece));
    public static AreaAttack AreaAttack = (AreaAttack)Resources.Load("AreaAttack", typeof(AreaAttack));
    public static DirectAttack DirectAttack = (DirectAttack)Resources.Load("DirectAttack", typeof(DirectAttack));
    public static SpikyBush SpikyBush = (SpikyBush)Resources.Load("SpikyBush", typeof(SpikyBush));
    public static Tree Tree = (Tree)Resources.Load("Tree", typeof(Tree));
    public static Ballista Ballista = (Ballista)Resources.Load("Ballista", typeof(Ballista));
    public static BallistaBolt BallistaBolt = (BallistaBolt)Resources.Load("BallistaBolt", typeof(BallistaBolt));
    public static Piston Piston = (Piston)Resources.Load("Piston", typeof(Piston));
}
