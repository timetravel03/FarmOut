using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets
{
    internal class CropManager : MonoBehaviour
    {
        private Dictionary<Vector3Int, CropTileData> crops = new Dictionary<Vector3Int, CropTileData>();

        public void PlantCrop(Vector3Int pos, CropTileData.CropType crop, Tilemap farmlandTilemap, Tilemap cropTilemap)
        {
            if (!crops.ContainsKey(pos) && farmlandTilemap.GetTile(pos) != null)
            {
                crops.Add(pos, new CropTileData(pos, crop));
            }
        }

        public void WaterCrop(Vector3Int pos)
        {
            if (crops.ContainsKey(pos))
            {
                crops[pos].isWatered = true;
            }
        }
    }
}
