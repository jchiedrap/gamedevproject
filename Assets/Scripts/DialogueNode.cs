using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "Dialogue Node", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class DialogueNode : SerializedScriptableObject
{
    public string text;
    public List<(DialogueNode, string)> options = new List<(DialogueNode, string)>();
}