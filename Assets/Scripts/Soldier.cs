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