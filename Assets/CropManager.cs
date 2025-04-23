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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cropTile = ScriptableObject.CreateInstance<Tile>();
        cropTile.sprite = cropSprite;

        farmlandTile = ScriptableObject.CreateInstance<Tile>();
        farmlandTile.sprite = farmlandSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private bool IsTileAvailable(Vector3Int pos)
    {
        // debe ampliarse para acoger mas casos, como estructuras y otras cosas, de momento solo comprueba si hay tierra en esa posicion
        return farmlandTilemap.GetTile(pos) == null;
    }

    public bool CreateFarmland(Vector3Int pos)
    {
        if (IsTileAvailable(pos))
        {
            farmlandTilemap.SetTile(pos, farmlandTile);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsCropPlanted(Vector3Int pos)
    {
        return cropTilemap.GetTile(pos) != null;
    }

    public bool PlantCrop(Vector3Int pos, CropTileData crop)
    {
        // x ahora así pero tendra su funcion propia supongo
        if (!IsTileAvailable(pos))
        {
            cropTilemap.SetTile(pos, cropTile);
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
            return true;
        }
        else
        {
            return false;
        }
    }


}
