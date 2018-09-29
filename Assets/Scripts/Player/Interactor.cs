using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private LayerMask interactables;

    public Action<InteractionTarget> InteractionTargetChanged;

    private InteractionTarget lastTarget;
    private InteractionTarget currentTarget;

	// Use this for initialization
	void Start ()
    {
		
	}

    private void FixedUpdate()
    {
        var hit = Physics2D.CircleCast(transform.position, interactionRange,
                Vector2.zero, Mathf.Infinity, interactables);
        
        currentTarget = hit.collider?.GetComponent<InteractionTarget>();

        if (currentTarget != lastTarget)
        {
            if (currentTarget != null)
            {
                Debug.Log($"[{currentTarget.ActionDisplay}] {currentTarget.gameObject.name}");

                currentTarget.ActionDisplayChanged += ActionDisplayChanged;
                currentTarget.EnterRange(this);
            }
            else
            {
                Debug.Log($"Moved out of interaction range.");

                if (lastTarget != null)
                {
                    lastTarget.ExitRange(this);
                    lastTarget.ActionDisplayChanged -= ActionDisplayChanged;
                }
            }

            InteractionTargetChanged?.Invoke(currentTarget);
        }

        lastTarget = currentTarget;
    }

    void ActionDisplayChanged(string message)
    {
        InteractionTargetChanged?.Invoke(currentTarget);
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.E))
        {
            currentTarget?.Interact(this);
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.33f);
        Gizmos.DrawSphere(transform.position, interactionRange);
    }
}
