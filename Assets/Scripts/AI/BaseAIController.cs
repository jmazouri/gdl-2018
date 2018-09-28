using System.Collections.Generic;
using Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI
{
    public class BaseAIController : BaseCharacter
    {
        public void AssignDestination(Tile target)
        {
        }

        public List<Vector3> Route { get; set; }
    }
}