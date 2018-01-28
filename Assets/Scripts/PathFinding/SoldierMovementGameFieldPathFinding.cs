using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Tuple;

namespace PathFinding
{
    public class SoldierMovementGameFieldPathFinding
    {
        private SoldierMovementManager _soldierMovementManager;
        private GameObject[,] _battleFieldObjects;
        private GameObject[,] _battleFieldBlocks;

        private Vector2Int SoldierPosition
        {
            get
            {
                return new Vector2Int((int) _soldierMovementManager.transform.position.x,
                    (int) _soldierMovementManager.transform.position.y);
            }
        }

        public SoldierMovementGameFieldPathFinding(SoldierMovementManager soldierMovementManager,
            GameObject[,] battleFieldObjects,
            GameObject[,] battleFieldBlocksBlocks)
        {
            _soldierMovementManager = soldierMovementManager;
            _battleFieldObjects = battleFieldObjects;
            _battleFieldBlocks = battleFieldBlocksBlocks;
        }

        public Dictionary<Vector2Int, Vector2Int[]> GetReachablePaths()
        {
            var source = SoldierPosition;
            var availablePaths = new Dictionary<Vector2Int, Vector2Int[]>();

            var bounds = GetFixedLowerAndUpperPositionsOfSoldier();
            var minPoint = bounds.Item1;
            var maxPoint = bounds.Item2;

            var dist = new double[_battleFieldBlocks.GetLength(0), _battleFieldBlocks.GetLength(1)];
            var prev = new Vector2Int?[_battleFieldBlocks.GetLength(0), _battleFieldBlocks.GetLength(1)];
            var queue = new List<Vector2Int>();
            var inspectedNodes = new List<Vector2Int>();


            // init dist to inf, prev to null and init queue
            for (var i = minPoint.x; i <= maxPoint.x; i++)
            {
                for (var j = minPoint.y; j <= maxPoint.y; j++)
                {
                    var newNode = new Vector2Int(i, j);

                    if (IsValidBlockToConsider(newNode))
                    {
                        dist[i, j] = double.PositiveInfinity;
                        prev[i, j] = null;
                        queue.Add(new Vector2Int(i, j));
                        inspectedNodes.Add(newNode);
                    }
                }
            }

            dist[source.x, source.y] = 0;

            while (queue.Count > 0)
            {
                var item = PopNodeWithShortestDistance(queue, dist);

                foreach (var neighbor in NeighborsOfNode(item))
                {
                    var alternativePathsDistance = dist[item.x, item.y] + DistanceBetweenNodes(item, neighbor);

                    if (alternativePathsDistance < dist[neighbor.x, neighbor.y])
                    {
                        if (alternativePathsDistance > _soldierMovementManager.RemainingFarDistance)
                        {
                            continue;
                        }
                        prev[neighbor.x, neighbor.y] = item;
                        dist[neighbor.x, neighbor.y] = alternativePathsDistance;
                    }
                }
            }

            foreach (var inspectedNode in inspectedNodes)
            {
                var path = CalculatePathToNode(inspectedNode, prev);
                if (path != null)
                {
                    path.Add(inspectedNode);
                    availablePaths.Add(inspectedNode, path.ToArray());
                }
            }

            return availablePaths;
        }

        private Vector2Int PopNodeWithShortestDistance(List<Vector2Int> queue, double[,] dist)
        {
            var nodeWithShortestDistance = queue[0];
            var lastShortestDistance = dist[nodeWithShortestDistance.x, nodeWithShortestDistance.y];

            foreach (var node in queue)
            {
                if (dist[node.x, node.y] < lastShortestDistance)
                {
                    nodeWithShortestDistance = node;
                    lastShortestDistance = dist[node.x, node.y];
                }
            }

            queue.Remove(nodeWithShortestDistance);
            return nodeWithShortestDistance;
        }

        private List<Vector2Int> CalculatePathToNode(Vector2Int node, Vector2Int?[,] prevMap)
        {
            var prev = prevMap[node.x, node.y];

            if (prev == null)
            {
                return null;
            }

            if (prev.Equals(SoldierPosition))
            {
                return new List<Vector2Int>(new[] {SoldierPosition});
            }

            var path = CalculatePathToNode((Vector2Int) prev, prevMap);
            path.Add((Vector2Int) prev);
            return path;
        }

        private double DistanceBetweenNodes(Vector2Int node1, Vector2Int node2)
        {
            return 1;
        }

        private Vector2Int[] NeighborsOfNode(Vector2Int node)
        {
            var neighbors = new List<Vector2Int>();

            // Top
            AddNeighborIfIsInsideBoundsAndNotBlockedByWall(node, 0, 1, neighbors);
            // Right
            AddNeighborIfIsInsideBoundsAndNotBlockedByWall(node, 1, 0, neighbors);
            // Bottom
            AddNeighborIfIsInsideBoundsAndNotBlockedByWall(node, -1, 0, neighbors);
            // Left
            AddNeighborIfIsInsideBoundsAndNotBlockedByWall(node, 0, -1, neighbors);

            return neighbors.ToArray();
        }

        private void AddNeighborIfIsInsideBoundsAndNotBlockedByWall(Vector2Int node, int deltaX, int deltaY,
            List<Vector2Int> neighbors)
        {
            var newPosition = new Vector2Int(
                node.x + deltaX,
                node.y + deltaY
            );

            if (IsValidBlockToConsider(newPosition) && !NodesBlockedByWall(node, newPosition))
            {
                neighbors.Add(newPosition);
            }
        }

        private bool NodesBlockedByWall(Vector2Int node1, Vector2Int node2)
        {
            var fieldBlock = _battleFieldBlocks[node1.x, node1.y].GetComponent<BattleFieldBlock>();
            var delta = node2 - node1;

            if (delta.x < 0 && fieldBlock.HasLeftWall)
            {
                return true;
            }

            if (delta.x > 0 && fieldBlock.HasRightWall)
            {
                return true;
            }

            if (delta.y < 0 && fieldBlock.HasBottomWall)
            {
                return true;
            }

            if (delta.y > 0 && fieldBlock.HasTopWall)
            {
                return true;
            }

            return false;
        }

        private bool IsValidBlockToConsider(Vector2Int position)
        {
            if (!IsInsideBoundsOfPossibleWalkingArea(position))
            {
                return false;
            }

            // Only true if there is no other object at the given position
            return _battleFieldObjects[position.x, position.y] == null || position == SoldierPosition;
        }

        private bool IsInsideBoundsOfPossibleWalkingArea(Vector2Int position)
        {
            var bounds = GetFixedLowerAndUpperPositionsOfSoldier();
            var minPoint = bounds.Item1;
            var maxPoint = bounds.Item2;

            return minPoint.x <= position.x && minPoint.y <= position.y &&
                   position.x <= maxPoint.x && position.y <= maxPoint.y &&
                   IsInsideWalkingRadius(position);
        }

        private bool IsInsideWalkingRadius(Vector2Int point)
        {
            var radius = _soldierMovementManager.FarWalkingDistance;
            var center = SoldierPosition;
            return Math.Pow(point.x - center.x, 2) + Math.Pow(point.y - center.y, 2) < Math.Pow(radius, 2);
        }

        private Utility.Tuple.Tuple<Vector2Int, Vector2Int> GetFixedLowerAndUpperPositionsOfSoldier()
        {
            // calculate bounds (only inspect areas that are possibly reachable)
            var minPoint = _soldierMovementManager.MinWalkingPoint; // Soldier will take care of checking if x||y < 0
            var maxPoint = _soldierMovementManager.MaxWalkingPoint;

            if (maxPoint.x >= _battleFieldBlocks.GetLength(0))
            {
                maxPoint.x = _battleFieldBlocks.GetLength(0) - 1;
            }

            if (maxPoint.y >= _battleFieldBlocks.GetLength(1))
            {
                maxPoint.y = _battleFieldBlocks.GetLength(1) - 1;
            }

            return new Utility.Tuple.Tuple<Vector2Int, Vector2Int>(minPoint, maxPoint);
        }
    }
}