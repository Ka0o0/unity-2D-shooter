using System;
using UnityEngine;

public class Soldier : Destroyable
{
    public Team Team;
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
}
