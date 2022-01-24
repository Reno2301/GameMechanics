using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;

    public Animator dialogueBoxAnimator;

    private Queue<string> lines;

    public bool dialogueOn = false;

    private void Start()
    {
        lines = new Queue<string>();
    }

    private void Update()
    {
        if (dialogueOn && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)))
            EndDialogue();

        if (Input.GetKeyDown(KeyCode.Space))
            DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueOn = true;

        lines.Clear();

        dialogueBoxAnimator.SetBool("IsOpen", true);

        foreach (string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = lines.Dequeue();
        dialogueText.text = line;
    }

    public void EndDialogue()
    {
        dialogueOn = false;

        dialogueBoxAnimator.SetBool("IsOpen", false);

        Debug.Log("End of conversation.");
    }
}
