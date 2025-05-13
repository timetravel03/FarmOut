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
        private int growthStage;
        public enum CropType { PUMPKIN, CARROT, TOMATO, BEAN, POTATO}

        public Vector3Int Position;
        public CropType Type;
        public int GrowthStage
        {
            get
            {
                return growthStage;
            }

            set
            {
                growthStage = value;
                CurrentSprite = GrowthSprites[value];
            }
        }
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

        public void GrowCrop(Tilemap cropTilemap, Tilemap waterTilemap)
        {
            if ((GrowthStage < GrowthSprites.Length - 1) && Watered)
            {
                GrowthStage++;
                CurrentSprite = GrowthSprites[GrowthStage];
                cropTilemap.SetTile(Position, GetTile());
                waterTilemap.SetTile(Position, null);
                Watered = false;
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
