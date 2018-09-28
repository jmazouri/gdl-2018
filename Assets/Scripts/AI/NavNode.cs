using UnityEngine;

namespace AI
{
    public class NavNode
    {
        public NavNode Parent { get; set; }
        public Vector2Int PseudoPosition { get; }
        public Vector3 RealWorldPosition { get; }

        public float G => Parent == null ? 0 : Parent.G + 1;

        public float H { get; }

        public float FinalScore => G + H;

        public NavNode(Vector2Int pseudoPosition, Vector3 realWorldPosition, Vector2Int pseudoDestination, NavNode parent = null)
        {
            PseudoPosition = pseudoPosition;
            RealWorldPosition = realWorldPosition;
            H = Vector2Int.Distance(pseudoPosition, pseudoDestination);
            Parent = parent;
        }
    }
}