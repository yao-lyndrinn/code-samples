using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenTransition : MonoBehaviour
{
    // placed on screen after beast is defeated, invokes transition after
    //enough time for dialogue completes
    void Awake(){
        Invoke("Transition", 20.0f);
    }

    void Transition(){
        SceneManager.LoadScene("EndingScreen");
    }
}
