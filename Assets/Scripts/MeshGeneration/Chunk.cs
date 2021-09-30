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
        public static List<Chunk> chunks = new List<Chunk>();
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        [SerializeField] private Material material;

        private void Awake() => chunks.Add(this);
        private void OnEnable() 
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