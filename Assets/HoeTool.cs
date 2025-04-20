using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeTool : MonoBehaviour
{
    public TileBase farmLand;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFarmLand(Vector3Int currentFacingTileLocation, Tilemap tilemap)
    {
        if (tilemap.GetTile(currentFacingTileLocation) != farmLand)
        {
            tilemap.SetTile(currentFacingTileLocation, farmLand);
        } else if (tilemap.GetTile(currentFacingTileLocation) == farmLand)
        {
            
        }
    }

    public void DeleteTile(Vector3Int currentFacingTileLocation, Tilemap tilemap)
    {
        if (tilemap.GetTile(currentFacingTileLocation) != null)
        {
            tilemap.SetTile(currentFacingTileLocation, null);
        }
    }
}
