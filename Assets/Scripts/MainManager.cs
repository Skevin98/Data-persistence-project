using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    // High Score
    public int highScore = 0;
    public string bestPlayerName;

    public string playerName;

    // Input
    public TMP_InputField input;

    // High score on the menu
    public TextMeshProUGUI highScoreText;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadHighScore();
        DontDestroyOnLoad(gameObject);
    }

    public void GameStart()
    {
        playerName = input.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [Serializable]
    public class Data
    {
        public int highScore;
        public string bestPlayerName;
    }

    public void SaveHighScore(string name, int score) {
        Data data = new();
        data.bestPlayerName = name;
        data.highScore = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/highscore.json" , json);
    
    }

    public void LoadHighScore()
    {
        Debug.Log("High Score loaded.");
        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/highscore.json");
            Data data = JsonUtility.FromJson<Data>(json);
            highScore = data.highScore;
            bestPlayerName = data.bestPlayerName;

        }
        else
        {

            highScore = 0;
            bestPlayerName = "";
        }
        // If no highscore saved, don't print the "name" part
        highScoreText.SetText($"Best Score : {highScore} {(bestPlayerName == "" ? "" : $"Name : {bestPlayerName}") }");
    }
}
