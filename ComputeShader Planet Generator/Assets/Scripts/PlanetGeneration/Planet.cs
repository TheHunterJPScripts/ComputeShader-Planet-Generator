using UnityEngine;
using System.Collections;
using System.Threading;

public class Planet {
    public GameObject planet;
    Icosphere ico;
    bool generated = false;

    private Planet() {
    }

    public Planet(Vector3 center, float radius, int divisions, float scale) {
        // We make sure the radius of the planet will work as intendad.
        if(radius <= 1) {
            radius = 1;
            Debug.LogWarning("The radius can't be smaller than 1.0f.");
        }

        // We make sure resizing the planet will work as intendad.
        if (scale <= 0) {
            scale = 0.1f;
            Debug.LogWarning("The scale can't be smaller than 0.1f.");
        }

        // Prevent the mesh to overflow.
        if (divisions > 6) {
            divisions = 6;
            Debug.LogWarning("Can't divide a planet more than 6 times.");
        }

        // We Generate the Basic Icosphere points and triangles.
        ico = new Icosphere(center, radius);

        // We divide the icosphere n times(add smoothness).
        Thread t = new Thread(() => generated = ico.Subdivide(divisions));
        t.Start();
    }

    public void Update(Vector3 center, float radius, NoiseData noise, float scale, Material terrainMaterial, Material waterMaterial) {
        if (generated) {
            // We load the materials to prevent null references.
            if (terrainMaterial == null) {
                //terrainMaterial = Resources.Load<Material>("Materials/StandardTerrain");
                terrainMaterial = new Material(Resources.Load<Material>("Materials/StandardTerrain"));
                terrainMaterial.SetColor("_Sand", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
                terrainMaterial.SetColor("_Grass", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
                terrainMaterial.SetColor("_DarkGrass", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
                terrainMaterial.SetColor("_Rocks", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
                terrainMaterial.SetColor("_DarkRocks", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
                terrainMaterial.SetColor("_Snow", new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1f));
            }
            if (waterMaterial == null) {
                waterMaterial = Resources.Load<Material>("Materials/StandardWater");
            }
            // We make sure the noise never get a null reference.
            if (noise == null) {
                noise = Resources.Load<NoiseData>("NoiseDatas/Standard");
            }

            // We make sure the planet is unique.
            noise.RecalculateOffsets();

            // Generate the GameObjects.
            // Terrain.
            planet = new GameObject();
            planet.name = "Planet";
            planet.transform.position = center;
            planet.AddComponent<MeshRenderer>().material = terrainMaterial;
            // Water.
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            water.transform.parent = planet.transform;
            water.GetComponent<MeshRenderer>().material = waterMaterial;
            water.transform.localScale *= radius * 2;

            // Generate Mesh.
            Mesh mesh = new Mesh();
            mesh.Clear();
            // We apply the noise to each point of the icosphere.
            mesh.vertices = ArrayConvertionManager.Vector3ToVector3Height(ico.GetPoints(), radius, noise); ;
            mesh.triangles = ico.GetTriangles();
            // We make sure to recalculate everything to good mesh behaviour.
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            
            // Apply mesh to the terrain.
            planet.AddComponent<MeshFilter>().mesh = mesh;
            planet.AddComponent<MeshCollider>().sharedMesh = mesh;
            planet.transform.localScale *= scale;
            generated = false;
        }
    }
    public void Delete() {
        GameObject.Destroy(planet);
    }
}
