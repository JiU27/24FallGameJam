using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "ScriptableObjects/BlockData")]
public class BlockData : ScriptableObject
{
    public string blockName;
    public Sprite icon; // 方块在背包中的图标
    public GameObject blockPrefab; // 方块预制体
    public int initialCount; // 初始数量
}
