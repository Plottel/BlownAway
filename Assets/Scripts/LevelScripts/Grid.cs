using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityEngine
{
    public enum Quadrant
    {
        TopLeft,
        TopRight,
        BotLeft,
        BotRight
    }
}

[System.Serializable]
public class Grid : MonoBehaviour
{
    public static float SHAKE_DISTANCE = 0.1f;
    public static float SHAKE_SPEED = 0.01f;
    public static int SHAKE_COUNT = 20;

    public Cell cell;

    public int cellSize = 1;

    [SerializeField]
    public int Cols;
    [SerializeField]
    public int Rows;

    [SerializeField]
    public List<CellListWrapper> _cells;

    [SerializeField]
    public int ColCellCount
    {
        get { return _cells.Count; }
    }

    public Cell MidCell
    {
        get
        {
            if (Cols <= 0 || Rows <= 0)
                return null;

            return _cells[(int)Mathf.Floor(Cols / 2)][(int)Mathf.Floor(Rows / 2)];
        }
    }

    public List<Cell> Corners
    {
        get
        {
            return new List<Cell>
            {
                _cells[0][0],
                _cells[0][Rows - 1],
                _cells[Cols - 1][0],
                _cells[Cols - 1][Rows - 1]
            };
        }
    }

    public Cell this[int col, int row]
    {
        get
        {
            if (col < Cols && col >= 0 && row < Rows && row >= 0)
                return _cells[col].items[row];
            return null;
        }
    }

    public Vector2 IndexOf(Cell cell)
    {
        for (int col = 0; col < Cols; ++col)
        {
            for (int row = 0; row < Rows; ++row)
            {
                if (cell == _cells[col][row])
                    return new Vector2(col, row);
            }                
        }

        throw new System.Exception("Cell not in Grid");
    }

    public List<Cell> CellsAsList
    {
        get
        {
            var result = new List<Cell>();

            for (int col = 0; col < Cols; ++col)
            {
                for (int row = 0; row < Rows; ++row)
                    result.Add(_cells[col][row]);
            }

            return result;
        }
    }

    public List<Cell> Border
    {
        get
        {
            var result = new List<Cell>();

            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    if (col == 0 || col == Cols - 1 || row == 0 || row == Rows - 1)
                        result.Add(_cells[col][row]);
                }
            }

            return result;
        }
    }

    public List<Cell> this[int col]
    {
        get
        {
            if (_cells.Count > 0)
                return _cells[0].items;
            return new List<Cell>();
        }
    }

    private Cell CreateCellAt(Vector3 pos)
    {
        Cell newCell = Instantiate(Prefabs.Cell, pos, Prefabs.Cell.transform.rotation);
        newCell.transform.parent = this.gameObject.transform;

        return newCell;
    }

    public void AddColumn()
    {
        var newCol = new CellListWrapper();

        // For each row in the newly created column.
        for (int row = 0; row < Rows; ++row)
        {
            float x = transform.position.x + Cols * cellSize;
            float y = transform.position.y;
            float z = transform.position.z + row * cellSize;

            newCol.items.Add(CreateCellAt(new Vector3(x, y, z)));
        }

        _cells.Add(newCol);
        ++Cols;
    }    

    public void AddRow()
    {
        //For each column to have a new row added to it
        for (int col = 0; col < Cols; ++col)
        {
            float x = transform.position.x + col * cellSize;
            float y = transform.position.y;
            float z = transform.position.z + Rows * cellSize;

            _cells[col].items.Add(CreateCellAt(new Vector3(x, y, z)));
        }

        ++Rows;
    }

    public void RemoveColumn()
    {
        for (int row = 0; row < Rows; ++row)
        {
            DestroyImmediate(_cells.Last()[row].gameObject);
            _cells.Last()[row] = null;
        }

        _cells.Remove(_cells.Last());

        --Cols;
    }

    public void RemoveRow()
    {
        for (int col = 0; col < Cols; ++col)
        {
            DestroyImmediate(_cells[col].items.Last().gameObject);
            _cells[col][Rows - 1] = null;
            _cells[col].items.RemoveAt(Rows - 1);
        }

        --Rows;
    }

    public void Clear()
    {
        for (int col = 0; col < Cols; ++col)
            RemoveColumn();
        for (int row = 0; row < Rows; ++row)
            RemoveRow();
    }

    public void MakePopulated1010()
    {
        Clear();
        
        for (int i = 0; i < 10; ++i)
        {
            AddColumn();
            AddRow();
        }

        PopulateIslands();
    }

    public void PopulateIslands()
    {
        for (int col = 0; col < Cols; ++col)
        {
            for (int row = 0; row < Rows; ++row)
                _cells[col][row].AddIslandPiece();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int col = 0; col < Cols; ++col)
        {
            for (int row = 0; row < Rows; ++row)
            {
                Cell c = _cells[col][row];
                Gizmos.DrawWireCube(c.transform.position, c.transform.localScale);
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Setup cell neighbours.
        for (int col = 0; col < Cols; ++col)
        {
            for (int row = 0; row < Rows; ++row)
            {
                var neighbours = new List<Cell>();

                AddCell(this[col - 1, row], neighbours); // West
                AddCell(this[col, row + 1], neighbours); // South
                AddCell(this[col + 1, row], neighbours); // East
                AddCell(this[col, row - 1], neighbours); // North

                this[col, row].neighbours = neighbours;
            }
        }
	}

    public Dictionary<Quadrant, List<Cell>> GetQuadrants()
    {
        var result = new Dictionary<Quadrant, List<Cell>>();

        // Top left
        var topLeft = new List<Cell>();
        // Col: 0 to Floor(Cols / 2)
        // Row: 0 to Floor(Rows / 2)
        for (int col = 0; col < (int)Mathf.Floor(Cols / 2); ++col)
        {
            for (int row = 0; row < (int)Mathf.Floor(Rows / 2); ++row)
                topLeft.Add(_cells[col][row]);
        }

        // Top right
        var topRight = new List<Cell>();
        // Col: Floor(Cols / 2) + 1 to Cols
        // Row: 0 to Floor(Rows / 2)
        for (int col = (int)Mathf.Floor(Cols / 2); col < Cols; ++col)
        {
            for (int row = 0; row < (int)Mathf.Floor(Rows / 2); ++row)
                topRight.Add(_cells[col][row]);
        }

        // Bottom left
        var botLeft = new List<Cell>();
        // Col: 0 to Floor(Cols / 2)
        // Row: Floor(Rows / 2) + 1 to Rows
        for (int col = 0; col < (int)Mathf.Floor(Cols / 2); ++col)
        {
            for (int row = (int)Mathf.Floor(Rows / 2); row < Rows; ++row)
                botLeft.Add(_cells[col][row]);
        }

        // Bottom right
        var botRight = new List<Cell>();
        // Col: Floor(Cols / 2) + 1 to Cols
        // Row: Floor(Rows / 2) + 1 to Rows
        for (int col = (int)Mathf.Floor(Cols / 2); col < Cols; ++col)
        {
            for (int row = (int)Mathf.Floor(Rows / 2); row < Rows; ++row)
                botRight.Add(_cells[col][row]);
        }

        // Return four cell lists mapped to each quadrant.
        result.Add(Quadrant.TopLeft, topLeft);
        result.Add(Quadrant.TopRight, topRight);
        result.Add(Quadrant.BotLeft, botLeft);
        result.Add(Quadrant.BotRight, botRight);

        return result;
       
    }

    private void AddCell(Cell cell, ICollection<Cell> list)
    {
        if (cell != null)
            list.Add(cell);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
