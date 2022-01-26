using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainGenerator terrainGenerator = (TerrainGenerator)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            terrainGenerator.NewMesh();
        }
    }
	
}