using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class TestMoveAssignment : MonoBehaviour
{
    [SerializeField] private BaseAIController _aiController;
    private bool _run;

    // Use this for initialization
    void Update()
    {
        if (_run) return;
        _aiController.AssignDestination(transform.position);
        _run = true;
    }
}