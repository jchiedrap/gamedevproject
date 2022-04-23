using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    abstract public void PerformInteraction();
    abstract public bool ConditionsMet();
}