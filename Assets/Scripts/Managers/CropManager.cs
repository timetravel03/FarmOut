using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    //no usar
    public Sprite[] beanSprites;

    private Tile cropTile;
    private Tile farmlandTile;
    private Dictionary<Vector3Int, CropTileData> cropData = new Dictionary<Vector3Int, CropTileData>();     // valores nulos representan terreno arado

    void Start()
    {
        cropTile = ScriptableObject.CreateInstance<Tile>();
        cropTile.sprite = cropSprite;
        farmlandTile = ScriptableObject.CreateInstance<Tile>();
        TimeManager.OnCycleComplete += GrowPlantedCrops;
        DoorScript.SaveEvent += SaveCrops;
        if (GlobalVariables.ResumeGame)
        {
            LoadCrops();
        }
    }

    private void Awake()
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
        if (IsCropPlanted(pos) && cropData[pos].GrowthStage == cropData[pos].GrowthSprites.Length - 1)
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

    //serializa y guarda los datos del diccionario
    private void SaveCrops()
    {
        string cropLine;
        try
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Application.persistentDataPath, "cropdata.fmout"), false))
            {
                foreach (KeyValuePair<Vector3Int, CropTileData> cropPair in cropData)
                {
                    Vector3Int position = cropPair.Key;
                    CropTileData tileData = cropPair.Value;

                    // vector clave
                    if (tileData != null)
                    {
                        // vectores de posicion (redundante pero prueba de concepto),tipo,etapa,regada
                        cropLine = $"{position.ToString()};{tileData.Position.ToString()};{tileData.Type.ToString()};{tileData.GrowthStage.ToString()};{tileData.Watered.ToString()}";
                    }
                    else
                    {
                        cropLine = $"{position.ToString()};NULL";

                    }
                    sw.WriteLine(cropLine);
                }
            }
            Debug.Log($"Cultivos Guardados en {Path.Combine(Application.persistentDataPath, "cropdata.fmout")}");
        }
        catch (System.Exception)
        {
            Debug.Log("Error al guardar los cultivos");
        }
    }

    //carga los datos de los cultivos
    private void LoadCrops()
    {
        string cropLine;
        string[] values;
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.persistentDataPath, "cropdata.fmout")))
            {
                while ((cropLine = sr.ReadLine()) != null)
                {
                    if (cropLine.Trim() != "")
                    {
                        values = cropLine.Split(';');
                        if (values.Length == 2)
                        {
                            cropData.Add(VectorFromString(values[0]), null);
                        }
                        else if (values.Length > 2)
                        {
                            Vector3Int v = VectorFromString(values[0]);
                            CropTileData.CropType type = (CropTileData.CropType)Enum.Parse(typeof(CropTileData.CropType), values[2]);
                            int stage = int.Parse(values[3]);
                            bool watered = bool.Parse(values[4]);
                            Sprite[] sprites = GetCorrectSprites(type);

                            CropTileData data = new CropTileData(v, type, sprites);
                            data.GrowthStage = stage;
                            data.Watered = watered;

                            cropData.Add(v, data);
                        }
                        else
                        {
                            Debug.Log("Error de formato en un cultivo");
                        }
                    }
                }
                Debug.Log("Cultivos cargados correctamente");

                UpdateCropTileMaps();
                Debug.Log("Tilemaps actualizados");
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Error al cargar los cultivos{ex.Message}");
        }
    }

    // convierte un string a un vector3int
    private Vector3Int VectorFromString(string vector)
    {
        Vector3Int parsedVector;
        string[] values;
        string tempString;

        tempString = vector.Substring(1, vector.Length - 2);
        values = tempString.Split(',');
        parsedVector = new Vector3Int(int.Parse(values[0].Trim()), int.Parse(values[1].Trim()), int.Parse(values[2].Trim()));

        return parsedVector;
    }

    // obtiene el sprite correcto en función del tipo
    private Sprite[] GetCorrectSprites(CropTileData.CropType type)
    {
        Sprite[] sp = null;
        switch (type)
        {
            case CropTileData.CropType.PUMPKIN:
                sp = pumpkinSprites;
                break;
            case CropTileData.CropType.CARROT:
                sp = carrotSprites;
                break;
            case CropTileData.CropType.TOMATO:
                sp = tomatoSprites;
                break;
            case CropTileData.CropType.BEAN:
                //no usar
                break;
            case CropTileData.CropType.POTATO:
                sp = potatoSprites;
                break;
        }
        return sp;
    }

    // actualiza todos los tilemaps relevantes
    private void UpdateCropTileMaps()
    {
        try
        {
            foreach (KeyValuePair<Vector3Int, CropTileData> data in cropData)
            {

                farmlandTilemap.SetTile(data.Key, farmlandRT);

                if (data.Value != null)
                {
                    if (data.Value.Watered)
                    {
                        wateredTilemap.SetTile(data.Key, wateredFarmlandRT);
                    }
                    cropTilemap.SetTile(data.Key, data.Value.GetTile());
                }

            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error al actualizar los tilemaps {ex.Message}");
        }

    }
}
