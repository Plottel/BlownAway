using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    using QuadMap = Dictionary<Quadrant, List<Cell>>;

    public class GridCon
    {
        public static float ISLAND_SPEED = 3f;
        public static float SPLIT_DIST = 20;

        private static GridCon _instance;

        public static GridCon Instance
        {
            get { return _instance; }
        }

        public void ReformGrid(Grid grid)
        {
            Vector3 dest = grid.transform.Mid3D();

            for (int col = 0; col < grid.cols; ++col)
            {
                for (int row = 0; row < grid.rows; ++row)
                {
                    if (grid[col, row].islandPiece != null)
                        grid[col, row].islandPiece.SetPathDirect(dest, ISLAND_SPEED);
                }
            }
        }

        public void SplitGridIntoFour(Grid grid)
        {
            grid.cols = grid.ColCellCount;
            grid.rows = grid[0].Count;

            Debug.Log("Grid Cols" + grid.cols);
            Debug.Log("Grid Rows: " + grid.rows);
            Debug.Log("Is Cells Null? " + grid._cells == null);

            QuadMap quadrants = grid.GetQuadrants();

            Vector3 mid = grid.transform.Mid3D();

            // Position where top left island pieces will move to.
            //Vector3 normToTopLeft = mid - grid[0, 0].transform.Mid3D();
            //Vector3 topLeftDest = mid + (normToTopLeft * SPLIT_DIST);

            // Position where top right island pieces will move to.
            Vector3 normToTopRight = mid - grid[grid.cols - 1, 0].transform.Mid3D();
            Vector3 topRightDest = mid + (normToTopRight * SPLIT_DIST);       

            // Position where bottom left island pieces will move to.
            Vector3 normToBotleft = mid - grid[0, grid.rows - 1].transform.Mid3D();
            Vector3 botLeftDest = mid + (normToBotleft * SPLIT_DIST);
            // Position where bot right island pieces will move to.
            Vector3 normToBotRight = mid - grid[grid.cols - 1, grid.rows - 1].transform.Mid3D();
            Vector3 botRightDest = mid + (normToBotRight * SPLIT_DIST);

            // Move the island pieces to their respective destinations.
            // Takes into account offsets so quadrant moves as one whole.
            MoveCellsToOffset(quadrants[Quadrant.BotLeft], botLeftDest);
            MoveCellsToOffset(quadrants[Quadrant.TopRight], topRightDest);
            //MoveCellsToOffset(quadrants[Quadrant.TopLeft], topLeftDest);
            MoveCellsToOffset(quadrants[Quadrant.BotRight], botRightDest);
        }

        private void MoveCellsToOffset(List<Cell> cells, Vector3 target)
        {
            foreach (Cell c in cells)
            {
                Vector3 offsetTarget = target = c.transform.Mid3D();

                if (c.islandPiece != null)
                    c.islandPiece.SetPathDirect(offsetTarget, ISLAND_SPEED);
            }
        }

        // Use this for initialization
        public GridCon()
        {
            if (_instance == null)
                _instance = this;
            else
                throw new System.Exception("Cannot have more than one instance of GridCon");
        }
    }
}


