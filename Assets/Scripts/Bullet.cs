using UnityEngine;

public class Bullet : MonoBehaviour
{
    public double Damage;
    public int Speed;
    
    public void StartMove()
    {
        transform.position = transform.position + transform.up;
        
        var bulletRigidbody2D = GetComponent<Rigidbody2D>();
        bulletRigidbody2D.AddForce(transform.up * 500);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;

        if (otherGameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}