using System;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Speed;
    public float MaxTravelDistance;
    public AudioClip ExplosionSound;

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
        var tags = new[] {"Obstacle", "Player", "Wall"};
        if (tags.Any(tagToMatch => otherGameObject.CompareTag(tagToMatch)))
        {
            if (ExplosionSound)
            {
                AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}