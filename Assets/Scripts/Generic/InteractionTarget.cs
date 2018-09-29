using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionTarget : MonoBehaviour
{
    [SerializeField]
    private string ActionName;

    //Implement this to override the action display name
    public virtual string ActionDisplay
    {
        get
        {
            return string.IsNullOrWhiteSpace(ActionName) ? "Use" : ActionName;
        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Implement this in components that extend InteractionTarget
    public virtual void Interact(Interactor interactor)
    {
        Debug.Log("Someone interacted with me! :DDD", gameObject);
    }
}
