using NUnit.Framework.Constraints;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public double Damage;
    public int Speed;
    public float MaxTravelDistance;

    private Vector3 _startPosition;

    public void StartMove()
    {
        _startPosition = transform.position;
        transform.position = _startPosition + transform.up;

        var bulletRigidbody2D = GetComponent<Rigidbody2D>();
        bulletRigidbody2D.AddForce(transform.up * 500);
    }

    private void FixedUpdate()
    {
        var travelledDistance = Vector3.Distance(transform.position, _startPosition);
        if (travelledDistance > MaxTravelDistance)
        {
            Destroy(gameObject);
        }
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