using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    public Cell cell;

    public int cellSize = 1;

    public int cols = 0;
    public int rows = 0;

    private List<List<Cell>> _cells = new List<List<Cell>>(); 

    public Cell this[int col, int row]
    {
        get { return _cells[col][row]; }
    }

    private Cell CreateCellAt(Vector3 pos)
    {
        Cell newCell = Instantiate(cell, pos, Quaternion.identity);
        newCell.transform.SetParent(this.gameObject.transform);

        return newCell;
    }

    public void AddColumn()
    {
        var newCol = new List<Cell>();

        // For each row in the newly created column.
        for (int row = 0; row < rows; ++row)
        {
            float x = transform.position.x + cols * cellSize;
            float y = transform.position.y;
            float z = transform.position.z + row * cellSize;

            newCol.Add(CreateCellAt(new Vector3(x, y, z)));
        }

        _cells.Add(newCol);
        ++cols;
    }    

    public void AddRow()
    {
        //For each column to have a new row added to it
        for (int col = 0; col < cols; ++col)
        {
            float x = transform.position.x + col * cellSize;
            float y = transform.position.y;
            float z = transform.position.z + rows * cellSize;

            _cells[col].Add(CreateCellAt(new Vector3(x, y, z)));
        }

        ++rows;
    }

    public void RemoveColumn()
    {
        for (int row = 0; row < rows; ++row)
        {
            DestroyImmediate(_cells.Last()[row].gameObject);
            _cells.Last()[row] = null;
        }

        _cells.Remove(_cells.Last());

        --cols;
    }

    public void RemoveRow()
    {
        for (int col = 0; col < cols; ++col)
        {
            DestroyImmediate(_cells[col].Last().gameObject);
            _cells[col][rows - 1] = null;
            _cells[col].RemoveAt(rows - 1);
        }

        --rows;
    }

    public void PopulateIslands()
    {
        for (int col = 0; col < cols; ++col)
        {
            for (int row = 0; row < rows; ++row)
                _cells[col][row].AddIslandPiece();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int col = 0; col < cols; ++col)
        {
            for (int row = 0; row < rows; ++row)
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
        for (int col = 0; col < cols; ++col)
        {
            for (int row = 0; row < rows; ++row)
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

    public void SplitIntoFour()
    {
        // Top left
        var topLeft = new List<Cell>();
        // Col: 0 to Floor(Cols / 2)
        // Row: 0 to Floor(Rows / 2)
        for (int col = 0; col < (int)Mathf.Floor(cols / 2); ++col)
        {
            for (int row = 0; row < (int)Mathf.Floor(rows / 2); ++row)
                topLeft.Add(_cells[col][row]);
        }

        // Top right
        var topRight = new List<Cell>();
        // Col: Floor(Cols / 2) + 1 to Cols
        // Row: 0 to Floor(Rows / 2)
        for (int col = (int)Mathf.Floor(cols / 2); col < cols; ++col)
        {
            for (int row = 0; row < (int)Mathf.Floor(rows / 2); ++row)
                topRight.Add(_cells[col][row]);
        }

        // Bottom left
        var botLeft = new List<Cell>();
        // Col: 0 to Floor(Cols / 2)
        // Row: Floor(Rows / 2) + 1 to Rows
        for (int col = 0; col < (int)Mathf.Floor(cols / 2); ++col)
        {
            for (int row = (int)Mathf.Floor(rows / 2); row < rows; ++row)
                botLeft.Add(_cells[col][row]);
        }

        // Bottom right
        var botRight = new List<Cell>();
        // Col: Floor(Cols / 2) + 1 to Cols
        // Row: Floor(Rows / 2) + 1 to Rows
        for (int col = (int)Mathf.Floor(cols / 2); col < cols; ++col)
        {
            for (int row = (int)Mathf.Floor(rows / 2); row < rows; ++row)
                botRight.Add(_cells[col][row]);
        }

        // Get vector from centre to each corner
        // Island travels along that vector outwards until a stopping point.
       
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
