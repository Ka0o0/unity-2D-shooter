using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Soldier : Destroyable
{
    public Team Team;
    public int MaxShootingsCount = 1;
    public GameObject BulletPrefab;
    public String Name;
    public bool IsSelected { get; set; }
    public bool IsTeamActive { get; set; }
    public SoldierMovementManager MovementManager { get; private set; }

    private HealthBarHelper _healthBarHelper;
    private Light _soldierLight;


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
        MovementManager = GetComponent<SoldierMovementManager>();
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSelected", IsSelected);
        GetComponent<Animator>().SetBool("IsTeamActive", IsTeamActive);

        _healthBarHelper.CurrentHpPercentage = HealthPoints;
        _healthBarHelper.gameObject.SetActive(IsTeamActive);
        _soldierLight.gameObject.SetActive(IsTeamActive);
        MovementManager.ShowDistanceCircles = IsSelected;
    }
}