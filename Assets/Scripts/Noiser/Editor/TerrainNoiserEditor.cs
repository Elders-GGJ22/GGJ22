using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainNoiser))]
[CanEditMultipleObjects]
public class TerrainNoiserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainNoiser terrainNoiser = (TerrainNoiser)target;
        terrainNoiser.GenerateTerrain();
    }
}