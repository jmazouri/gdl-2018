using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI
{
    public class TileNavController : MonoBehaviour
    {
        [SerializeField] private Tilemap _navMap;
        private Vector3?[,] _pseudoGrid;

        void Start()
        {
            var bounds = _navMap.cellBounds;
            TileBase[] allTiles = _navMap.GetTilesBlock(bounds);
            _pseudoGrid = new Vector3?[bounds.size.x, bounds.size.y];

            for (int x = 0; x < _pseudoGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _pseudoGrid.GetLength(1); y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile == null) continue;

                    _pseudoGrid[x, y] = _navMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                }
            }
        }

        public IEnumerator ResolvePath(Vector3 realWorldPosition, Vector2Int pseudoTarget,
            BaseAIController requestingController)
        {
            var open = new List<NavNode>();
            var closed = new List<NavNode>();

            Vector2Int? pseudoGridPosition = GetPseudoPosition(realWorldPosition);

            if (!pseudoGridPosition.HasValue)
                throw new Exception(
                    $"Controller {requestingController.gameObject.name} requested destination to tile position {pseudoTarget} but current position of agent is {requestingController.transform.position} which is invalid!");
            if (pseudoTarget.x < 0 || pseudoTarget.x >= _pseudoGrid.GetLength(0) || pseudoTarget.y < 0 ||
                pseudoTarget.y >= _pseudoGrid.GetLength(1)) throw new ArgumentOutOfRangeException(nameof(pseudoTarget));

            var node = new NavNode(pseudoGridPosition.Value, realWorldPosition, pseudoTarget);
            open.Add(node);
            var routeFound = false;

            while (open.Count > 0)
            {
                node = open.OrderBy(x => x.FinalScore).First();
                closed.Add(node);
                open.Remove(node);

                if (closed.Any(x => x.PseudoPosition == pseudoTarget))
                {
                    routeFound = true;
                    break;
                }

                List<Vector2Int> adjacentNodePoints = GetAdjacentNodePoints(node.PseudoPosition);

                foreach (var point in adjacentNodePoints)
                {
                    if (closed.Any(x => x.PseudoPosition == point)) continue;
                    if (_pseudoGrid[point.x, point.y].HasValue && open.Any(x => x.PseudoPosition != point))
                    {
                        open.Add(new NavNode(point, _pseudoGrid[point.x, point.y].Value, pseudoTarget, node));
                    }
                    else if (_pseudoGrid[point.x, point.y].HasValue)
                    {
                        var testNode = new NavNode(point, _pseudoGrid[point.x, point.y].Value, pseudoTarget, node);
                        if (testNode.FinalScore < open.First(x => x.PseudoPosition == point).FinalScore)
                        {
                            open.First(x => x.PseudoPosition == point).Parent = node;
                        }
                    }

                    yield return null;
                }
            }

            if (!routeFound)
                throw new Exception(
                    $"Somehow, {requestingController.gameObject.name} made a malformed request that escaped the algorithm.");

            var realWorldPositions = new List<Vector3>();

            var destinationNode = closed.First(x => x.PseudoPosition == pseudoTarget);

            ResolveParentChain(destinationNode, realWorldPositions);
            realWorldPositions.Reverse();
            requestingController.Route = realWorldPositions;
        }

        private void ResolveParentChain(NavNode destinationNode, List<Vector3> realWorldPositions)
        {
            realWorldPositions.Add(destinationNode.RealWorldPosition);

            if (destinationNode.Parent != null)
            {
                ResolveParentChain(destinationNode.Parent, realWorldPositions);
            }
        }

        private List<Vector2Int> GetAdjacentNodePoints(Vector2Int point)
        {
            var returnList = new List<Vector2Int>();
            for (int xIndex = point.x - 1; xIndex < point.x + 1; xIndex++)
            {
                for (int yIndex = point.y - 1; yIndex < point.y + 1; yIndex++)
                {
                    var adjacentVector = new Vector2Int(xIndex, yIndex);
                    if (adjacentVector == point) continue;
                    
                    if (adjacentVector.x > -1 && adjacentVector.y > -1)
                    {
                        returnList.Add(adjacentVector);
                    }
                }
            }

            return returnList;
        }

        public Vector2Int? GetPseudoPosition(Vector3 realWorldPosition)
        {
            for (int x = 0; x < _pseudoGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _pseudoGrid.GetLength(1); y++)
                {
                    if (_pseudoGrid[x, y].HasValue && Vector3.Distance(_pseudoGrid[x, y].Value, realWorldPosition) < 1)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return null;
        }
    }
}