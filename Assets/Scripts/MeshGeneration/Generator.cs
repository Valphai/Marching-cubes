using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace MeshGeneration
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Generator : MonoBehaviour
    {
        public Cube Cube;
        private DensityFunction densityFunction;
        private Mesh mesh;
        private List<Vector3> vertexes = new List<Vector3>();
        private List<int> triangles = new List<int>();
        [SerializeField] private DrawChunkDetails chunkDetails;
        [SerializeField] private NoiseSettings noiseSettings;
        [SerializeField] private int xSize, zSize;
        [Range(.01f, 2f)]
        [SerializeField] private float marchSpeed;
    
        private void Awake() 
        {
            Cube = new Cube();
    
            chunkDetails = gameObject.AddComponent<DrawChunkDetails>();
            chunkDetails.cube = Cube;
            densityFunction = new DensityFunction(noiseSettings);
        }
        private void FixedUpdate() 
        {
            MarchingCube(Cube);
            // StartCoroutine(MarchingCube(Cube));
        }
        private void MeshStartingSetup()
        {
            GetComponent<MeshFilter>().mesh = new Mesh();
            
            vertexes.Clear();
            triangles.Clear();
        }
    
        private void SetMesh(Vector3[] vert, int[] tri)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            // mesh.Clear();
    
            mesh.vertices = vert;
            mesh.triangles = tri;
    
            mesh.RecalculateNormals();
            //m.RecalculateTangents();  //if URP
        }
        private void MarchingCube(Cube cube)
        {
            cube.corners = new Vector4[8];
            MeshStartingSetup();
            
            for (int x = 0; x < xSize; x++)
            {
                for (int y = -8; y < xSize - 8; y++)
                {
                    for (int z = 0; z < zSize; z++)
                    {
                        cube.corners[0] = new Vector3(x, y, z);
                        cube.corners[1] = new Vector3(x, y, z) + Vector3.up;
                        cube.corners[2] = new Vector3(x, y, z) + Vector3.up + Vector3.right;
                        cube.corners[3] = new Vector3(x, y, z) + Vector3.right;
                        cube.corners[4] = new Vector3(x, y, z) + Vector3.forward;
                        cube.corners[5] = new Vector3(x, y, z) + Vector3.up + Vector3.forward;
                        cube.corners[6] = new Vector3(x, y, z) + Vector3.up + Vector3.forward + Vector3.right;
                        cube.corners[7] = new Vector3(x, y, z) + Vector3.forward + Vector3.right;
    
                        for (int i = 0; i < cube.corners.Length; i++)
                        {
                            cube.corners[i].w = densityFunction.PointDensity((Vector3)cube.corners[i]);
                        }
    
                        MarchTheCube(cube);
                        SetMesh(vertexes.ToArray(), triangles.ToArray());
    
                        // yield return new WaitForSeconds(marchSpeed);
                    }
                }
            }
            // Debug.Log("done");
        }
        private void MarchTheCube(Cube cube)
        {
            // Calculate the index of the current cube configuration
            // the for loop maps cubeIndex to [0,255]
            int cubeIndex = 0;
            for (int i = 0; i < 8; i++)
            {
                if (cube.corners[i].w < noiseSettings.Ground)
                {
                    cubeIndex |= 1 << i;
                }
            }
    
            // Looping over each edge sum that make up a triangle
            Func<int, int> edgeIndexOfTheCube = i => Table.Triangulation[cubeIndex, i];
            for (int i = 0; edgeIndexOfTheCube(i) != -1; i += 3)
            {
                int cornIndexA0 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i), 0];
                int cornIndexB0 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i), 1];
    
                int cornIndexA1 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i + 1), 0];
                int cornIndexB1 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i + 1), 1];
    
                int cornIndexA2 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i + 2), 0];
                int cornIndexB2 = Table.CornerIndexFromEdge[edgeIndexOfTheCube(i + 2), 1];
    
                Vector3[] VertexPositions = new Vector3[] {
                    InterpolateVertecies(cube.corners[cornIndexA0], cube.corners[cornIndexB0]),
                    InterpolateVertecies(cube.corners[cornIndexA1], cube.corners[cornIndexB1]),
                    InterpolateVertecies(cube.corners[cornIndexA2], cube.corners[cornIndexB2]),
                };
    
                vertexes.Add(VertexPositions[0]);
                vertexes.Add(VertexPositions[1]);
                vertexes.Add(VertexPositions[2]);
    
                triangles.Add(vertexes.Count - 1);
                triangles.Add(vertexes.Count - 2);
                triangles.Add(vertexes.Count - 3);
            }
        }
        private Vector3 InterpolateVertecies(Vector4 v1, Vector4 v2)
        {
            float t = (noiseSettings.Ground - v1.w) / (v2.w - v1.w);
            return (Vector3)v1 + t * ((Vector3)v2 - (Vector3)v1);
        }
}
}