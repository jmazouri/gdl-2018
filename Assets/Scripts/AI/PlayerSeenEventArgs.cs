using System;
using UnityEngine;

namespace AI
{
    public class PlayerSeenEventArgs : EventArgs
    {
        public Vector3 Position { get; }

        public PlayerSeenEventArgs(Vector3 position)
        {
            Position = position;
        }
    }
}
