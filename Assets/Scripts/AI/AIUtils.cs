using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIUtils
{
    public static bool ApproximatePositionReached(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z)) <= GlobalAISettings.DISTANCE_THRESHOLD;
    }
}
