using UnityEngine;

namespace AI
{
    public class NavNode
    {
        public NavNode Parent { get; set; }
        public Vector2Int PseudoPosition { get; }
        public Vector3 RealWorldPosition { get; }

        public NavNode(Vector2Int pseudoPosition, Vector3 realWorldPosition, NavNode parent)
        {
            PseudoPosition = pseudoPosition;
            RealWorldPosition = realWorldPosition;
            Parent = parent;
        }
    }
}