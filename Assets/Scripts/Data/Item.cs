using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Ttem")]
public class Item : ScriptableObject
{
    public enum ItemType { Hoe, Sword, Pickaxe, WaterCan, PumpkinSeed, Pumpkin, TomatoSeed, Tomato, CarrotSeed, Carrot, Potato, PotatoSeed, Empty }
    public enum ActionType { Plant, Harvest, Attack, Break, Till }

    [Header("Gameplay")]
    public TileBase tile;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}
