using UnityEngine;

public class WaterForce : MonoBehaviour{
void OnTriggerStay2D(Collider2D other)
    {
        SwordsmanController player = other.GetComponent<SwordsmanController>();
        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
        if (player != null)
        {
            playerBody.AddForce(Vector2.left * 8.0f, ForceMode2D.Impulse);
        }
    }
}