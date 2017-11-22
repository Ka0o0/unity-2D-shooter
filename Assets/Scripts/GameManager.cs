using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerPrefab;


    private void Start()
    {
        Instantiate(PlayerPrefab);
    }
}
