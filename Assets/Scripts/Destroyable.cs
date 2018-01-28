using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float HealthPoints = 100;

    private bool IsDead
    {
        get { return HealthPoints <= 0; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Bullet"))
        {
            Destroy(otherGameObject);
            HitByBullet(otherGameObject.GetComponent<Bullet>());
        }
    }

    private void HitByBullet(Bullet bullet)
    {
        HealthPoints -= bullet.Damage;
        DestroyIfDead();
    }

    private void DestroyIfDead()
    {
        if (IsDead)
        {
            Destroy(gameObject);
        }
    }
}