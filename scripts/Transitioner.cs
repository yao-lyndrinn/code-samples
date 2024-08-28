using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    [SerializeField]
    private LevelConnection _connection;
    [SerializeField]
    private string _targetSceneName;
    [SerializeField]
    private Transform _spawnPoint;
    private void Start()
    {
        if (_connection == LevelConnection.ActiveConnection)
        {
            foreach (SwordsmanController amni in FindObjectsOfType<SwordsmanController>()){
                amni.transform.position = _spawnPoint.position;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D Swordsman){
        var player = Swordsman.gameObject.GetComponent<SwordsmanController>();
    if (player != null)
        {
            LevelConnection.ActiveConnection = _connection;
            SceneManager.LoadScene(_targetSceneName);
        }   
    }
    
}
