using System.Collections;
using UnityEngine;
using Nova;

public class DialogueManager : MonoBehaviour
{
    public TextBlock nameText;
    public TextBlock dialogueText;
    public Animator animator;

    private Queue sentences;

    void Start()
    {
        sentences = new Queue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        nameText.Text = dialogue.name;
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
            EndDialogue();
            return;
        }
        string sentence = (string)sentences.Dequeue();
        dialogueText.Text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentenceWithDelay(sentence));
    }

    private IEnumerator TypeSentenceWithDelay(string sentence)
    {
        dialogueText.Text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.Text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    public void ResetDialogue()
    {
        StopAllCoroutines();
        sentences.Clear();
        dialogueText.Text = "";
        animator.SetBool("isOpen", false);
    }
}
