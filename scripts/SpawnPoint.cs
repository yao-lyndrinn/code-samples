using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject SwordsmanPrefab;
    void Start()
    {
        GameObject Swordsman = Instantiate(SwordsmanPrefab, transform.position, Quaternion.identity);
    }
}
