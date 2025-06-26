using UnityEngine;

public class EndDialogueTrigger : MonoBehaviour
{
    public Dialogue endDialogue;
    [SerializeField] private FadeController fadeController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && fadeController == null)
            return;

        FindAnyObjectByType<DialogueManager>().StartEnd(endDialogue);
        fadeController.FadeToBlack();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           DialogueManager manager = FindAnyObjectByType<DialogueManager>();
            if (manager != null)
            {
                manager.ResetEndDialogue();
            }
        }
    }
}
