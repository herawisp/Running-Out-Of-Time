using UnityEngine;

[System.Serializable]
public class ArtData
{
    public string title;
    [TextArea(3, 10)]
    public string description;
    public Sprite artSprite;
}