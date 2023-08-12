using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : MonoBehaviour
{
    public GameObject groundTile;
    public Material HellGroundMat;

    private int groundSizex;
    private int groundSizey;

    public Tiles groundTiles;

    private int hellRadius;

    public Transform hellStartPos;
    public float hellExpansionRate = 0.5f;
    private float prevtime = 0;

    private List<(Vector3,int)> hellNodes; 

    void Start()
    {
        groundSizex = (int)transform.localScale.x;
        groundSizey = (int)transform.localScale.z ;
        groundTiles = new Tiles(transform.position, groundSizex, groundSizey);

        transform.localScale = new Vector3(1, 0.1f, 1);

        SpawnTiles();
        BoxCollider c = gameObject.AddComponent<BoxCollider>();
        c.size = new Vector3(groundSizex , 0.1f, groundSizey );

        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject.GetComponent<MeshFilter>());

        hellNodes = new List<(Vector3, int)>();
        CreateHellNode(hellStartPos.position);
    }

    void Update()
    {
        //expands hell at determined rate
        if(Time.time - prevtime >= hellExpansionRate)
        {
            ExpandHell();
            prevtime = Time.time;
        }

    }

    void SpawnTiles()
    {
        for (int i = -groundSizex / 2 ; i <= groundSizex / 2; i++) 
        {
            for (int j = -groundSizey / 2; j <= groundSizey / 2; j++)
            {
                GameObject g = Instantiate(groundTile, transform.position + new Vector3(i, 0, j), Quaternion.identity, transform);
                groundTiles.AddTile(g);
            }
        }
    }

    public void CreateHellNode(Vector3 pos)
    {
        hellNodes.Add((pos,0));
        ExpandHell();
    }

    void ExpandHell()
    {
        foreach(var hNode in hellNodes)
        {
            IncreaseHell(hNode,new Vector2(1, 1));    //top right corner
            IncreaseHell(hNode,new Vector2(1, -1));   //bot right corner  
            IncreaseHell(hNode,new Vector2(-1, 1));   //top left corner
            IncreaseHell(hNode,new Vector2(-1, -1));  //bot left corner
        }

        //increase radius - probably better way
        for (int i = 0; i < hellNodes.Count; i++)
        {
            var node = hellNodes[i];
            node.Item2 += 1;
            hellNodes[i] = node;
        }
    }

    void IncreaseHell((Vector3,int) node,Vector2 dir)
    {
        int xdir = (dir.x >= 0) ? 1 : -1;
        int ydir = (dir.y >= 0) ? 1 : -1;
        int radius = node.Item2;

        if (radius > groundSizex && radius > groundSizey) { return; }
        if(radius == 0)
        {
            Vector3 pos = node.Item1 + new Vector3(xdir * 1, 0,ydir * 1);
            if (groundTiles.GetTile(pos, out GameObject tile))
            {
                ConvertTile(ref tile);
            }
            return;
        }

        for (int i = 0; i < radius; i++)
        {
            Vector3 newPos = new Vector3(xdir * (i), 0, ydir * (radius - (i+1)));

            Vector3 pos = node.Item1 + newPos;
            if (groundTiles.GetTile(pos, out GameObject tile))
            {
                ConvertTile(ref tile);
            }
        }
    }

    private void ConvertTile(ref GameObject _tile)
    {
        Tile tileScr = _tile.GetComponent<Tile>();
        if (tileScr.faction == Tile.Faction.normal)
        {
            _tile.GetComponent<Renderer>().material = HellGroundMat;
            tileScr.faction = Tile.Faction.hell;
        }
    }


    void TestGrid()
    {
        List<Vector3> testPos = new List<Vector3>() { new Vector3(25, 0, 0), new Vector3(0, 0, 0), new Vector3(10, 0, 10), new Vector3(-10, 0, -15) };

        foreach (var v in testPos)
        {
            Vector3 tilePos = transform.position + v;
            if (groundTiles.GetTile(tilePos, out GameObject t))
            {
                t.GetComponent<Renderer>().material = HellGroundMat;
            }
        }
    }

}


public struct Tiles
{
    List<GameObject> groundTiles;
    Vector3 worldPosOrigin;
    Vector3 centrePos;
    (int, int) groundsize;

    public Tiles(Vector3 _worldPosOrigin, int width,int length)
    {
        worldPosOrigin = _worldPosOrigin;
        groundTiles = new List<GameObject>();
        groundsize = (width, length + 1);
        centrePos = new Vector3(length / 2, 0, width / 2);
    }

    public void AddTile(GameObject _tile)
    {
        groundTiles.Add(_tile);
    }
    public bool GetTile(Vector3 worldPos, out GameObject tile)
    {
        Vector3 pos = worldPos - worldPosOrigin;    //relative to the grid
        Vector3 offset = centrePos + pos;   //offset from the start of grid
        return GetTile((int)offset.x, (int)offset.z, out tile);
    }
    //gets tile from list from x,y coords in grid
    public bool GetTile(int x , int y, out GameObject tile)
    {
        tile = null;

        //as tiles are stored in 1d array but spawned like 2d array
        int index = groundsize.Item2 * x + y;
        if(index < 0 || index >= groundTiles.Count) { return false; }

        tile = groundTiles[index];

        return index <= groundTiles.Count;
    }


}