using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicChange : MonoBehaviour
{
    [SerializeField]
    private LevelConnection _connection;
    [SerializeField]
    private string _targetSceneName;
    [SerializeField]
    private Transform _spawnPoint;
    AudioSource music;
    public AudioClip newMusic;
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
            music = player.gameObject.GetComponent<AudioSource>();
            music.clip = newMusic;
            music.Play();
            SceneManager.LoadScene(_targetSceneName);
        }   
    }
    
}