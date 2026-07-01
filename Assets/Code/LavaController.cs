using UnityEngine;

public class LavaController : MonoBehaviour
{
    public Material lavaMaterial;

    void Start()
    {
        lavaMaterial.SetVector("_LavaSpeed", new Vector2(0.1f, 0f));
        lavaMaterial.SetVector("_LavaNormalSpeed", new Vector2(0.05f, 0f));
        lavaMaterial.SetVector("_NoiseSpeed", new Vector2(0.05f, 0.05f));
        lavaMaterial.SetVector("_VoronoiSpeed", new Vector2(0.05f, 0.05f));
    }
}