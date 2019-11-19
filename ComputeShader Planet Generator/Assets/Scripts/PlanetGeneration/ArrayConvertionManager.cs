using UnityEngine;

public class ArrayConvertionManager {
    static public Vector3[] Vector3ToVector3Height(Vector3[] input, float radius, NoiseData noiseData) {
        // We load the compute shader to prevent null references.
        ComputeShader shader = Resources.Load<ComputeShader>("ComputeShaders/MeshGenerator");
        // Find the kernel for the function we want to execute.
        int kernel = shader.FindKernel("CSMain");

        Vector3[] output = new Vector3[input.Length];

        // Set all the buffers
        ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, 12);
        ComputeBuffer outputBuffer = new ComputeBuffer(output.Length, 12);
        ComputeBuffer offsetBuffer = new ComputeBuffer(noiseData.offsets.Length, 12);

        // Fill the buffers with the info.
        inputBuffer.SetData(input);
        offsetBuffer.SetData(noiseData.offsets);

        // Import the data into the shader.
        shader.SetBuffer(kernel, "input", inputBuffer);
        shader.SetBuffer(kernel, "output", outputBuffer);
        shader.SetFloat("radius", radius);
        shader.SetBuffer(kernel, "offsets", offsetBuffer);
        shader.SetInt("octaves", noiseData.octaves);
        shader.SetFloat("lacunarity", noiseData.lacunarity);
        shader.SetFloat("persistence", noiseData.persistence);
        shader.SetFloat("noiseScale", noiseData.noiseScale);
        shader.SetFloat("weightMultiplier", noiseData.weightMultiplier);

        shader.Dispatch(kernel, input.Length, 1, 1);
        outputBuffer.GetData(output);
        inputBuffer.Release();
        outputBuffer.Release();
        offsetBuffer.Release();

        return output;
    }
}
