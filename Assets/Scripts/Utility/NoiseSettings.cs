using UnityEngine;

namespace Utility
{
    [CreateAssetMenu()]
    public class NoiseSettings : ScriptableObject
    {
        [Range(1,8)]
        public int NumLayers = 1;
        public float Persistance = .5f;
        public float NoiseWeight = 2;
        public float BaseRoughness = 1;
        public float Roughness = 2;
        public float FloorOffset;
        public float HardFloor;
        public float FloorWeight;
        public float HardFloorWeight;
        public Vector3 OffsetPoint;
    }
}