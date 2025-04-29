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
        public Sprite[] GrowthSprites;
        public Sprite CurrentSprite;

        public CropTileData(Vector3Int pos, CropType crop, Sprite[] sprites)
        {
            Position = pos;
            Type = crop;
            GrowthSprites = sprites;
            GrowthStage = 0;
            Watered = false;
            CurrentSprite = GrowthSprites[GrowthStage];
        }

        public void GrowCrop()
        {
            if (GrowthStage < 3 && Watered)
            {
                GrowthStage++;
                CurrentSprite = GrowthSprites[GrowthStage];
            }
        }

        public Tile GetTile()
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = CurrentSprite;
            //tile.sprite.
            return tile;
        }
    }
}
