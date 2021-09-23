using UnityEngine;

[CreateAssetMenu()]
public class NoiseSettings : ScriptableObject
{
    [Range(1,8)]
    public int NumLayers = 1;
    public float Persistance = .5f;
    public float Strength = 1;
    public float BaseRoughness = 1;
    public float Roughness = 2;
    public Vector3 Center;
    public int UpperBoundary, Ground, LowerBoundry;
}