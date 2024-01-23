using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class Pathfinding2D : MonoBehaviour
    {
        public Transform seeker, target;
        Grid2D _grid;
        Node2D _seekerNode, _targetNode;
        public GameObject gridOwner;    
        public List<Node2D> path = null;


        void Start()
        {
            gridOwner = GameObject.FindWithTag("GridOwner");
            //Instantiate grid
            _grid = gridOwner.GetComponent<Grid2D>();
        }


        public void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            //get player and target position in grid coords
            _seekerNode = _grid.NodeFromWorldPoint(startPos);
            _targetNode = _grid.NodeFromWorldPoint(targetPos);

            List<Node2D> openSet = new List<Node2D>();
            HashSet<Node2D> closedSet = new HashSet<Node2D>();
            openSet.Add(_seekerNode);
            
            //calculates path for pathfinding
            while (openSet.Count > 0)
            {

                //iterates through openSet and finds lowest FCost
                Node2D node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost <= node.FCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                //If target found, retrace path
                if (node == _targetNode)
                {
                    RetracePath(_seekerNode, _targetNode);
                    return;
                }
                
                //adds neighbor nodes to openSet
                foreach (Node2D neighbour in _grid.GetNeighbors(node))
                {
                    if (neighbour.obstacle)
                    {
                        var hitData = Physics2D.Raycast(neighbour.worldPosition, Vector2.zero, 0);
                        if (hitData)
                        {
                            if (neighbour.worldPosition != _targetNode.worldPosition)
                            {
                             continue;   
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, _targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        //reverses calculated path so first node is closest to seeker
        void RetracePath(Node2D startNode, Node2D endNode)
        {
            path = new List<Node2D>();
            Node2D currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            _grid.path = path;

        }

        //gets distance between 2 nodes for calculating cost
        int GetDistance(Node2D nodeA, Node2D nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        public void UpdateGrid()
        {
            gridOwner = GameObject.FindWithTag("GridOwner");
            _grid = gridOwner.GetComponent<Grid2D>();
            _grid.CreateGrid();
        }
    }
}