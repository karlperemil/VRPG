using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridTileConfig : MonoBehaviour
{
    // Start is called before the first frame update

    public int weight = 0;
    public int xpos;
    public int zpos;
    public int index;
    public int[] distances;

    public GameObject model;

    public TextMeshPro text;

    public GameObject isPartOfShortestGO;

    public bool isPartOfShortestPath = false;
    internal int shortestPath;

    public List<GridTileConfig> neighbours = new List<GridTileConfig>();
    public int divisionIndex;
    public int width;
    public int height;

    private void Start() {
    }

    public void ShowIsPartOfShortest(){
        isPartOfShortestGO.SetActive(true);
        isPartOfShortestPath = true;
    }

    public void CheckIntersection(List<GridTileConfig> tiles){
        BoxCollider[] thisTileColliders = model.GetComponents<BoxCollider>();

        foreach (GridTileConfig tile in tiles)
        {
            if(tile != this){
            
                BoxCollider[] otherTileColliders = tile.model.GetComponents<BoxCollider>();
                foreach (BoxCollider thisTileCollider in thisTileColliders)
                {
                    
                    Bounds rendBounds = tile.model.GetComponent<Renderer>().bounds;
                    if(rendBounds.Intersects(thisTileCollider.bounds)){
                        tile.model.GetComponent<Renderer>().material.color = new Color(1,0,0,1);
                    }

                }

            }

        }
    }
}
