using System.Collections;
using Boo.Lang;
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
        
        public IEnumerator ResolvePath(Vector3Int pseudoPosition, Vector3Int pseudoTarget, BaseAIController requestingController)
        {
            var open = new List<NavNode>();
            var closed = new List<NavNode>();
            
            
        }
    }
}