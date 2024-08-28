using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : MonoBehaviour
{
    public TileBase water;
    public TileBase ice;
    bool frozen = false;

    public void Freeze(){
        if(frozen == false){
            Tilemap tilemap = GetComponent<Tilemap>();
            tilemap.SwapTile(water, ice);
            frozen = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (frozen == false){
    
        SwordsmanController player = other.GetComponent<SwordsmanController>();
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        if (player != null)
        {
            playerBody.AddForce(Vector2.left * 15.0f, ForceMode2D.Impulse);
        }
        }
    }
}