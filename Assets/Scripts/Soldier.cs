using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public Team Team;
    public float HealthPoints = 100;
    public int MaxShootingsCount = 1;
    public GameObject BulletPrefab;
    public String Name;
    public bool IsSelected { get; set; }
    public bool IsTeamActive { get; set; }

    private float CenterOffset = (float) 0.5;
    private HealthBarHelper _healthBarHelper;
    private Light _soldierLight;

    public bool CanMoveToPosition(Vector2 position)
    {
        // TODO: Implement me :)
        return true;
    }

    public void MoveToPosition(Vector2 position)
    {
        transform.position = new Vector3((float) Math.Floor(position.x) + CenterOffset,
            (float) Math.Floor(position.y) + CenterOffset, 0);
    }

    public void ShootToPoint(Vector2 point)
    {
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = transform.position;
        //Math.tan
        var rotationRad = Mathf.Atan2(bullet.transform.position.y - point.y, bullet.transform.position.x - point.x);
        var deg = Mathf.Rad2Deg * rotationRad + 90;
        bullet.transform.Rotate(0, 0, deg);
        bullet.GetComponent<Bullet>().StartMove();
    }

    // MARK: Private

    private Boolean IsDead
    {
        get { return HealthPoints <= 0; }
    }

    private void Start()
    {
        _healthBarHelper = GetComponentInChildren<HealthBarHelper>();
        _soldierLight = GetComponentInChildren<Light>();
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSelected", IsSelected);
        GetComponent<Animator>().SetBool("IsTeamActive", IsTeamActive);

        _healthBarHelper.CurrentHpPercentage = HealthPoints;
        _healthBarHelper.gameObject.SetActive(IsTeamActive);
        _soldierLight.gameObject.SetActive(IsTeamActive);
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