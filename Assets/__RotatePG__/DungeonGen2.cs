using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen2 : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 16;
    public int gridHeight = 10;

    public List<GridTileConfig> tiles;

    private List<GridTileConfig> tilesAfterMerge;

    private int[, ] graph;

    private GridTileConfig[,] tilesArr;

    public Vector2Int start;
    public Vector2Int end;
    private Dijsktra dijsktra;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        divisionList.Add(tiles);
        SubDivide(tiles);
        MultipleRandomDivide();
        MergeTiles();
        ReplaceModels();
        //CreateGraph();
        // GenerateShortestPath();
        // UpdateTilesWithPath();
        // StepThroughPath(tilesArr[end.x,end.y]);
    }

    private void ReplaceModels()
    {
        //Start here :)
    }

    private void MergeTiles()
    {
        tilesAfterMerge = new List<GridTileConfig>();
        foreach (List<GridTileConfig> group in divisionList)
        {
            foreach (GridTileConfig tile in group)
            {
                tile.gameObject.SetActive(false);
            }

            TileBounds tileBounds = new TileBounds();
            tileBounds.CalculateTileBounds(group);
            Debug.Log("index x:" +tileBounds.minWidth+" y:"+tileBounds.minHeight);
            GridTileConfig firstTile = tilesArr[tileBounds.minWidth, tileBounds.minHeight];
            firstTile.isPartOfShortestPath = true;
            //firstTile.model.GetComponent<Renderer>().material.color = new Color(1,0,0,1);
            firstTile.gameObject.SetActive(true);
            int width = firstTile.width = 1 + (tileBounds.maxWidth - tileBounds.minWidth);
            int height = firstTile.height = 1 + (tileBounds.maxHeight - tileBounds.minHeight);
            firstTile.text.text = width.ToString() + ":" + height.ToString();
            firstTile.transform.localScale = new Vector3((float)width,1f,(float)height);
            tilesAfterMerge.Add(firstTile);
        }
    }

    private int divisionIndexCount = 0;
    private void SubDivide(List<GridTileConfig> tiles)
    {
        Color divColor1 = new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),1f);
        Color divColor2 = new Color(UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),UnityEngine.Random.Range(0f,1f),1f);
        List<GridTileConfig> tilesToDivide = tiles;
        TileBounds tileBounds = new TileBounds();
        tileBounds.CalculateTileBounds(tilesToDivide);
        Debug.Log("width: " + tileBounds.minWidth + " - " +tileBounds.maxWidth + " height: " + tileBounds.minHeight + " - " +tileBounds.maxHeight);
        List<GridTileConfig> group1 = new List<GridTileConfig>();
        List<GridTileConfig> group2 = new List<GridTileConfig>();
        bool xSideBiggest = tileBounds.maxWidth - tileBounds.minWidth > tileBounds.maxHeight - tileBounds.minHeight;
        if(tileBounds.maxWidth - tileBounds.minWidth == tileBounds.maxHeight - tileBounds.minHeight){
            xSideBiggest = UnityEngine.Random.Range(0f,1f) > 0.5f ? true : false;
        }
        if(xSideBiggest){
            int cutOff = tileBounds.minWidth + (tileBounds.maxWidth - tileBounds.minWidth)/2;
            Debug.Log("maxWidth > maxHeight , cutoff " +cutOff);
            foreach (GridTileConfig tile in tilesToDivide){
                if(tile.xpos <= cutOff){
                    tile.divisionIndex = divisionIndexCount + 1;
                    tile.model.GetComponent<Renderer>().material.color = divColor1;
                    group1.Add(tile);
                }
                else {
                    tile.model.GetComponent<Renderer>().material.color = divColor2;
                    tile.divisionIndex = divisionIndexCount + 2;
                    group2.Add(tile);
                }
            }
        }
        else {
            int cutOff = tileBounds.minHeight + (tileBounds.maxHeight - tileBounds.minHeight)/2;
            Debug.Log("maxWidth < maxHeight , cutoff " +cutOff);
            foreach (GridTileConfig tile in tilesToDivide){
                if(tile.zpos <= cutOff){
                    tile.divisionIndex = divisionIndexCount + 1;
                    tile.model.GetComponent<Renderer>().material.color = divColor1;
                    group1.Add(tile);
                }
                else {
                    tile.model.GetComponent<Renderer>().material.color = divColor2;
                    tile.divisionIndex = divisionIndexCount + 2;
                    group2.Add(tile);
                }
            }
        }
        divisionList.Add(group1);
        divisionList.Add(group2);
        divisionIndexCount+=2;

        if(divisionIndexCount < 30){
            CheckDivisionWithLowestNumber();
        }
    }

    public void CheckDivisionWithLowestNumber()
    {
        divisionList.RemoveAt(0);
        SubDivide(divisionList[0]);
    }

    public void RandomlyDivide(){
        Debug.Log("RandomlyDivide");
        int listIndex = UnityEngine.Random.Range(0,divisionList.Count-1);
        List<GridTileConfig> randomList = divisionList[listIndex];
        if(randomList.Count < 2){
            RandomlyDivide();
            return;
        }
        divisionList.RemoveAt(listIndex);
        SubDivide(randomList);
    }

    public void MultipleRandomDivide()
    {
        for(int i = 0; i < randomDivideAmount;i++){
            RandomlyDivide();
        }
        
    }

    private List<List<GridTileConfig>> divisionList = new List<List<GridTileConfig>>();



    private List<GridTileConfig> GetTilesWithIndex(int index)
    {
        List<GridTileConfig> tilesToDivide = new List<GridTileConfig>();
        foreach (GridTileConfig tile in tiles)
        {
            if(tile.divisionIndex == index){
                tilesToDivide.Add(tile);
            }
        }

        return tilesToDivide;
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
    public int randomDivideAmount;

    private void GenerateShortestPath()
    {
		dijsktra = new Dijsktra();
        dijsktra.V = gridWidth*gridHeight;
		dijsktra.ShortestPathAtIndex(graph,0); // second param is starting node
    }



    private void CreateGraph()
    {
        foreach (GridTileConfig tile in tilesAfterMerge){
            
        }


        return;
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
                config.divisionIndex = 0;
                config.weight = UnityEngine.Random.Range(1,9);
                Material gridMat = config.model.GetComponent<Renderer>().material;
                gridMat.color = new Color((float)config.weight/9f,0,0,1);
                tiles.Add(config);
                // Debug.Log(x + " " + z);
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