using UnityEngine;
using UnityEngine.SceneManagement;

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
        Invoke(nameof(ReturnToMainMenu), 10f);
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

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        AudioManager.Instance?.StopSFX("GameplayMusic");
        AudioManager.Instance?.PlaySFX("MenuMusic");
    }

  
}
