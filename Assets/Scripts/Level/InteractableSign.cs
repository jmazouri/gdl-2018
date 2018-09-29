using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSign : InteractionTarget
{
    private bool isShown = false;

    [SerializeField]
    private GameObject textPopup;

    [SerializeField]
    [Multiline]
    private string message;

    public override void Interact(Interactor interactor)
    {
        isShown = !isShown;
    }

    public override void InternalStart()
    {
        textPopup.transform.localScale = Vector3.zero;
        textPopup.GetComponentInChildren<TextMeshPro>().SetText(message);
    }

    public override void InternalUpdate()
    {
        Vector3 targetScale = isShown ? new Vector3(1, 1, 1) : Vector3.zero;

        textPopup.transform.localScale =
            Vector3.Lerp(textPopup.transform.localScale, targetScale, Time.deltaTime * 8f);
    }
}
