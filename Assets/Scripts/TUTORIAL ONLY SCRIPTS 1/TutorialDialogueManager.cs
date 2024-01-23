using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public TextTutorial dialogue;
    public Text dialogueText;

    public Image imageEnemy;
    public Image imageAlly;
    public Canvas canvasTutorial, canvasUI;


    public Image exampleCompass;
    public Image exampleSelect;

    public Image exampleMove;

    public Image exampleAttack;

    public int counter = 0;

    void Start()
    {
        sentences = new Queue<string>();
        dialogue = GetComponent<TextTutorial>();
        StartTutorial(dialogue.dialogue);
    }


    public void StartTutorial(Dialogue dialogue)
    {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }


    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        counter++;

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;

        if (counter == 2)
        {
            imageEnemy.gameObject.SetActive(false);
            exampleCompass.gameObject.SetActive(true);
            exampleSelect.gameObject.SetActive(true);
        }
        if (counter == 3)
        {
            exampleCompass.gameObject.SetActive(false);
            exampleSelect.gameObject.SetActive(false);
            exampleMove.gameObject.SetActive(true);
        }
        if (counter == 4)
        {
            exampleMove.gameObject.SetActive(false);
            exampleAttack.gameObject.SetActive(true);
        }


    }

    void EndDialogue()
    {
        canvasTutorial.gameObject.SetActive(false);
        imageAlly.gameObject.SetActive(false);
    }

}
