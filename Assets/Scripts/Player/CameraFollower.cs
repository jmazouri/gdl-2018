using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private float smoothness;

    private Transform followTarget;

    public void Follow(Transform target)
    {
        followTarget = target;
    }

	// Use this for initialization
	void Start ()
    {
        followTarget = FindObjectOfType<PlayerMovement>().transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var targetPosition = new Vector3(followTarget.position.x, followTarget.position.y, -10);

        transform.position = 
            Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
	}
}
