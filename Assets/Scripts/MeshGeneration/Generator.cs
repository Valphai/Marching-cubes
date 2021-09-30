using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace MeshGeneration
{
    // [ExecuteInEditMode]
    public class Generator : MonoBehaviour
    {
        private ComputeBuffer triangsBuffer;
        private ComputeBuffer positionsBuffer;
        private DensityGenerator densityGenerator;
        private int maxResolution = 16; 

        [SerializeField] private Vector3Int numberOfChunks;
        
        [Tooltip("Number of points per axis")]
        [Range (2, 16)]
        [SerializeField] private int resolution = 8; 
        [SerializeField] private float isoLevel;
        [SerializeField] private ComputeShader cubeShader;
        [SerializeField] private ComputeShader densityShader;
        [SerializeField] private NoiseSettings noiseSettings;
        [SerializeField] private Material chunkMaterial;

        private struct Triangle
        {
            public Vector3 vert0;
            public Vector3 vert1;
            public Vector3 vert2;
        }

        private void Awake() 
        {
            densityGenerator = new DensityGenerator(noiseSettings, densityShader);
        }
        private void Update()
        {
            InitBuffers();
            InitChunks();
            RemoveUnnecessaryChunks();
        }

        private void RemoveUnnecessaryChunks()
        {
            Vector3Int oldPosition = new Vector3Int(numberOfChunks.x * (resolution - 1), 
                                                    numberOfChunks.y * (resolution - 1), 
                                                    numberOfChunks.z * (resolution - 1));

            for (int i = 0; i < Chunk.chunks.Count; i++)
            {
                if (Chunk.chunks[i].ChunkCenterPosition.x > oldPosition.x || 
                    Chunk.chunks[i].ChunkCenterPosition.y > oldPosition.y || 
                    Chunk.chunks[i].ChunkCenterPosition.z > oldPosition.z)
                {
                    Destroy(Chunk.chunks[i].gameObject);
                    Chunk.chunks.RemoveAt(i);
                }
            }
        }

        private void OnEnable()
        {
            InitBuffers();
        }
        private void OnDisable()
        {
            ReleaseBuffers();
        }
        private void InitChunks()
        {
            MakeNewChunks();
            foreach (Chunk chunk in Chunk.chunks)
            {
                GenerateMesh(chunk);
            }
        }

        private void MakeNewChunks()
        {
            for (int x = 0; x <= numberOfChunks.x; x++)
            {
                for (int y = 0; y <= numberOfChunks.y; y++)
                {
                    for (int z = 0; z <= numberOfChunks.z; z++)
                    {
                        bool exists = false;
                        Vector3Int newPosition = new Vector3Int(x * (resolution - 1), y * (resolution - 1), z * (resolution - 1));
                        for (int i = 0; i < Chunk.chunks.Count; i++)
                        {
                            if (Chunk.chunks[i].ChunkCenterPosition == newPosition)
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (!exists || Chunk.chunks.Count == 0)
                        {
                            GameObject chunkObj = new GameObject();
                            Chunk chunk = chunkObj.AddComponent<Chunk>();
                            chunk.ChunkCenterPosition = newPosition;
                            chunk.InitChunk(chunkMaterial);
                        }
                    }
                }
            }
        }

        private void InitBuffers()
        {
            // maximum triangle configuration yields 5
            int maxTriangleCount = resolution * resolution * resolution * 5;
            int maxNumOfPoints = maxResolution * maxResolution * maxResolution;

            // 3 positions in struct each being v3
            triangsBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);

            // just 1 vec4 meaning 4 floats
            positionsBuffer = new ComputeBuffer(maxNumOfPoints, sizeof(float) * 4);
        }
        private void ReleaseBuffers()
        {
            triangsBuffer.Release();
            positionsBuffer.Release();
            triangsBuffer = null;
            positionsBuffer = null;
        }
        private void GenerateMesh(Chunk chunk)
        {
            densityGenerator.Generate(positionsBuffer, resolution, chunk.ChunkCenterPosition);

            triangsBuffer.SetCounterValue(0);
            cubeShader.SetBuffer(0, "Positions", positionsBuffer);
            cubeShader.SetBuffer(0, "Triangs", triangsBuffer);

            cubeShader.SetInt("Resolution", resolution);
            cubeShader.SetFloat("IsoLevel", isoLevel);

            int numThreadsPerAxis = Mathf.CeilToInt(resolution / 8f);
            cubeShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

            int numOfTriangs = triangsBuffer.count;

            Triangle[] triang = new Triangle[numOfTriangs];
            triangsBuffer.GetData(triang);

            Mesh mesh = chunk.mesh;
            mesh.Clear();

            CalculateVertTri(ref mesh, triang, numOfTriangs);

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();  // if URP
        }

        private void CalculateVertTri(ref Mesh mesh, Triangle[] triang, int numOfTriangs)
        {
            List<Vector3> vertexes = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i < numOfTriangs; i++)
            {
                vertexes.Add(triang[i].vert0);
                vertexes.Add(triang[i].vert1);
                vertexes.Add(triang[i].vert2);

                triangles.Add(vertexes.Count - 1);
                triangles.Add(vertexes.Count - 2);
                triangles.Add(vertexes.Count - 3);
            }

            mesh.vertices = vertexes.ToArray();
            mesh.triangles = triangles.ToArray();
        }
    }
}
