using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMover : InteractionTarget
{
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private float speed;

    private bool isMoving;
    private float movementAmount;
    private Vector3 startPosition;

	// Use this for initialization
	void Start ()
    {
        startPosition = transform.position;
	}

    public override void Interact(Interactor interactor)
    {
        isMoving = true;
    }

    public override void InternalUpdate()
    {
        transform.position = Vector3.Lerp(startPosition, startPosition + targetPosition, movementAmount);

        if (isMoving)
        {
            movementAmount += Time.deltaTime * speed;
        }

        if (isMoving && movementAmount >= 1)
        {
            Destroy(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + targetPosition, transform.lossyScale);
    }
}
