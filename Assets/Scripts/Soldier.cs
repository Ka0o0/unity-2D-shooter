using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Team Team;
    public double HealthPoints = 100;
    public GameObject BulletPrefab;
    public String Name;

    private float CenterOffset = (float) 0.5;

    public bool CanMoveToPosition(Vector2 position)
    {
        // TODO: Implement me :)
        return true;
    }

    public void MoveToPosition(Vector2 position)
    {
        transform.position = new Vector3((float) Math.Floor(position.x) + CenterOffset, (float) Math.Floor(position.y) + CenterOffset, 0);
    }
    
    // MARK: Private

    private Boolean IsDead
    {
        get { return HealthPoints <= 0; }
    }

    private void Start()
    {
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.Rotate(0, 0, 135);
        bullet.GetComponent<Bullet>().StartMove();
    }

    private void FixedUpdate()
    {
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