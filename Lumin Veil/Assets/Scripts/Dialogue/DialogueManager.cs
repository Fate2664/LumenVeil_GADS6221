using System.Collections;
using UnityEngine;
using Nova;

public class DialogueManager : MonoBehaviour
{
    public TextBlock nameText;
    public TextBlock dialogueText;
    public TextBlock popupText;
    public TextBlock popupNameText;
    public TextBlock endText;
    public TextBlock endName;
    public Animator animator;
    public Animator popupAnimator;
    public Animator endAnimator;

    private Queue sentences;

    void Start()
    {
        sentences = new Queue();
    }

    public void StartPopup(Dialogue popup)
    {
        popupAnimator.SetBool("isOpenPopup", true);
        popupNameText.Text = popup.name;
        sentences.Clear();
        foreach (string sentence in popup.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextPopup();
    }

    public void StartEnd(Dialogue endDialogue)
    {
        endAnimator.SetBool("isOpenEnd", true);
        endName.Text = endDialogue.name;
        sentences.Clear();
        foreach (string sentence in endDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextEndDialogue();
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

    public void DisplayNextPopup()
    {
        if (sentences.Count == 0)
        {
            EndPopup();
            return;
        }
        string sentence = (string)sentences.Dequeue();
        popupText.Text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypePopupWithDelay(sentence));
    }

    public void DisplayNextEndDialogue()
    {
        if (sentences.Count == 0)
        {
            EndEndDialogue();
            return;
        }
        string sentence = (string)sentences.Dequeue();
        endText.Text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeEndWithDelay(sentence));
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

    private IEnumerator TypeEndWithDelay(string sentence)
    {
        endText.Text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            endText.Text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        DisplayNextEndDialogue();
    }

    private IEnumerator TypePopupWithDelay(string sentence)
    {
        popupText.Text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            popupText.Text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        DisplayNextPopup();
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    public void EndEndDialogue()
    {
        animator.SetBool("isEnd", false);
    }

    public void EndPopup()
    {
        popupAnimator.SetBool("isOpenPopup", false);
    }

    public void ResetPopup()
    {
        StopAllCoroutines();
        sentences.Clear();
        popupText.Text = "";
        popupAnimator.SetBool("isOpenPopup", false);
    }

    public void ResetDialogue()
    {
        StopAllCoroutines();
        sentences.Clear();
        dialogueText.Text = "";
        animator.SetBool("isOpen", false);
    }

    public void ResetEndDialogue()
    {
        if (sentences != null && animator != null)
        {
            StopAllCoroutines();
            sentences.Clear();
            endText.Text = "";
            animator.SetBool("isEnd", false); 
        }
    }
}
