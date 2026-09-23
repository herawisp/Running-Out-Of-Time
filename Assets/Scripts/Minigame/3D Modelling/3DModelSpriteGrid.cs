using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteGrid
{
    [SerializeField] 
    private List<ObjectSprites> rows = new();

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