using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Cache = UnityEngine.Cache;

public class DungeonManager : MonoBehaviour
{

    public int dungeonWidth;
    public int dungeonLenght;
    public int roomWidthMin;
    public int roomLenghtMin;
    public int corridorwidth;
    public int maxIterations;
    public Material material;
    [Range(0.0f,0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f,1.0f)]
    public float roomTopCornerModifier;
    [Range(0.0f,2.0f)]
    public int roomOffset;

    public GameObject wallVertical;
    public GameObject wallHorizontal;
    public List<GameObject> decorativeAssets;
    public GameObject assetPrefab;
    
    private List<Vector3Int> possibleDoorVerticalPosition;
    private List<Vector3Int> possibleDoorHorizontalPosition;
    private List<Vector3Int> possibleWallVerticalPosition;
    private List<Vector3Int> possibleWallHorizontalPosition;
    
    private List<Vector3Int> possiblAssetPosition;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLenght);
        var roomList = generator.CalculateRooms(maxIterations, roomWidthMin, roomLenghtMin,
            roomBottomCornerModifier, roomTopCornerModifier, roomOffset, corridorwidth);
        GameObject wallParent = new GameObject("Wall_Parent");
        GameObject assetParent = new GameObject("Asset_Parent");
        wallParent.transform.parent = transform;
        assetParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            CreateMeshes(roomList[i].bottomLeftAreaCorner, roomList[i].topRightAreaCorner);
            
        }

        CreateWalls(wallParent);
        CreateAssets(assetParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallposition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent,wallposition,wallHorizontal);
           
        }

        foreach (var wallposition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallposition,wallVertical);
           
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallposition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallposition, Quaternion.identity, wallParent.transform);
    }
    
    public void CreateAssets(GameObject assetParent)
    {
        foreach (var assetposition  in possiblAssetPosition)
        {
           initiateAsset(assetParent,assetposition,assetPrefab);
           
        }
        
    }

    public void initiateAsset(GameObject assetParent, Vector3Int assetPosition, GameObject assetPrefab)
    {
        Instantiate(assetPrefab, assetPosition, Quaternion.identity, assetParent.transform);
    }
    
    private void CreateMeshes(Vector2 bottomleftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomleftCorner.x, 0, bottomleftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomleftCorner.y);
        Vector3 topLeftV = new Vector3(bottomleftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        GameObject dungeonFloor = new GameObject
            ("Mesh"+bottomleftCorner, typeof(MeshFilter), 
                  typeof(MeshRenderer), typeof(MeshCollider), typeof(NavMeshSurface));
        
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshCollider>().convex = true;
        dungeonFloor.GetComponent<MeshCollider>().sharedMesh = mesh;
        dungeonFloor.transform.parent = transform;
        dungeonFloor.GetComponent<NavMeshSurface>().BuildNavMesh();
       
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }

        for (int row =(int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallposition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallposition, possibleWallHorizontalPosition,possibleDoorHorizontalPosition);
        }

        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }

        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }


  
    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount !=0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
