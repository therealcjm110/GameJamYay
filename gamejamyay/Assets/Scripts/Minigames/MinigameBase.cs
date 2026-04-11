using UnityEngine;
using System;

public abstract class MinigameBase : MonoBehaviour
{

    void Start(){
        GameManager.Instance.RegisterMinigame(this);
    }

    public string Title;
    public float BaseTimeLimit = 5f;

    public event Action<bool> OnMGComplete;

    public abstract void StartMinigame(float speedMult);
    public abstract void ResetMinigame()

    public void ForceFailure() => Complete(false);  // may not be needed for one scene

    protected void Complete(bool success){
        OnMGComplete?.Invoke(success);  // find diff way to do this
    }

}
