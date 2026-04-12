using UnityEngine;
using System.Collections;

public class FeedAtMouth : MonoBehaviour
{
    [Header("Creature Sprites")]
    public GameObject sadCreature;
    public GameObject happyCreature;

    [Header("Cheer Card")]
    public GameObject cheerCard;
    public float cheerCardDelay = 1f;
    public float transitionDelay = 2f; // seconds after cheer card before moving on

    void Start()
    {
        if (happyCreature != null) happyCreature.SetActive(false);
        if (cheerCard != null) cheerCard.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lollipop"))
            CheerUp(other.gameObject);
    }

    void CheerUp(GameObject lollipop)
    {
        if (sadCreature != null) sadCreature.SetActive(false);
        if (happyCreature != null) happyCreature.SetActive(true);

        lollipop.SetActive(false);

        StartCoroutine(ShowCheerCardThenTransition());

        Debug.Log("Experiment 3.012 is cheered up!");
    }

    IEnumerator ShowCheerCardThenTransition()
    {
        yield return new WaitForSeconds(cheerCardDelay);

        if (cheerCard != null)
            cheerCard.SetActive(true);

        yield return new WaitForSeconds(transitionDelay);

        if (SceneTransition.Instance != null)
            SceneTransition.Instance.GoToNextScene();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}