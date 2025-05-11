using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    public Tilemap soilTilemap;     // tilemap base
    public Tilemap farmlandTilemap; // tilemap donde se crea la tierra arada
    public Tilemap wateredTilemap;
    public Tilemap cropTilemap;     // tilemap donde se plantan los cultivos
    public Sprite cropSprite;
    public RuleTile farmlandRT;
    public RuleTile wateredFarmlandRT;
    public Sprite wateredFarmlandSprite;
    public Sprite[] pumpkinSprites;
    public Sprite[] carrotSprites;
    public Sprite[] potatoSprites;
    public Sprite[] tomatoSprites;
    public Sprite[] beanSprites;

    private Tile cropTile;
    private Tile farmlandTile;
    private Dictionary<Vector3Int, CropTileData> cropData = new Dictionary<Vector3Int, CropTileData>();     // valores nulos representan terreno arado

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cropTile = ScriptableObject.CreateInstance<Tile>();
        cropTile.sprite = cropSprite;
        farmlandTile = ScriptableObject.CreateInstance<Tile>();
        TimeManager.OnCycleComplete += GrowPlantedCrops;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //determina si hay un sprite de tierra arada en el tilemap
    private bool IsSoil(Vector3Int pos)
    {
        return cropData.ContainsKey(pos);
    }

    // determina si hay terreno arado y si esta disponible (no hay ningún cultivo)
    private bool IsSoilAvailable(Vector3Int pos)
    {
        return cropData.ContainsKey(pos) && cropData[pos] == null;
    }

    // determina si el terreno arado tiene el sprite de agua sobrepuesto
    public bool IsSoilWatered(Vector3Int pos)
    {
        return wateredTilemap.GetTile(pos) != null;
    }

    public bool IsCropPlanted(Vector3Int pos)
    {
        return cropData.ContainsKey(pos) && cropData[pos] != null;
    }

    // crea terreno arado si es posible e informa si tuvo éxito
    public bool CreateFarmland(Vector3Int pos)
    {
        if (!IsSoil(pos))
        {
            farmlandTilemap.SetTile(pos, farmlandRT);
            cropData.Add(pos, null);
            return true;
        }
        else
        {
            return false;
        }
    }

    // planta un cultivo si es posible
    public bool PlantCrop(Vector3Int pos, CropTileData.CropType cropType)
    {
        if (IsSoilAvailable(pos))
        {
            CropTileData crop = new CropTileData(pos, cropType, pumpkinSprites);
            crop.Watered = IsSoilWatered(pos);
            cropData[pos] = crop;
            cropTilemap.SetTile(pos, cropData[pos].GetTile());

            return true;
        }
        else
        {
            return false;
        }
    }

    // riega el terreno arado si no lo está ya
    public void WaterTile(Vector3Int pos)
    {
        if (IsSoil(pos))
        {
            Tile temp = ScriptableObject.CreateInstance<Tile>();
            temp.sprite = wateredFarmlandSprite;
            wateredTilemap.SetTile(pos, wateredFarmlandRT);

            if (cropData.ContainsKey(pos) && cropData[pos] != null)
            {
                cropData[pos].Watered = true;
            }
        }
    }

    public void HarvestCrop(Vector3Int pos)
    {
        if (IsCropPlanted(pos) && cropData[pos].GrowthStage <= cropData[pos].GrowthSprites.Length - 1)
        {
            InventoryManager.instance.AddItem(InventoryManager.instance.itemList[1]);
            RemoveCrop(pos);
        }
    }
    public void DebugMakeCropGrow(Vector3Int pos)
    {
        if (cropData.ContainsKey(pos) && cropData[pos] != null)
        {
            cropData[pos].GrowCrop(cropTilemap, wateredTilemap);
        }
    }

    public void GrowPlantedCrops()
    {
        foreach (CropTileData crop in cropData.Values)
        {
            if (crop != null)
            {
                Debug.Log("Cycle Complete");
                crop.GrowCrop(cropTilemap, wateredTilemap);
            }
        }
    }

    public bool RemoveCrop(Vector3Int pos)
    {
        if (cropData.ContainsKey(pos) && cropData[pos] != null)
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
        if (IsSoil(pos))
        {
            farmlandTilemap.SetTile(pos, null);
            cropData.Remove(pos);
            if (wateredTilemap.GetTile(pos) != null)
            {
                wateredTilemap.SetTile(pos, null);
            }
            return true;
        }
        else
        {
            return false;
        }
    }


}
