﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding
{
    public class Grid2D : MonoBehaviour
    {
         public Vector3 gridWorldSize;
        public float nodeRadius;
        public Node2D[,] Grid;
        public Tilemap obstacleMap;
        public List<Node2D> path;
        Vector3 worldBottomLeft;

        float nodeDiameter;
        public int gridSizeX, gridSizeY;

        void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        

        public void CreateGrid()
        {
            Grid = new Node2D[gridSizeX, gridSizeY];
            worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    Grid[x, y] = new Node2D(false, worldPoint, x, y);

                    if (obstacleMap.HasTile(obstacleMap.WorldToCell(Grid[x, y].worldPosition)))
                        Grid[x, y].SetObstacle(true);
                    else
                        Grid[x, y].SetObstacle(false);
                }
            }
        }


        //gets the neighboring nodes in the 4 cardinal directions.
        public List<Node2D> GetNeighbors(Node2D node)
        {
            List<Node2D> neighbors = new List<Node2D>();

            //checks and adds top neighbor
            if (node.GridX >= 0 && node.GridX < gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX, node.GridY + 1]);

            //checks and adds bottom neighbor
            if (node.GridX >= 0 && node.GridX < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX, node.GridY - 1]);

            //checks and adds right neighbor
            if (node.GridX + 1 >= 0 && node.GridX + 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
                neighbors.Add(Grid[node.GridX + 1, node.GridY]);

            //checks and adds left neighbor
            if (node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
                neighbors.Add(Grid[node.GridX - 1, node.GridY]);
            
            return neighbors;
        }
        
        public Node2D NodeFromWorldPoint(Vector3 worldPosition)
        {
            // Let unity convert from world to cell coord
            var cellPos = obstacleMap.WorldToCell(worldPosition);
            
            // Make the (0, 0) tile to be bottom left tile
            cellPos.x += gridSizeX / 2;
            cellPos.y += gridSizeY / 2;
            
            return Grid[cellPos.x, cellPos.y];

            // R: This calculation is expecting the tileMap to be aligned to the bottom left corner of the camera frustum
            // int x = Mathf.RoundToInt(worldPosition.x - 1 + (gridSizeX / 2));
            // int y = Mathf.RoundToInt(worldPosition.y + (gridSizeY / 2));
            // return Grid[x, y];
        }


        
        //Draws visual representation of grid
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

            if (Grid != null)
            {
                foreach (Node2D n in Grid)
                {
                    if (n.obstacle)
                        Gizmos.color = Color.red;
                    else
                        Gizmos.color = Color.white;

                    if (path != null && path.Contains(n))
                        Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeRadius));

                }
            }
        }
    }
}
