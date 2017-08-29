using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    using QuadMap = Dictionary<Quadrant, List<Cell>>;

    public static class GridCon
    {
        public static float ISLAND_SPEED = 4f;
        public static float SPLIT_DIST = 4f;

        public static void ReformGrid(Grid grid, params object[] args)
        {
            Debug.DrawRay(grid.MidCell.transform.Mid3D(), Vector3.up, Color.blue, 5, false);

            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    if (c.islandPiece != null)
                        c.islandPiece.SetPath(c.transform.Mid3D(), ISLAND_SPEED);
                }
            }
        }

        public static void MoveGridBy(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments instead of 1 passed to MoveGridBy(Vector3)");

            Vector3 moveBy = (Vector3)args[0];

            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    if (c.islandPiece != null)
                        c.islandPiece.SetPath(c.transform.position + moveBy, ISLAND_SPEED);
                }
            }
        }

        public static void SplitGridIntoFour(Grid grid, params object[] args)
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

        public static void SwapTwoCells(Grid grid, params object[] args)
        {
            int RAISE_DIST = 3;
            int SPLIT_DIST = 1;

            if (args.Length != 2)
                Debug.LogError(args.Length + " arguments passed instead of 2 to SwapTwoPieces");

            Cell c1 = (Cell)args[0];
            Cell c2 = (Cell)args[1];

            var c1Waypoints = new List<Vector3>();
            var c2Waypoints = new List<Vector3>();

            // Waypoint 1 - go straight up
            c1Waypoints.Add(c1.transform.position + new Vector3(0, RAISE_DIST, 0));
            c2Waypoints.Add(c2.transform.position + new Vector3(0, RAISE_DIST, 0));

            // Waypoint 2 - split 90 degrees
            Vector3 from1To2Raised = c2.transform.position - c1.transform.position;
            from1To2Raised.y += 5;
            //Quarternion.Euler(x, y, z)
            // One of them needs to be 90, dunno which. Test.
            Vector3 normedPerp = Quaternion.Euler(0, 0, 90) * from1To2Raised;
            normedPerp.y = 0; // Don't want to move on Y-axis.
            normedPerp.Normalize();

            // C1 minus -> C2 plus
            // Go in opposite directions
            c1Waypoints.Add(c1Waypoints[0] + (normedPerp * SPLIT_DIST));
            c2Waypoints.Add(c2Waypoints[0] - (normedPerp * SPLIT_DIST));

            // Waypoint 3 head towards other cell at offset same as split
            // Calculated by projecting vector from Waypoint 2 the opposite way and assigning to opposite cell.
            // i.e. if Cell 1 went left, then Cell 2's waypoint is result if Cell 2 instead went right.
            c1Waypoints.Add(c2Waypoints[0] + (normedPerp * SPLIT_DIST));
            c2Waypoints.Add(c1Waypoints[0] - (normedPerp * SPLIT_DIST));

            // Waypoint 4 - split 90 degrees back into being above cell
            c1Waypoints.Add(c2Waypoints[0]);
            c2Waypoints.Add(c1Waypoints[0]);

            // Waypoint 5 - Back to original cell position, but swapped.
            c1Waypoints.Add(c2Waypoints[0] - new Vector3(0, RAISE_DIST, 0));
            c2Waypoints.Add(c1Waypoints[0] - new Vector3(0, RAISE_DIST, 0));


            // Set path for two cells
            if (c1.islandPiece != null)
                c1.islandPiece.SetPath(c1Waypoints, ISLAND_SPEED);
            if (c2.islandPiece != null)
                c2.islandPiece.SetPath(c2Waypoints, ISLAND_SPEED);


            // Swap island piece ownership
            IslandPiece temp = c1.islandPiece;
            c1.islandPiece = c2.islandPiece;
            c2.islandPiece = temp;
        }

        private static void MoveCellsAsGroup(List<Cell> cells, Vector3 target, Vector3 offsetFrom)
        {
            foreach (Cell c in cells)
            {
                Vector3 adjustedTarget = target + (c.transform.Mid3D() - offsetFrom);

                if (c.islandPiece != null)
                    c.islandPiece.SetPath(adjustedTarget, ISLAND_SPEED);
            }
        }
    }
}