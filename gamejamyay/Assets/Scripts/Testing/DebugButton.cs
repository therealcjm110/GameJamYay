using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("Button clicked via code");
            GameManager.Instance.StartGame();   // no idea why this works but nothing else does lowkey. sorry :(
        });
    }
}
