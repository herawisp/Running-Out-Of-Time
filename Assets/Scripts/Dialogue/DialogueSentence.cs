using UnityEngine;

[System.Serializable]
public class DialogueSentence
{
    public Sprite speakerAvatar;
    [TextArea(3, 5)]
    public string sentence;
}

[System.Serializable]
public class Dialogue
{
    public DialogueSentence[] sentences;
}