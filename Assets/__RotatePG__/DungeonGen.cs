using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 16;
    public int gridHeight = 8;

    public List<GameObject> tiles;

    private int[, ] graph;

    private GridTileConfig[,] tilesArr;

    public Vector2Int start;
    public Vector2Int end;
    private Dijsktra dijsktra;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        CreateGraph();
        GenerateShortestPath();
        UpdateTilesWithPath();
        StepThroughPath(tilesArr[end.x,end.y]);
    }

    private void StepThroughPath(GridTileConfig startTile)
    {
        startTile.ShowIsPartOfShortest();
        int dist = startTile.shortestPath;

        GridTileConfig shortestNeighbour = null;
        foreach (GridTileConfig neighbour in startTile.neighbours)
        {
            if(neighbour.shortestPath < dist){
                shortestNeighbour = neighbour;
            }
        }

        if(shortestNeighbour != null){
            StepThroughPath(shortestNeighbour);
        }
        else {
            Debug.Log("StepThroughPath complete!");
        }
        

    }

    private void UpdateTilesWithPath()
    {
        int counter = 0;
        for(int z = 0; z < gridHeight; z++){
            for(int x = 0; x < gridWidth; x++){
                tilesArr[x,z].text.text = dijsktra.shortestPath[counter].ToString();
                tilesArr[x,z].shortestPath = dijsktra.shortestPath[counter];
                // bool isPartOfShortest = dijsktra.isPartOfShortestPath[counter];
                // tilesArr[x,z].isPartOfShortest.SetActive(isPartOfShortest);
                // tilesArr[x,z].isPartOfShortestPath = isPartOfShortest;
                counter++;
            }
        }
    }

    int[] shortestPath;

    private void GenerateShortestPath()
    {
        // int[, ] graph = new int[, ] { { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
		// 							{ 4, 0, 8, 0, 0, 0, 0, 11, 0 },
		// 							{ 0, 8, 0, 7, 0, 4, 0, 0, 2 },
		// 							{ 0, 0, 7, 0, 9, 14, 0, 0, 0 },
		// 							{ 0, 0, 0, 9, 0, 10, 0, 0, 0 },
		// 							{ 0, 0, 4, 14, 10, 0, 2, 0, 0 },
		// 							{ 0, 0, 0, 0, 0, 2, 0, 1, 6 },
		// 							{ 8, 11, 0, 0, 0, 0, 1, 0, 7 },
		// 							{ 0, 0, 2, 0, 0, 0, 6, 7, 0 } };
		dijsktra = new Dijsktra();
        dijsktra.V = gridWidth*gridHeight;
		dijsktra.ShortestPathAtIndex(graph,0); // second param is starting node
    }

    private void CreateGraph()
    {
        graph = new int[gridWidth*gridHeight,gridWidth*gridHeight];
        for(int z = 0; z < gridHeight; z++){
            for(int x = 0; x < gridWidth; x++){
                //top left tile
                GridTileConfig gridTile = tilesArr[x,z];
                SetGraphDistances(x-1,z,gridTile);
                SetGraphDistances(x,z-1,gridTile);
                SetGraphDistances(x+1,z,gridTile);
                SetGraphDistances(x,z+1,gridTile);
                //   x
                //  x x
                //   x
                

                int counter = 0;
                foreach (int weight in gridTile.distances){
                    graph[gridTile.index,counter] = gridTile.distances[counter];
                    counter++;
                }
            }
        }
    }

    private void SetGraphDistances(int x, int z, GridTileConfig tile){
        if(x < 0)
            return;
        if(x >= gridWidth) // 16 >= 16
            return;
        if(z < 0)
            return;
        if(z >= gridHeight) // 8 >= 8
            return;
        
        Debug.Log("SetGraphDistances: " +x+" "+z);
        GridTileConfig neighbourTile = tilesArr[x,z];
        Debug.Log(neighbourTile.index + " neighbourTile.index");
        tile.distances[neighbourTile.index] = neighbourTile.weight; /// 0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,2
        tile.neighbours.Add(neighbourTile);
    }



    private void CreateGrid()
    {
        tilesArr = new GridTileConfig[gridWidth,gridHeight];
        Debug.Log(gridWidth + " " + gridHeight);
        int counter = 0;
        for(int z = 0; z < gridHeight; z++){
            for(int x = 0; x < gridWidth; x++){
                Vector3 pos = new Vector3(x * 1, 0, z * 1);
                GameObject newTile = GameObject.Instantiate(tilePrefab,pos,Quaternion.identity,this.transform);
                GridTileConfig config = newTile.GetComponent<GridTileConfig>();
                config.distances = new int[gridWidth*gridHeight]; // create arr of length 128, they have value 0 by default
                config.xpos = x;
                config.zpos = z;
                config.index = counter;
                config.weight = UnityEngine.Random.Range(1,9);
                Material gridMat = newTile.GetComponent<Renderer>().material;
                gridMat.color = new Color((float)config.weight/9f,0,0,1);
                tiles.Add(newTile);
                Debug.Log(x + " " + z);
                tilesArr[x,z] = config;
                counter++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
