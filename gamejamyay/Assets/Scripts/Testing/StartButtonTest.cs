using UnityEngine;

public class StartButtonTest : MonoBehaviour
{
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Debug.Log("Mouse clicked");
            GameManager.Instance.StartGame();
        }
    }
}
