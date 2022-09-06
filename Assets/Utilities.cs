using UnityEngine;

public static class Utilities
{
    public static Vector2 RotateVector2(Vector2 vector, float rotation)
    {
        float radians = Mathf.Deg2Rad * rotation;
        return new Vector2(
            vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians),
            vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians));
    }

    public static Vector3 RotateVector3(Vector3 vector, float rotation)
    {
        return Quaternion.Euler(0, rotation, 0) * vector;
    }
}
