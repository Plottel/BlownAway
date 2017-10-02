using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine
{
    using QuadMap = Dictionary<Quadrant, List<Cell>>;

    public static class GridCon
    {
        private static List<Cell> _offScreenPiecesToDelete = new List<Cell>();

        public static float ISLAND_SPEED = 20f;
        public static float SPLIT_DIST = 4f;

        public static void CleanUpOffScreenPieces()
        {
            for (int i = _offScreenPiecesToDelete.Count - 1; i >= 0; --i)
            {
                Cell c = _offScreenPiecesToDelete[i];

                // Validate that it's a real cell.
                if (c == null)
                {
                    _offScreenPiecesToDelete.RemoveAt(i);
                }
                else
                {
                    if (c.islandPiece.HasArrived)
                    {
                        Object.Destroy(c.gameObject);
                        _offScreenPiecesToDelete.RemoveAt(i);
                    }
                }                
            }
        }

        /// <summary>
        /// Num Args: 0
        /// </summary>
        public static void ReformGrid(Grid grid, params object[] args)
        {
            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    if (c.islandPiece != null)
                    {
                        c.islandPiece.SetPath(c.transform.Mid3D(), ISLAND_SPEED, true);
                        ShakeCell(c, 10, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
                    }
                }
            }
        }

        /// <summary>
        /// Num Args: 1
        /// args[0]: Vector3 moveBy 
        /// </summary>
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
                    {
                        c.islandPiece.SetPath(c.transform.position + moveBy, ISLAND_SPEED, true);
                        ShakeCell(c, 20, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);

                    }
                }
            }
        }

        /// <summary>
        /// Num Args: 0
        /// </summary>
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

        /// <summary>
        /// Num Args: 2
        /// args[0]: Cell oldCell
        /// args[1]: TerrainType newTerrainType
        /// </summary>
        public static void ChangeCellTerrain(Grid grid, params object[] args)
        {
            if (args.Length != 2)
                Debug.LogError(args.Length + " arguments passed instead of 2 to ChangeCellTerrain");

            Cell oldCell = (Cell)args[0];
            TerrainType newTerrainType = (TerrainType)args[1];

            Vector3 spawnPos = GetOffScreenPosFor(grid, oldCell);
            Cell newCell = GridFactory.MakeTerrainCellAt(spawnPos, newTerrainType);

            ReplaceWithOffScreenPiece(grid, oldCell, newCell);
        }

        public static void ChangeCellTerrainMultiple(Grid grid, params object[] args)
        {
            if (args.Length != 2)
                Debug.LogError(args.Length + "arguments passed instead of 2 to ChanceCellTerrainMultiple");

            List<Cell> cells = (List<Cell>)args[0];
            TerrainType newTerrainType = (TerrainType)args[1];

            foreach (Cell c in cells)
            {
                Vector3 spawnPos = GetOffScreenPosFor(grid, c);
                Cell newCell = GridFactory.MakeTerrainCellAt(spawnPos, newTerrainType);
                ReplaceWithOffScreenPiece(grid, c, newCell);
            }
        }

        /// <summary>
        /// Num Args: 1
        /// args[0] Cell toDrop
        /// </summary>
        public static void DropCell(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments passed instead of 1 to DropCell");

            Cell toDrop = (Cell)args[0];

            if (toDrop.islandPiece == null)
                return;

            Vector3 dropTo = toDrop.transform.position - new Vector3(0, 10, 0);
            Cell destroyOnContact = GridFactory.MakeEmptyCellAt(dropTo);

            toDrop.islandPiece.SetPath(dropTo, ISLAND_SPEED, true);
            ShakeCell(toDrop, Grid.SHAKE_COUNT, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);

            destroyOnContact.islandPiece = toDrop.islandPiece;
            destroyOnContact.islandPiece.transform.SetParent(destroyOnContact.transform);

            //toDrop.islandPiece = null; //MAYBE NEEDED?????????
            _offScreenPiecesToDelete.Add(destroyOnContact);
        }

        /// <summary>
        /// Num Args: 1
        /// args[0]: List of Cell toDrop
        /// </summary>
        public static void DropCellMultiple(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments passed instead of 1 to DropCell");

            List<Cell> toDrop = (List<Cell>)args[0];

            foreach (Cell c in toDrop)
                DropCell(grid, c);
        }

        /// <summary>
        /// Num Args: 0
        /// </summary>
        public static void DropRandomCell(Grid grid, params object[] args)
        {
            if (args.Length != 0)
                Debug.LogError(args.Length + " arguments passed instead of 0 to DropRandomCell");

            DropCell(grid, grid[LevelManager.RNG.Next(0, grid.Cols - 1), LevelManager.RNG.Next(0, grid.Rows - 1)]);
        }


        /// <summary>
        /// Num Args: 2
        /// args[0]: Cell oldCell
        /// args[1]: Cell newCell
        /// </summary>
        public static void ReplaceWithOffScreenPiece(Grid grid, params object[] args)
        {
            if (args.Length != 2)
                Debug.LogError(args.Length + " argumnts passed instead of 2 to ReplaceWithOffScreenPiece");

            Cell oldCell = (Cell)args[0];
            Cell newCell = (Cell)args[1];

            //Vector2 oldCellIndex = grid.IndexOf(oldCell);
    
            SwapTwoCells(grid, oldCell, newCell);
            _offScreenPiecesToDelete.Add(newCell);
        }
        
        /// <summary>
        /// Num Args: 0
        /// </summary>
        public static void SwapRandomCells(Grid grid, params object[] args)
        { 
            SwapTwoCells(grid,
                grid[LevelManager.RNG.Next(0, grid.Cols - 1), LevelManager.RNG.Next(0, grid.Rows - 1)],
                grid[LevelManager.RNG.Next(0, grid.Cols - 1), LevelManager.RNG.Next(0, grid.Rows - 1)]);
        }

        /// <summary>
        /// Num Args: 1
        /// args[0]: int numSwaps
        /// </summary>
        public static void SwapRandomCellsMultiple(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments passed instead of 1 to SwapRandomCellsMultiple");

            int numSwaps = (int)args[0];

            for (int i = 0; i < numSwaps; ++i)
                SwapRandomCells(grid);
        }

        /// <summary>
        /// Num Args: 2
        /// args[0]: Cell c1
        /// args[1]: Cell c2
        /// </summary>
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
            //Vector3 from1To2Raised = c2.transform.position - c1.transform.position;
            //from1To2Raised.y += 5;
            ////Quarternion.Euler(x, y, z)
            //// One of them needs to be 90, dunno which. Test.
            //Vector3 normedPerp = Quaternion.Euler(0, 0, 90) * from1To2Raised;
            //normedPerp.y = 0; // Don't want to move on Y-axis.
            //normedPerp.Normalize();

            //// C1 minus -> C2 plus
            //// Go in opposite directions
            //c1Waypoints.Add(c1Waypoints[0] + (normedPerp * SPLIT_DIST));
            //c2Waypoints.Add(c2Waypoints[0] - (normedPerp * SPLIT_DIST));

            // Waypoint 3 head towards other cell at offset same as split
            // Calculated by projecting vector from Waypoint 2 the opposite way and assigning to opposite cell.
            // i.e. if Cell 1 went left, then Cell 2's waypoint is result if Cell 2 instead went right.
            //c1Waypoints.Add(c2Waypoints[0] + (normedPerp * SPLIT_DIST));
            //c2Waypoints.Add(c1Waypoints[0] - (normedPerp * SPLIT_DIST));

            // Waypoint 4 - split 90 degrees back into being above cell
            c1Waypoints.Add(c2Waypoints[0]);
            c2Waypoints.Add(c1Waypoints[0]);

            // Waypoint 5 - Back to original cell position, but swapped.
            c1Waypoints.Add(c2Waypoints[0] - new Vector3(0, RAISE_DIST, 0));
            c2Waypoints.Add(c1Waypoints[0] - new Vector3(0, RAISE_DIST, 0));


            // Set path for two cells
            if (c1.islandPiece != null)
            {
                c1.islandPiece.SetPath(c1Waypoints, ISLAND_SPEED, true);
                ShakeCell(c1, Grid.SHAKE_COUNT, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
            }

            if (c2.islandPiece != null)
            {
                c2.islandPiece.SetPath(c2Waypoints, ISLAND_SPEED, true);
                ShakeCell(c2, Grid.SHAKE_COUNT, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
            }


            // Swap island piece ownership
            IslandPiece temp = c1.islandPiece;

            c1.islandPiece = c2.islandPiece;
            c2.islandPiece = temp;           

            if (c1.islandPiece != null)
                c1.islandPiece.transform.SetParent(c1.transform);
            if (c2.islandPiece != null)
                c2.islandPiece.transform.SetParent(c2.transform);
        }

        /// <summary>
        /// Num Args: 2
        /// args[0]: List of Cell chunk1
        /// args[1]: List of Cell chunk2
        /// </summary>
        public static void SwapTwoChunks(Grid grid, params object[] args)
        {
            if (args.Length != 2)
                Debug.LogError(args.Length + " arguments send instead of 2 to ReplaceBorderWIthTrees()");

            var chunk1 = (List<Cell>)args[0];
            var chunk2 = (List<Cell>)args[1];

            if (chunk1.Count != chunk2.Count)
                Debug.LogError("Chunks not same size in SwapTwoChunks");

            for (int i = 0; i < chunk1.Count; ++i)
                SwapTwoCells(grid, chunk1[i], chunk2[i]);
        }

        /// <summary>
        /// Num Args: 0
        /// </summary>
        public static void ReplaceBorderWithTrees(Grid grid, params object[] args)
        {
            if (args.Length != 0)
                Debug.LogError(args.Length + " arguments sent instead of 0 to ReplaceBorderWithTrees()");

            foreach (Cell c in grid.Border)
            {
                var offScreenTree = GridFactory.MakeTreeCellAt(GetOffScreenPosFor(grid, c));

                ReplaceWithOffScreenPiece(grid, c, offScreenTree);
            }
        }

        /// <summary>
        /// Num Args: 1
        /// args[0]: List of Cell toRestore
        /// </summary>
        public static void RestoreEmptyCellMultiple(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments sent instead of 0 to RestoreEmptyCellMultiple()");

            List<Cell> toRestore = (List<Cell>)args[0];

            foreach (Cell c in toRestore)
                RestoreEmptyCell(grid, c);
        }

        /// <summary>
        /// Num Args: 1
        /// args[0]: Cell toRestore
        /// </summary>
        public static void RestoreEmptyCell(Grid grid, params object[] args)
        {
            if (args.Length != 1)
                Debug.LogError(args.Length + " arguments sent instead of 1 to RestoreEmptyCell()");

            Cell cell = (Cell)args[0];
            Cell offScreenPiece = GridFactory.MakeIslandPieceCellAt(GetOffScreenPosFor(grid, cell));

            ReplaceWithOffScreenPiece(grid, cell, offScreenPiece);
        }

        public static Vector3 GetOffScreenPosFor(Grid grid, Cell c)
        {
            Vector3 centerCellOffScreenPos = grid.MidCell.transform.position + new Vector3(20, 0, 0);

            Vector3 offScreenPos = centerCellOffScreenPos + (c.transform.position - grid.MidCell.transform.position);

            return offScreenPos;
            //Vector3 gridCtrToC = c.transform.position - grid.MidCell.transform.position;
            //return c.transform.position + (gridCtrToC.normalized * 25);
        }

        private static void MoveCellsAsGroup(List<Cell> cells, Vector3 target, Vector3 offsetFrom)
        {
            foreach (Cell c in cells)
            {
                Vector3 adjustedTarget = target + (c.transform.Mid3D() - offsetFrom);

                if (c.islandPiece != null)
                {
                    c.islandPiece.SetPath(adjustedTarget, ISLAND_SPEED, true);
                    ShakeCell(c, 10, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
                }
                    
            }
        }

        public static void ShakeCell(Cell c, float shakeCount, float shakeSpeed, float shakeDistance)
        {
            if (c.islandPiece != null)
            {
                var shakeWaypoints = new List<Vector3>();

                // FIX: HARDCODE SHAKE COUNT
                shakeCount = 7;

                for (int i = 0; i < shakeCount; ++i)
                {
                    Vector3 pos = c.islandPiece.transform.position;
                    Vector3 shake = new Vector3(0, shakeDistance, 0);

                    shakeWaypoints.Add(pos + shake);
                    shakeWaypoints.Add(pos - shake);
                }

                c.islandPiece.SetPath(shakeWaypoints, ISLAND_SPEED, false);
            }           
        }

        public static GridScene CreateGridScene(Grid grid, string name, Text contextualText)
        {
            Debug.Log("Name was: " + name);
            if (name == "Insane")
                return new GridScene_Tutorial(grid, contextualText);
            else if (name == "Ballista")
                return new GridScene_Ballistas(grid);

            return null;
        }
    }
}