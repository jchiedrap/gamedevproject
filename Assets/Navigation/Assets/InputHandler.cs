using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class InputHandler : MonoBehaviour
{
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode rotLeft = KeyCode.Q;
    public KeyCode rotRight = KeyCode.E;
    public KeyCode interact = KeyCode.F;

    PlayerInputController inputController;

    private void Awake() {
        inputController = GetComponent<PlayerInputController>();
    }

    private void Update() {
        if (Input.GetKeyDown(forward)) inputController.MoveForward();
        if (Input.GetKeyDown(back)) inputController.MoveBack();
        if (Input.GetKeyDown(left)) inputController.MoveLeft();
        if (Input.GetKeyDown(right)) inputController.MoveRight();
        if (Input.GetKeyDown(rotLeft)) inputController.RotateLeft();
        if (Input.GetKeyDown(rotRight)) inputController.RotateRight();
        if (Input.GetKeyDown(interact)) inputController.Interact();
    }
}
