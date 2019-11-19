using System.Collections.Generic;
using UnityEngine;

public class Icosphere {
    Vector3 center;
    float radius;
    List<Vector3> points;
    List<int> triangles;

    protected Icosphere() { }

    public Vector3[] GetPoints() {
        return points.ToArray();
    }
    public int[] GetTriangles() {
        return triangles.ToArray();
    }

    public Icosphere(Vector3 center, float radius) {
        this.center = center;
        this.radius = radius;

        // We generate the 12 vertices present on a icosphere.
        float t = ( 1.0f + Mathf.Sqrt(5.0f) ) / 2.0f;

        points = new List<Vector3> {
            new Vector3(-1, t, 0).normalized * radius + center,
            new Vector3(1, t, 0).normalized * radius + center,
            new Vector3(-1, -t, 0).normalized * radius + center,
            new Vector3(1, -t, 0).normalized * radius + center,
            new Vector3(0, -1, t).normalized * radius + center,
            new Vector3(0, 1, t).normalized * radius + center,
            new Vector3(0, -1, -t).normalized * radius + center,
            new Vector3(0, 1, -t).normalized * radius + center,
            new Vector3(t, 0, -1).normalized * radius + center,
            new Vector3(t, 0, 1).normalized * radius + center,
            new Vector3(-t, 0, -1).normalized * radius + center,
            new Vector3(-t, 0, 1).normalized * radius + center
        };

        // We add the 20 triangles that reference their
        // relative points.
        triangles = new List<int> {
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,
            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,
            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,
            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1 };
    }
    public bool Subdivide(int count) {
        // the icosphere with 0 disions is the initial one.
        if (count == 0) 
            return true;


        List<Vector3> p = new List<Vector3>();
        List<int> t = new List<int>();
        Dictionary<int, int> dic = new Dictionary<int, int>();

        // We insert all the basic points into the new list.
        for (int i = 0; i < points.Count; i++) {
            p.Add(points[i]);
        }

        // We divide each triangle.
        for (int i = 0; i < triangles.Count / 3; i++) {
            SubdivideTriangleNTimes(triangles[i * 3], triangles[i * 3+1], triangles[i * 3+2], ref dic, ref p, ref t, count, center, radius);
        }

        points = p;
        triangles = t;
        
        return true;
    }

    static private void SubdivideTriangleNTimes(int indexA, int indexB, int indexC, ref Dictionary<int, int> dictionary, ref List<Vector3> points, ref List<int> triangles, int times, Vector3 center, float radius) {
        // We don't want to continue the recursion.
        if (times == 0) {
            triangles.Add(indexA);
            triangles.Add(indexB);
            triangles.Add(indexC);

            return;
        }

        // Get the index avoiding duplicates.
        int indexAB = GetMidPointIndex(dictionary, points, indexA, indexB, center, radius);
        int indexBC = GetMidPointIndex(dictionary, points, indexB, indexC, center, radius);
        int indexCA = GetMidPointIndex(dictionary, points, indexC, indexA, center, radius);

        // Continue the recursion.
        SubdivideTriangleNTimes(indexA, indexAB, indexCA, ref dictionary, ref points, ref triangles, times - 1, center, radius);
        SubdivideTriangleNTimes(indexAB, indexBC, indexCA, ref dictionary, ref points, ref triangles, times - 1, center, radius);
        SubdivideTriangleNTimes(indexAB, indexB, indexBC, ref dictionary, ref points, ref triangles, times - 1, center, radius);
        SubdivideTriangleNTimes(indexCA, indexBC, indexC, ref dictionary, ref points, ref triangles, times - 1, center, radius);
    }
    static private int GetMidPointIndex(Dictionary<int, int> cache, List<Vector3> points, int indexA, int indexB, Vector3 center, float radius) {
        // We create a key out of the two original indices
        // by storing the smaller index in the upper two bytes
        // of an integer, and the larger index in the lower two
        // bytes. By sorting them according to whichever is smaller
        // we ensure that this function returns the same result
        // whether you call
        // GetMidPointIndex(cache, 5, 9)
        // or...
        // GetMidPointIndex(cache, 9, 5)
        int smaller_index = Mathf.Min(indexA, indexB);
        int greater_index = Mathf.Max(indexA, indexB);
        int key = ( smaller_index << 16 ) + greater_index;
        // If a midpoint is already defined, just return it.
        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;
        // If we're here, it's because a midpoint for these two
        // vertices hasn't been created yet. Let's do that now!
        Vector3 p1 = points[indexA];
        Vector3 p2 = points[indexB];
        Vector3 middle = ( Vector3.Lerp(p1, p2, 0.5f) - center ).normalized * radius + center;

        ret = points.Count;
        points.Add(middle);

        cache.Add(key, ret);
        return ret;
    }
}
