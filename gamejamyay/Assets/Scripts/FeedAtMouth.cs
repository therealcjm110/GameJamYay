using UnityEngine;
using System.Collections;

public class FeedAtMouth : MonoBehaviour
{
    [Header("Creature Sprites")]
    public GameObject sadCreature;
    public GameObject happyCreature;

    [Header("Cheer Card")]
    public GameObject cheerCard;
    public float cheerCardDelay = 1f; // seconds after eating before card appears

    void Start()
    {
        if (happyCreature != null)
            happyCreature.SetActive(false);

        if (cheerCard != null)
            cheerCard.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lollipop"))
        {
            CheerUp(other.gameObject);
        }
    }

    void CheerUp(GameObject lollipop)
    {
        if (sadCreature != null) sadCreature.SetActive(false);
        if (happyCreature != null) happyCreature.SetActive(true);

        lollipop.SetActive(false);

        StartCoroutine(ShowCheerCard());

        Debug.Log("Experiment 3.012 is cheered up!");
    }

    IEnumerator ShowCheerCard()
    {
        yield return new WaitForSeconds(cheerCardDelay);

        Debug.Log("Trying to show cheer card. cheerCard is null: " + (cheerCard == null));

        if (cheerCard != null)
            cheerCard.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}