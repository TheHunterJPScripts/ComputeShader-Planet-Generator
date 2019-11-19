using UnityEngine;

[CreateAssetMenu(fileName = "Noise", menuName = "Own/Noise", order = 0)]
public class NoiseData : ScriptableObject
{
    [HideInInspector]
    public Vector3[] offsets;
    public int octaves;
    public float lacunarity;
    public float persistence;
    public float noiseScale;
    public float weightMultiplier;

    public void RecalculateOffsets() {
        offsets = new Vector3[octaves];
        for (int i = 0; i < offsets.Length; i++) {
            offsets[i] = new Vector3(Random.Range(-10000.0f, 10000.0f), Random.Range(-10000.0f, 10000.0f), Random.Range(-10000.0f, 10000.0f));
        }
    }
}
