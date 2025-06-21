using UnityEngine;

public static class CoordinateNormalizer
{
    public static Vector2 TransformToVR(Vector2 normalizedInput)
    {
        Vector2 rotated = new Vector2(-normalizedInput.y, normalizedInput.x);
        float xNormalized = (rotated.x + 0.72f) / 0.72f;
        float yNormalized = rotated.y;
        float targetWidth = 1193f - 157f;
        float targetHeight = 566f - (-411f);
        float mappedX = 157f + xNormalized * targetWidth;
        float mappedY = -411f + yNormalized * targetHeight;
        return new Vector2(mappedX, mappedY);
    }

    public static Vector2 ReverseToTable(Vector2 input)
    {
        float targetWidth = 1193f - 157f;
        float targetHeight = 566f - (-411f);
        float xNormalized = (input.x - 157f) / targetWidth;
        float yNormalized = (input.y - (-411f)) / targetHeight;
        float rotatedX = xNormalized * 0.72f - 0.72f;
        float rotatedY = yNormalized;
        float originalX = rotatedY;
        float originalY = -rotatedX;
        return new Vector2(originalX, originalY);
    }
}
