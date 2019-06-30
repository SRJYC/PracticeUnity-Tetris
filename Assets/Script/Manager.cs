using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Playfield playfield;
    public GroupController groupController;
    public Spawner spawner;

    public static Manager Instance { get; private set; }

    public GameObject gameOverInfo;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            DestroyImmediate(this);
    }

    private void Start()
    {
        gameOverInfo.SetActive(false);
    }

    public void GameStart()
    {
        playfield.Reset();
        groupController.GameStart();
    }

    public void GameOver()
    {
        gameOverInfo.SetActive(true);
    }
}
