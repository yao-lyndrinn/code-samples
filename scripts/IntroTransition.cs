using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroTransition : MonoBehaviour
{
    void Awake(){
        Invoke("Transition", 30.0f);
    }

    void Transition(){
        SceneManager.LoadScene("SampleScene");
    }
}
