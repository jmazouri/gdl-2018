using System.Collections.Generic;
using System.Linq;
using Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AI
{
    public class BaseAIController : BaseCharacter
    {
        [SerializeField] private TileNavController _navController;
        private Coroutine _navRoutine;
        public List<Vector3> Route { get; set; }
        
        void Update()
        {
            if (Route == null || Route.Count == 0) return;

            if (Vector3.Distance(Route.First(), transform.position) <= 0.1f)
            {
                Route.Remove(Route.First());
                if (Route == null || Route.Count == 0)
                {
                    return; //todo: jank
                }
            }

            Vector3 diff = Route.First() - transform.position;
            diff.Normalize();
 
            float rotationValue = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationValue - 90);
            transform.Translate(Vector3.up * 5 * Time.deltaTime);
        }
        
        public void AssignDestination(Vector3 destination)
        {
            if (_navRoutine != null)
            {
                StopCoroutine(_navRoutine);
            }
            Route = null;
            _navRoutine = StartCoroutine(_navController.ResolvePath(transform.position, destination, this));
        }


    }
}