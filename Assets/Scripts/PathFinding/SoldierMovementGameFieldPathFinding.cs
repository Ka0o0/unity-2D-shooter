using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace PathFinding
{
    public class SoldierMovementGameFieldPathFinding
    {
        private Soldier _soldier;
        private GameObject[,] _battleField;

        private Vector2Int SoldierPosition
        {
            get { return new Vector2Int((int) _soldier.transform.position.x, (int) _soldier.transform.position.y); }
        }

        public SoldierMovementGameFieldPathFinding(Soldier soldier, GameObject[,] battleField)
        {
            _soldier = soldier;
            _battleField = battleField;
        }

        public List<Utility.Tuple.Tuple<Vector2Int, Vector2Int[]>> GetReachablePaths()
        {
            var source = SoldierPosition;
            var availablePaths = new List<Utility.Tuple.Tuple<Vector2Int, Vector2Int[]>>();

            var bounds = GetFixedLowerAndUpperPositionsOfSoldier();
            var minPoint = bounds.Item1;
            var maxPoint = bounds.Item2;

            var dist = new double[_battleField.GetLength(0), _battleField.GetLength(1)];
            var prev = new Vector2Int?[_battleField.GetLength(0), _battleField.GetLength(1)];
            var queue = new List<Vector2Int>();
            var inspectedNodes = new List<Vector2Int>();


            // init dist to inf, prev to null and init queue
            for (var i = minPoint.x; i <= maxPoint.x; i++)
            {
                for (var j = minPoint.y; j <= maxPoint.y; j++)
                {
                    var newNode = new Vector2Int(i, j);

                    if (IsInsideBoundsOfPossibleWalkingArea(newNode))
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
                    availablePaths.Add(
                        new Utility.Tuple.Tuple<Vector2Int, Vector2Int[]>(inspectedNode, path.ToArray()));
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
            AddNeighborIfIsInsideBoundsAndNotBlockedByObstacle(node, 0, 1, neighbors);
            // Right
            AddNeighborIfIsInsideBoundsAndNotBlockedByObstacle(node, 1, 0, neighbors);
            // Bottom
            AddNeighborIfIsInsideBoundsAndNotBlockedByObstacle(node, -1, 0, neighbors);
            // Left
            AddNeighborIfIsInsideBoundsAndNotBlockedByObstacle(node, 0, -1, neighbors);

            return neighbors.ToArray();
        }

        private void AddNeighborIfIsInsideBoundsAndNotBlockedByObstacle(Vector2Int node, int deltaX, int deltaY,
            List<Vector2Int> neighbors)
        {
            var newPosition = new Vector2Int(
                node.x + deltaX,
                node.y + deltaY
            );

            if (IsInsideBoundsOfPossibleWalkingArea(newPosition) && !NodesBlockedByObstacle(node, newPosition))
            {
                neighbors.Add(newPosition);
            }
        }

        private bool NodesBlockedByObstacle(Vector2Int node1, Vector2 node2)
        {
            // TODO Implement me
            return false;
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
            var radius = _soldier.FarWalkingDistance;
            var center = SoldierPosition;
            return Math.Pow(point.x - center.x, 2) + Math.Pow(point.y - center.y, 2) < Math.Pow(radius, 2);
        }

        private Utility.Tuple.Tuple<Vector2Int, Vector2Int> GetFixedLowerAndUpperPositionsOfSoldier()
        {
            // calculate bounds (only inspect areas that are possibly reachable)
            var minPoint = _soldier.MinWalkingPoint; // Soldier will take care of checking if x||y < 0
            var maxPoint = _soldier.MaxWalkingPoint;

            if (maxPoint.x >= _battleField.GetLength(0))
            {
                maxPoint.x = _battleField.GetLength(0) - 1;
            }

            if (maxPoint.y >= _battleField.GetLength(1))
            {
                maxPoint.y = _battleField.GetLength(1) - 1;
            }

            return new Utility.Tuple.Tuple<Vector2Int, Vector2Int>(minPoint, maxPoint);
        }
    }
}