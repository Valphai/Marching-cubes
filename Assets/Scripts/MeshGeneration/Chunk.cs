using System.Collections.Generic;
using UnityEngine;

namespace MeshGeneration
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Chunk : MonoBehaviour
    {
        [HideInInspector]
        public Mesh mesh;
        
        [HideInInspector]
        public Vector3 ChunkCenterPosition;

        [HideInInspector]
        public static List<Chunk> chunks = new List<Chunk>();
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private void Awake() => chunks.Add(this);
        public void InitChunk(Material material) 
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            mesh = meshFilter.sharedMesh;
    
            if (!mesh)
            {
                mesh = new Mesh();
                meshFilter.sharedMesh = mesh;
            }
    
            meshRenderer.material = material;
        }
        private void OnDisable() 
        {
            mesh.Clear();
        }
    }
}