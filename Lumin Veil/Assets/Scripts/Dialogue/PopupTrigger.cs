using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public Dialogue popupDialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        FindAnyObjectByType<DialogueManager>().StartPopup(popupDialogue);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindAnyObjectByType<DialogueManager>().ResetPopup();
        }
    }
}
