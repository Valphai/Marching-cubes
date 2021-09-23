using UnityEngine;

public class DensityFunction
{
    private NoiseSettings settings;
    Noise noise = new Noise();

    public DensityFunction(NoiseSettings settings)
    {
        this.settings = settings;
    }
    public float PointDensity(Vector3 point)
    {
        float density = -point.y;

        // Layered noise
        float frequency = settings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.NumLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.Center);
            density += v * amplitude;
            frequency *= settings.Roughness;
            amplitude *= settings.Persistance;
        }
        density *= settings.Strength;

        // Debug.Log(density);
        return density;
    }

    // private float Gauss(Vector3 point)
    // {
    //     float density = Mathf.Exp(-.5f * (Mathf.Pow((point.y - settings.LowerBoundry) / settings.UpperBoundary, 2))) * .5f;
    //     density *= settings.Persistance;

    //     return density * settings.Strength;
    // }

    // private float Old(Vector3 point)
    // {
    //     // Layered noise
    //     float density = 0;
    //     float frequency = settings.BaseRoughness;
    //     float amplitude = 1;

    //     for (int i = 0; i < settings.NumLayers; i++)
    //     {
    //         float v = noise.Evaluate(point * settings.Roughness + settings.Center);
    //         density += v * amplitude;
    //         frequency *= settings.Roughness;
    //         amplitude *= settings.Persistance;
    //     }

    //     // float density = (1 - Mathf.Abs(Mathf.Sin(point)))*(1 - Mathf.Abs(Mathf.Sin(point)));
    //     // float density = noiseValue > settings.Ground ? noiseValue % settings.UpperBoundary : noiseValue % settings.LowerBoundry;

    //     // Clamp density 
    //     density = Mathf.InverseLerp(density, settings.LowerBoundry, settings.UpperBoundary);

    //     return density * settings.Strength;
    // }
}