using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    public Tilemap soilTilemap;     // tilemap base
    public Tilemap farmlandTilemap; // tilemap donde se crea la tierra arada
    public Tilemap cropTilemap;     // tilemap donde se plantan los cultivos
    public Sprite cropSprite;
    public Sprite farmlandSprite;

    private Tile cropTile;
    private Tile farmlandTile;
    private Dictionary<Vector3Int, CropTileData> cropData = new Dictionary<Vector3Int, CropTileData>();     // valores nulos representan terreno arado

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cropTile = ScriptableObject.CreateInstance<Tile>();
        //cropTile.sprite = cropSprite;

        farmlandTile = ScriptableObject.CreateInstance<Tile>();
        farmlandTile.sprite = farmlandSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    private bool IsSoilAvailable(Vector3Int pos)
    {
        return cropData.ContainsKey(pos) && cropData[pos] == null;
    }

    private bool IsSoil(Vector3Int pos)
    {
        return cropData.ContainsKey(pos);
    }

    public bool CreateFarmland(Vector3Int pos)
    {
        if (IsSoil(pos))
        {
            farmlandTilemap.SetTile(pos, farmlandTile);
            cropData.Add(pos, null);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlantCrop(Vector3Int pos, CropTileData crop)
    {
        // x ahora así pero tendra su funcion propia supongo
        if (!IsSoilAvailable(pos))
        {
            cropTilemap.SetTile(pos, cropTile);
            cropData[pos] = crop;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveCrop(Vector3Int pos)
    {
        if (cropTilemap.GetTile(pos) != null)
        {
            cropTilemap.SetTile(pos, null);
            cropData[pos] = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveFarmland(Vector3Int pos)
    {
        if (farmlandTilemap.GetTile(pos) != null)
        {
            farmlandTilemap.SetTile(pos, null);
            cropData.Remove(pos);
            return true;
        }
        else
        {
            return false;
        }
    }


}
