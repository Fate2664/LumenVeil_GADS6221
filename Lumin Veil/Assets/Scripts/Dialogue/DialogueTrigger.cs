using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue noOrbsDialogue;
    public Dialogue notEnoughOrbsDialogue;
    public Dialogue correctOrbsDialogue;
    [SerializeField] private int requiredOrbCount = 10;
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private GameObject powerupPrefab;
    private bool hasStartedDialogue = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || hasStartedDialogue)
            return;

        hasStartedDialogue = true;

        int orbcount = inventoryPanel.GetOrbCount();

        if (orbcount <= 0)
        {
            FindAnyObjectByType<DialogueManager>().StartDialogue(noOrbsDialogue);
        }
        else
        if (orbcount < requiredOrbCount)
        {
            FindAnyObjectByType<DialogueManager>().StartDialogue(notEnoughOrbsDialogue);
        }
        else
        {
            FindAnyObjectByType<DialogueManager>().StartDialogue(correctOrbsDialogue);
            Invoke(nameof(ShowPowerup), 10f); // Show powerup after a short delay
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasStartedDialogue = false;
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.ResetDialogue();
        }
    }

    private void ShowPowerup()
    {
        powerupPrefab.SetActive(true);
    }
}


