using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    internal class CropTileData
    {
        public enum CropType { PUMPKIN, CARROT, TOMATO, BEAN, POTATO}
        public Vector3Int position;
        public CropType cropType;
        public int growthStage;
        public bool isWatered;

        public CropTileData(Vector3Int pos, CropType crop)
        {
            position = pos;
            cropType = crop;
            growthStage = 0;
            isWatered = false;
        }
    }
}
