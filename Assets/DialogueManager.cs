using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class DialogueManager : SerializedMonoBehaviour
{
    public DialogueNode startNode;
    public DialogueNode currentNode;
    public UnityEvent StartDialogue;
    public UnityEvent SelectDialogue;

    public HashSet<DialogueNode> tracker = new HashSet<DialogueNode>();

    public GameObject UI;

    public Text displayText;
    public GameObject buttonsContainter;
    public List<Button> buttons = new List<Button>();

    public void Start()
    {
        buttons = buttonsContainter.GetComponentsInChildren<Button>().ToList();
        UI.SetActive(false);
    }

    public void StartDialogueFirst()
    {
        if (currentNode == null)
        {
            UI.SetActive(true);
            OnStartDialogue(startNode);
        }
    }
    
    public void OnStartDialogue(DialogueNode node)
    {
        if (node.text == "FINAL NODE")
        {
            EndDialogue();
            return;
        }

            displayText.text = node.text;
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < node.options.Count)
            {
                buttons[i].GetComponentInChildren<Text>().text = node.options[i].Item2;
                buttons[i].gameObject.SetActive(true);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        currentNode = node;
        tracker.Add(node);
    }

    private void EndDialogue()
    {
        UI.SetActive(false);
        currentNode = null;
        EventHandler.Instance.OnEndEvent();
    }

    public void OnSelectDialogue(int optionIndex)
    {
        OnStartDialogue(currentNode.options[optionIndex].Item1);
    }
}
