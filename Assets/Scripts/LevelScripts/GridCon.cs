using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    using QuadMap = Dictionary<Quadrant, List<Cell>>;

    public class GridCon
    {
        public static float ISLAND_SPEED = 2f;
        public static float SPLIT_DIST = 4f;

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
        
        public void MoveGridFullLength(Grid grid)
        {
            Vector3 destination = grid.transform.position - new Vector3(grid.Cols / 2, 0, 0);
            MoveCellsAsGroup(grid.CellsAsList, destination, grid.MidCell.transform.Mid3D());
        }

        public void SplitGridIntoFour(Grid grid)
        {
            // Need to normalize all "norm" vectors
            QuadMap quadrants = grid.GetQuadrants();

            // For easy reference...
            Vector3 mid = grid.MidCell.transform.Mid3D();
            Vector3 topLeft = grid[0, 0].transform.Mid3D();
            Vector3 topRight = grid[grid.Cols - 1, 0].transform.Mid3D();
            Vector3 botLeft = grid[0, grid.Rows - 1].transform.Mid3D();
            Vector3 botRight = grid[grid.Cols - 1, grid.Rows - 1].transform.Mid3D();

            // Position where top left island pieces will move to.
            Vector3 normToTopLeft = (topLeft - mid).normalized;
            Vector3 topLeftDest = topLeft + (normToTopLeft * SPLIT_DIST);

            // Position where top right island pieces will move to.
            Vector3 normToTopRight = (topRight - mid).normalized;
            Vector3 topRightDest = topRight + (normToTopRight * SPLIT_DIST);       

            // Position where bottom left island pieces will move to.
            Vector3 normToBotleft = (botLeft - mid).normalized;
            Vector3 botLeftDest = botLeft + (normToBotleft * SPLIT_DIST);

            // Position where bot right island pieces will move to.
            Vector3 normToBotRight = (botRight - mid).normalized;
            Vector3 botRightDest = botRight + (normToBotRight * SPLIT_DIST);

            // Move the island pieces to their respective destinations.
            // Takes into account offsets so quadrant moves as one whole.
            MoveCellsAsGroup(quadrants[Quadrant.BotLeft], botLeftDest, botLeft);
            MoveCellsAsGroup(quadrants[Quadrant.TopRight], topRightDest, topRight);
            MoveCellsAsGroup(quadrants[Quadrant.TopLeft], topLeftDest, topLeft);
            MoveCellsAsGroup(quadrants[Quadrant.BotRight], botRightDest, botRight);
        }

        private void MoveCellsAsGroup(List<Cell> cells, Vector3 target, Vector3 offsetFrom)
        {
            foreach (Cell c in cells)
            {
                Vector3 adjustedTarget = target + (c.transform.Mid3D() - offsetFrom);

                if (c.islandPiece != null)
                    c.islandPiece.SetPathDirect(adjustedTarget, ISLAND_SPEED);
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


