using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIUtils
{
    public static bool ApproximatePositionReached(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z)) <= GlobalAISettings.DISTANCE_THRESHOLD;
    }
    public static Quaternion RotateY(Vector3 direction)
    {
        float rot_z = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 270 - rot_z, 0f);
    }
    public static Quaternion LookAt(Vector3 from, Vector3 to) {
        Vector3 direction = (from - to).normalized;
        float rot_z = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 270 - rot_z, 0f);
    }
}
