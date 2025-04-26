using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets
{
    public class CropTileData
    {
        public enum CropType { PUMPKIN, CARROT, TOMATO, BEAN, POTATO}
        public Vector3Int Position;
        public CropType Type;
        public int GrowthStage;
        public bool Watered;
        public Sprite[] Sprites;
        public Sprite CurrentSprite;

        public CropTileData(Vector3Int pos, CropType crop)
        {
            Position = pos;
            Type = crop;
            GrowthStage = 0;
            Watered = false;
        }

        public void GrowCrop()
        {
            if (GrowthStage < 3 && Watered)
            {
                GrowthStage++;
                CurrentSprite = Sprites[GrowthStage];
            }
        }

        public Tile GetTile()
        {
            return ScriptableObject.CreateInstance<Tile>();
        }
    }
}
