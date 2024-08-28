using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalTransitioner : MonoBehaviour
{
    SwordsmanController player;
    void OnTriggerEnter2D(Collider2D Swordsman)
    {
        player = Swordsman.gameObject.GetComponent<SwordsmanController>();
    if (player != null)
        {
            Destroy(Swordsman.gameObject);
            SceneManager.LoadScene("EndingScreen");
        }
    }
}
