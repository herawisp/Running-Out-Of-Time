using System;
using System.Collections.Generic;
using UnityEngine;

public class ModellingGame : MonoBehaviour
{
    [Header("Minigame Stats")]
    [SerializeField] private int score = 0;
    [SerializeField] private int level = 0;
    [SerializeField] private int objectFinished = 0;

    [Header("Settings")]
    [SerializeField] private int objectNeeded = 3;
    [SerializeField] private int scoreThreshold = 5;
    [SerializeField] private SpriteGrid spriteGrid = new SpriteGrid();

    [Header("References")]
    [SerializeField] private SpriteRenderer objectImage;
    [SerializeField] private Timer timer;

    private List<Sprite> objectSprites;

    // Public properties for external UI or game state readers
    public int Score => score;
    public int Level => level;
    public int ObjectFinished => objectFinished;
    public int ObjectNeeded => objectNeeded;

    private void Awake()
    {
        if (timer == null)
            timer = GetComponent<Timer>();

        if (objectImage == null)
            objectImage = GetComponent<SpriteRenderer>();
    }

    public void StartGame()
    {
        score = 0;
        level = 0;
        objectFinished = 0;

        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
            timer.OnTimeout += HandleTimeout;
            timer.StartTimer();
        }

        LoadNextObject();
    }

    public void IncreaseClick()
    {
        score++;

        if (score >= scoreThreshold)
        {
            score = 0;
            level++;
            UpdateObjectImage();
        }
    }

    private void UpdateObjectImage()
    {
        if (objectSprites == null || objectImage == null) return;

        if (level < objectSprites.Count)
        {
            objectImage.sprite = objectSprites[level];
        }
        else
        {
            OnObjectCompleted();
        }
    }

    private void OnObjectCompleted()
    {
        objectFinished++;
        Debug.Log($"Object Finished! ({objectFinished}/{objectNeeded})");

        if (objectFinished >= objectNeeded)
        {
            OnGameWon();
        }
        else
        {
            LoadNextObject();
        }
    }

    private void LoadNextObject()
    {
        level = 0;
        score = 0;
        objectSprites = spriteGrid.GetRandomRow();

        if (objectSprites != null && objectSprites.Count > 0 && objectImage != null)
        {
            objectImage.sprite = objectSprites[level];
        }
        else
        {
            Debug.LogWarning("Failed to load sprites for the next object.");
        }
    }

    private void OnGameWon()
    {
        Debug.Log("You Won! All required objects have been created.");

        if (timer != null)
        {
            timer.StopTimer();
        }

        // TODO: Add additional win logic here (e.g., trigger UI, load next scene)
    }

    private void HandleTimeout()
    {
        Debug.Log("Game Over! Time ran out.");
        // TODO: Add loss/game over logic here
    }

    private void OnDestroy()
    {
        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
        }
    }
}

// ====================================================================================================================================//

[Serializable]
public class ObjectSprites
{
    public List<Sprite> sprites = new List<Sprite>();
}

[Serializable]
public class SpriteGrid
{
    [SerializeField] 
    private List<ObjectSprites> rows = new List<ObjectSprites>();

    public List<Sprite> GetRandomRow()
    {
        if (rows == null || rows.Count == 0)
        {
            Debug.LogWarning("SpriteGrid has no rows configured!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, rows.Count);
        return rows[randomIndex].sprites;
    }

    public Sprite GetSprite(int row, int col)
    {
        if (rows != null && row >= 0 && row < rows.Count)
        {
            if (rows[row].sprites != null && col >= 0 && col < rows[row].sprites.Count)
            {
                return rows[row].sprites[col];
            }
        }
        return null;
    }
}