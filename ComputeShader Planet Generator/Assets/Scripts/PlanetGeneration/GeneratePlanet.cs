using UnityEngine;

public class GeneratePlanet : MonoBehaviour {
    Planet planet;
    public float radius = 10;
    public int div = 6;
    public float scale = 10;
    public NoiseData noise;
    public Material terrainMaterial;
    public Material waterMaterial;

    bool generated = false;

    public void Build() {
        if (generated)
            planet.Delete();
        planet = new Planet(transform.position, radius, div, scale);
        generated = true;
    }
    private void Update() {
        if(generated == true)
            planet.Update(transform.position, radius, noise, scale, terrainMaterial, waterMaterial);
    }
}
