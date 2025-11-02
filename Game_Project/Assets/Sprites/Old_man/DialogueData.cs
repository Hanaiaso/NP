// File: DialogueData.cs
using UnityEngine;

// [System.Serializable] cho phép ta chỉnh sửa nó trên Inspector
[System.Serializable]
public class DialogueLine
{
    public string speakerName; // Tên: "Ông lão", "Player"
    public Sprite portrait;    // Ảnh chân dung của người nói

    [TextArea(3, 10)] // Cho ô text to hơn
    public string text; // Nội dung lời thoại
}

[System.Serializable]
public class Conversation
{
    public DialogueLine[] lines; // Một mảng chứa tất cả các lời thoại
}