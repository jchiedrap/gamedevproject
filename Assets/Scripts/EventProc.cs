using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventProc : Interactable
{
    
    public enum ProcType
    {
        None,
        Contact,
        Interact
    }
    
    public ProcType procType = ProcType.None;
    
    public UnityEvent onCall;

    public void StartEvent()
    {
        onCall.Invoke();
        EventHandler.Instance.OnStartEvent();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (procType == ProcType.Contact && other.gameObject.CompareTag("Player")) StartEvent();
    }

    public override void PerformInteraction()
    {
        if (ConditionsMet()) StartEvent();
    }

    public override bool ConditionsMet()
    {
        return procType == ProcType.Interact;
    }
}
