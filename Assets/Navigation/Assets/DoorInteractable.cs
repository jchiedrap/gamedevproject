using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    public bool isLocked = false;
    public bool isTransitioning = false;
    PlayerInputController p;

    private void Start() {
        p = FindObjectOfType<PlayerInputController>();
    }

    override public void PerformInteraction() {
        if (ConditionsMet()) {
            StartCoroutine("callFade");
        }
    }

    public void ToggleLock() {
        isLocked = !isLocked;
    }

    public override bool ConditionsMet()
    {
        return !isLocked;
    }

    IEnumerator callFade() 
    {
        p.GetComponentInChildren<Curtain>().Fade(true);
        yield return new WaitForSeconds(1);
        p.smoothTransition = false;
        p.targetGridPos += p.transform.forward*2;
        yield return new WaitForEndOfFrame();
        p.smoothTransition = p.playerSettingSmoothTransition;
        p.GetComponentInChildren<Curtain>().Fade(false);
        yield return new WaitForSeconds(1);
        EventHandler.Instance.OnEndEvent();
    }
}
