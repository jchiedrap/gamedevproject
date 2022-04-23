using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    public TMP_Text direction;
    PlayerInputController playerInputController;

    private void Start() {
        playerInputController = FindObjectOfType<PlayerInputController>();
    }

    private void Update() {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        float rot = playerInputController.transform.rotation.eulerAngles.y;
        float dir = rot / 90.0f;
        if (dir < 0.1f && dir > -0.1f) direction.SetText("N");
        else if (dir < 1.1f && dir > 0.9f) direction.SetText("E");
        else if (dir < 2.1f && dir > 1.1f) direction.SetText("S");
        else if (dir < 3.1 && dir > 2.1f) direction.SetText("W");
        Debug.Log(dir);
    }
}
