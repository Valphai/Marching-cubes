using System.Collections.Generic;
using UnityEngine;

namespace MeshGeneration
{
    [System.Serializable]
    public class DrawChunkDetails : MonoBehaviour
    {
        [HideInInspector] public Cube cube;
        private List<Vector4> cubeGizmos = new List<Vector4>();
        [SerializeField] private bool drawCube;
        [SerializeField] private bool drawDensities;
    
        private void OnDrawGizmos() 
        {
            if (drawCube)
                Cube(cube);
    
            if (drawDensities)
                Densities();
        }
        private void Cube(Cube cube)
        {
            if (cube == null) return;
    
            Gizmos.DrawCube((cube.corners[1] + cube.corners[7]) / 2, Vector3.one);
        }
    
        private void Densities()
        {
            foreach (Vector4 vec in cubeGizmos)
            {
                Color c = new Color(vec.w, 0, 1, 1);
                Gizmos.color = c;
    
                Gizmos.DrawSphere((Vector3)vec, .25f);
            }
        }
    }
}