using UnityEngine;

public class PotionMixer : MonoBehaviour
{
    public static PotionMixer Instance;

    [Header("Target Color Display")]
    public SpriteRenderer finalPotionUnderlay;

    [Header("Win Card")]
    public GameObject mixTitleCard;
    public float cardDelay = 1f;

  
    public enum MixColor { Orange, Green, Purple }
    private MixColor targetColor;


    private string slot1 = null;
    private string slot2 = null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (mixTitleCard != null)
            mixTitleCard.SetActive(false);

        PickRandomTarget();
    }

    void PickRandomTarget()
    {
        targetColor = (MixColor)Random.Range(0, 3);
        Debug.Log("Target color: " + targetColor);

        
    }

    public void PotionDropped(string colorName)
    {
        if (slot1 == null)
        {
            slot1 = colorName;
            Debug.Log("Slot 1 filled: " + colorName);
        }
        else if (slot2 == null && colorName != slot1)
        {
            slot2 = colorName;
            Debug.Log("Slot 2 filled: " + colorName);
            CheckMix();
        }
        else
        {
            Debug.Log("Already have that color or both slots full.");
        }
    }

    void CheckMix()
    {
        MixColor? result = GetMixResult(slot1, slot2);

        if (result == null)
        {
            Debug.Log("Invalid combo — resetting.");
            ResetBowl();
            return;
        }

       
        if (finalPotionUnderlay != null)
            finalPotionUnderlay.color = GetColor(result.Value);

        if (result.Value == targetColor)
        {
            Debug.Log("Correct! Mixed: " + result.Value);
            StartCoroutine(ShowWinCard());
        }
        else
        {
            Debug.Log("Wrong mix: " + result.Value + " — needed " + targetColor);
            Invoke(nameof(ResetBowl), 1f);
        }
    }

    void ResetBowl()
    {
        slot1 = null;
        slot2 = null;

        if (finalPotionUnderlay != null)
            finalPotionUnderlay.color = Color.clear;

        Debug.Log("Bowl reset.");
    }

    MixColor? GetMixResult(string a, string b)
    {
        if (Has(a, b, "Red", "Yellow")) return MixColor.Orange;
        if (Has(a, b, "Blue", "Yellow")) return MixColor.Green;
        if (Has(a, b, "Red", "Blue")) return MixColor.Purple;
        return null;
    }

    bool Has(string a, string b, string x, string y)
    {
        return (a == x && b == y) || (a == y && b == x);
    }

    Color GetColor(MixColor mix)
    {
        switch (mix)
        {
            case MixColor.Orange: return new Color(1f, 0.5f, 0f);
            case MixColor.Green: return new Color(0f, 0.8f, 0f);
            case MixColor.Purple: return new Color(0.6f, 0f, 0.8f);
            default: return Color.white;
        }
    }

    System.Collections.IEnumerator ShowWinCard()
    {
        yield return new WaitForSeconds(cardDelay);
        if (mixTitleCard != null)
            mixTitleCard.SetActive(true);
    }
}