using UnityEngine;

namespace Utility
{
    public class DensityGenerator
    {
        private NoiseSettings settings;
        private ComputeShader densityShader;

        public DensityGenerator(NoiseSettings settings, ComputeShader densityShader)
        {
            this.settings = settings;
            this.densityShader = densityShader;
        }
        public void Generate(ComputeBuffer positionsBuffer, int resolution, Vector3 chunkCenter)
        {
            densityShader.SetBuffer(0, "Positions", positionsBuffer);
            densityShader.SetInt("Resolution", resolution);
            densityShader.SetVector("OffsetPoint", settings.OffsetPoint);
            densityShader.SetVector("ChunkCenter", chunkCenter);

            densityShader.SetInt("NumLayers", settings.NumLayers);
            densityShader.SetFloat("NoiseWeight", settings.NoiseWeight);
            densityShader.SetFloat("BaseRoughness", settings.BaseRoughness);
            densityShader.SetFloat("Roughness", settings.Roughness);
            densityShader.SetFloat("Persistance", settings.Persistance);

            densityShader.SetFloat("FloorOffset", settings.FloorOffset);
            densityShader.SetFloat("HardFloor", settings.HardFloor);
            densityShader.SetFloat("FloorWeight", settings.FloorWeight);
            densityShader.SetFloat("HardFloorWeight", settings.HardFloorWeight);

            int numThreadsPerAxis = Mathf.CeilToInt(resolution / 8f);
            densityShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);
        }
    }
}