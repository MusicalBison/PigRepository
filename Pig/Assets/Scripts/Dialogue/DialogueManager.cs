using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;

    public Animator startDialogueAnim;
    public Animator waitAnim;
    public Animator circleAnim;
    public Animator boltAnim;
    public Animator boxAnim;
    public Animator endAnim;

    private Queue<string> sentences;

    Dialogue dialogue;


    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(GameObject gameObject)
    {
        dialogue = gameObject.GetComponent<DialogueTrigger>().dialogue;

        boxAnim.SetBool("BoxOpen", true);
        startDialogueAnim.SetBool("StartOpen", false);
        waitAnim.SetBool("StartOpen", false);
        circleAnim.SetBool("StartOpen", false);
        boltAnim.SetBool("StartOpen", false);
        endAnim.SetBool("StartOpen", false);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            dialogue.isRead = true; // ставим пометку, что диалог прочитан
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue(); // удаляем из очереди строку
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        boxAnim.SetBool("BoxOpen", false);
    }
}
