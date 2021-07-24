using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollTest : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GridTileConfig> tiles = new List<GridTileConfig>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiles[0].CheckIntersection(tiles);
    }
}
