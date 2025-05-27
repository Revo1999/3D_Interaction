using UnityEngine;

public class CommentBoxAttribute : PropertyAttribute
{
    public readonly string message;
    public readonly int messageType;

    public CommentBoxAttribute(string message, int messageType = 0)
    {
        this.message = message;
        this.messageType = messageType;
    }
}
