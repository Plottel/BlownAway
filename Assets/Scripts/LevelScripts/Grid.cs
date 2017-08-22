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

    private List<List<Cell>> cells = new List<List<Cell>>(); 

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

        cells.Add(newCol);
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

            cells[col].Add(CreateCellAt(new Vector3(x, y, z)));
        }

        ++rows;
    }

    public void RemoveColumn()
    {
        for (int row = 0; row < rows; ++row)
        {
            DestroyImmediate(cells.Last()[row].gameObject);
            cells.Last()[row] = null;
        }

        cells.Remove(cells.Last());

        --cols;
    }

    public void RemoveRow()
    {
        for (int col = 0; col < cols; ++col)
        {
            DestroyImmediate(cells[col].Last().gameObject);
            cells[col][rows - 1] = null;
            cells[col].RemoveAt(rows - 1);
        }

        --rows;
    }

    public void PopulateIslands()
    {
        for (int col = 0; col < cols; ++col)
        {
            for (int row = 0; row < rows; ++row)
                cells[col][row].AddIslandPiece();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int col = 0; col < cols; ++col)
        {
            for (int row = 0; row < rows; ++row)
            {
                Cell c = cells[col][row];
                Gizmos.DrawWireCube(c.transform.position, c.transform.localScale);
            }
        }
    }

    void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
