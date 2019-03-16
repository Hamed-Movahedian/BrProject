using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrMathExtention 
{
    public static Vector3 FindLineCircleIntersections(
        float cx, float cz, float radius,
        Vector3 point1, Vector3 point2)
    {
        float dx, dy, A, B, C, det, t;

        dx = point2.x - point1.x;
        dy = point2.z - point1.z;

        A = dx * dx + dy * dy;
        B = 2 * (dx * (point1.x - cx) + dy * (point1.z - cz));
        C = (point1.x - cx) * (point1.x - cx) +
            (point1.z - cz) * (point1.z - cz) -
            radius * radius;

        det = B * B - 4 * A * C;

        // Two solutions.
        t = (-B + Mathf.Sqrt(det)) / (2 * A);

        var intersection1 = new Vector3(point1.x + t * dx, 0, point1.z + t * dy);

        t = (-B - Mathf.Sqrt(det)) / (2 * A);

        var intersection2 =
            new Vector3(point1.x + t * dx, 0, point1.z + t * dy);
        
        if ((point2 - intersection1).sqrMagnitude < (point2 - intersection2).sqrMagnitude)
            return intersection1;
        
        return intersection2;

    }
}
