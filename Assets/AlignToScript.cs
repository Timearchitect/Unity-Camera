using UnityEngine;

public class AlignToTerrain : MonoBehaviour
{

    public void Start()
    {
        AlignToSurface();
    }

    public void AlignToSurface()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            Vector3 terrainPosition = terrain.transform.position;
            float terrainHeight = terrain.SampleHeight(transform.position) + terrainPosition.y;
            transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
        }
        else
        {
            Debug.LogWarning("No active terrain found.");
        }
    }
}
