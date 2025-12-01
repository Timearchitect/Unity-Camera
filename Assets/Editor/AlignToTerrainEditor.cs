using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignToTerrain))]
public class AlignToTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AlignToTerrain alignToTerrain = (AlignToTerrain)target;
        if (GUILayout.Button("Align to Terrain"))
        {
            alignToTerrain.AlignToSurface();
        }
    }
}