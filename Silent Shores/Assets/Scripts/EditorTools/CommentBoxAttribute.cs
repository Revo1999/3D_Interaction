using UnityEngine;

public class CommentBoxAttribute : PropertyAttribute
{
    public string message;
    public int messageType; // 0 = Info, 1 = Warning, 2 = Error

    public CommentBoxAttribute(string message, int messageType = 0)
    {
        this.message = message;
        this.messageType = messageType;
    }
}
