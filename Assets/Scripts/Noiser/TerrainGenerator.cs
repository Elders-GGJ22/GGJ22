using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour {

    [SerializeField] private Vector2Int terrainSize = new Vector2Int(20, 20);
    //[SerializeField] private float distanceToReset = 5f;
    //[SerializeField] private Vector2Int terrainOffset = Vector2Int.zero;
    //[SerializeField] private Transform target;
    [Range(0, 1f)] [SerializeField] private float distanceSmoothing = 0.3f;
    [SerializeField] private float maxHeight = 2f;

    private Vector2Int terrainOffset;

#if UNITY_EDITOR
    [SerializeField] private bool ShowGizmos = true;
#endif

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    public void NewMesh ()
    {
        terrainOffset = new Vector2Int((int)this.transform.position.x - (int)(terrainSize.x / 2f), (int)this.transform.position.z - (int)(terrainSize.y / 2f));
        
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear(); //necessary?
        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles();
        mesh.RecalculateNormals();
        this.GetComponent<MeshCollider>().sharedMesh = mesh; //reset collider\\
    }

    /*void FixedUpdate() //just for do in not every frame\\
    {
        //if( (new Vector3 (terrainOffset.x, 0, terrainOffset.y) - target.transform.position).magnitude > distanceToReset ) //not working, because the offset
        //{
        NewMesh();
        //}
    }*/

    Vector3[] CreateVertices()
    {
        vertices = new Vector3[(terrainSize.x + 1) * (terrainSize.y + 1)];

        for (int i = 0, y = terrainOffset.y; y <= terrainSize.y + terrainOffset.y; y++)
        {
            for(int x = terrainOffset.x; x <= terrainSize.x + terrainOffset.x; x++)
            {
                float height = Mathf.PerlinNoise(x * distanceSmoothing, y * distanceSmoothing) * maxHeight;
                vertices[i] = new Vector3(x, height, y);
                i++;
            }
        }

        return vertices;
    }

    int[] CreateTriangles()
    {
        triangles = new int[terrainSize.x * terrainSize.y * 6];

        int vert = 0;
        int tris = 0;

        for (int y = 0; y < terrainSize.y; y++)
        {
            for (int x = 0; x < terrainSize.x; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + terrainSize.x + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + terrainSize.x + 1;
                triangles[tris + 5] = vert + terrainSize.x + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        return triangles;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if(!ShowGizmos || vertices == null) { return; }
        foreach(Vector3 vertice in vertices)
        {
            Gizmos.DrawSphere(vertice, 0.1f);
        }
    }
#endif

}