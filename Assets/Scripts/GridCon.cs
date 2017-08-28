using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    using QuadMap = Dictionary<Quadrant, List<Cell>>;

    public class GridCon
    {
        public static float ISLAND_SPEED = 3f;
        public static float SPLIT_DIST = 50;

        private static GridCon _instance;

        public static GridCon Instance
        {
            get { return _instance; }
        }

        public void ReformGrid(Grid grid)
        {
            Debug.DrawRay(grid.MidCell.transform.Mid3D(), Vector3.up, Color.blue, 5, false);

            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    if (c.islandPiece != null)
                        c.islandPiece.SetPathDirect(c.transform.Mid3D(), ISLAND_SPEED);
                }
            }
        }

        public void SplitGridIntoFour(Grid grid)
        {
            // Need to normalize all "norm" vectors
            QuadMap quadrants = grid.GetQuadrants();

            Vector3 mid = grid.MidCell.transform.Mid3D();

            // Position where top left island pieces will move to.
            Vector3 normToTopLeft = grid[0, 0].transform.Mid3D() - mid;
            Vector3 topLeftDest = mid + (normToTopLeft * SPLIT_DIST);

            // Position where top right island pieces will move to.
            Vector3 normToTopRight = grid[grid.Cols - 1, 0].transform.Mid3D() - mid;
            Vector3 topRightDest = mid + (normToTopRight * SPLIT_DIST);       

            // Position where bottom left island pieces will move to.
            Vector3 normToBotleft = grid[0, grid.Rows - 1].transform.Mid3D() - mid;
            Vector3 botLeftDest = mid + (normToBotleft * SPLIT_DIST);
            // Position where bot right island pieces will move to.
            Vector3 normToBotRight = grid[grid.Cols - 1, grid.Rows - 1].transform.Mid3D() - mid;
            Vector3 botRightDest = mid + (normToBotRight * SPLIT_DIST);

            // Move the island pieces to their respective destinations.
            // Takes into account offsets so quadrant moves as one whole.
            MoveCellsToOffset(quadrants[Quadrant.BotLeft], botLeftDest);
            MoveCellsToOffset(quadrants[Quadrant.TopRight], topRightDest);
            MoveCellsToOffset(quadrants[Quadrant.TopLeft], topLeftDest);
            MoveCellsToOffset(quadrants[Quadrant.BotRight], botRightDest);
        }

        private void MoveCellsToOffset(List<Cell> cells, Vector3 target)
        {
            foreach (Cell c in cells)
            {
                Vector3 offsetTarget = target - c.transform.Mid3D();

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


