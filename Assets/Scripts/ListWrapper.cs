using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellListWrapper
{
    [SerializeField]
    public List<Cell> items = new List<Cell>();

    public Cell this[int i]
    {
        get { return items[i]; }
        set { items[i] = value; }
    }
}
