using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    enum Direction {
        FWD, BCK, LFT, RGT
    }
    
    public bool smoothTransition = true;
    public bool playerSettingSmoothTransition = true;

    public bool eventHappening = false;
    public bool isMoving = false;

    [Range(7f,15f)] public float transitionSpeed;
    [Range(450f,550f)] public float rotationSpeed;

    public Vector3 targetGridPos;
    public Vector3 prevTargetGridPos;
    public Vector3 targetRotation;

    public Vector3 defaultPos;

    public void RotateLeft()
    {
        if (AtRest && CanDoInputs) targetRotation -= Vector3.up * 90f;
    }

    public void RotateRight()
    {
        if (AtRest && CanDoInputs) targetRotation += Vector3.up * 90f;
    }

    public void MoveLeft()
    {
        if (AtRest && CanMove(Direction.LFT))
        {
            targetGridPos -= transform.right;
            isMoving = true;
        }
    }

    public void MoveRight()
    {
        if (AtRest && CanMove(Direction.RGT))
        {
            targetGridPos += transform.right;
            isMoving = true;
        }
    }

    public void MoveForward()
    {
        if (AtRest && CanMove(Direction.FWD))
        {
            targetGridPos += transform.forward;
            isMoving = true;
        }
    }

    public void MoveBack()
    {
        if (AtRest && CanMove(Direction.BCK))
        {
            targetGridPos -= transform.forward;
            isMoving = true;
        }
    }

    private void Start()
    {
        targetGridPos = defaultPos;
    }

    private void FixedUpdate() {
        MovePlayer();
        if (!isMoving) return;
        
        LevelManager.Instance.encounterHandler.CalculateEncounter();
        isMoving = false; 
    }

    void MovePlayer() {
        Vector3 targetPos = targetGridPos;

        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0;
        if (targetRotation.y < 0f) targetRotation.y = 270f;

        if (!smoothTransition) {
            transform.position = targetPos;
            transform.rotation = Quaternion.Euler(targetRotation);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotationSpeed);
        }
    } 

    public void Interact() {
        CanBeInteractedWith();
    }

    bool AtRest {
        get {
            return (Vector3.Distance(transform.position, targetGridPos) < 0.05f && Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f);
        }
    }
    
    bool CanDoInputs
    {
        get {
            return !eventHappening && AtRest;
        }
    }

    bool CanMove(Direction dir) {
        if (!CanDoInputs) return false;
        
        bool res = true;
        RaycastHit hit = new RaycastHit();
        LayerMask mask = LayerMask.GetMask("Collision");
        switch (dir) {
            case Direction.FWD:
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1.1f, mask)) {res = false; Debug.Log("Hit fwd:" + hit.transform.name);}
                break;
            case Direction.BCK:
                if (Physics.Raycast(transform.position, -1*transform.forward, out hit, 1.1f, mask)) {res = false; Debug.Log("Hit bck:" + hit.transform.name);}
                break;
            case Direction.LFT:
                if (Physics.Raycast(transform.position, -1*transform.right, out hit, 1.1f, mask)) {res = false; Debug.Log("Hit lft:" + hit.transform.name);}
                break;
            case Direction.RGT:
                if (Physics.Raycast(transform.position, transform.right, out hit, 1.1f, mask)) {res = false; Debug.Log("Hit rgt:" + hit.transform.name);}
                break;
        }
        if (!hit.Equals(new RaycastHit())) Debug.DrawLine(transform.position, hit.point, Color.red, 1f);
        return res;
    }

    bool CanBeInteractedWith() {
        if (!CanDoInputs) return false;
        
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.1f)) {
            Interactable[] inter = hit.collider.gameObject.GetComponents<Interactable>();
            if (inter != null) 
            {
                foreach (Interactable i in inter) {
                    EventHandler.Instance.OnStartEvent();
                    i.PerformInteraction();
                }
                return inter.Length > 0;
            }
        }
        return false;
    }
}
