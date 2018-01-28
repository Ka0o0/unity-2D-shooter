using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Soldiers;

    private List<string> AvailableScenes = new List<string>(new[] {"Level00"});
    private Random _random = new Random();

    public void RestartGame()
    {
        var randomIndex = Random.Range(0, AvailableScenes.Count);
        var selectedScene = AvailableScenes[randomIndex];
        SceneManager.LoadScene(selectedScene);
    }
}