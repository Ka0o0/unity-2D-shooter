using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class BattleFieldManager : MonoBehaviour
{   
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var camera = Camera.main;
            var magic = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,
                camera.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(magic, Vector2.zero);
            if (hit)
            {
                var selectedGameObject = hit.collider.gameObject;
                Destroy(selectedGameObject);
            }
        }
    }
}