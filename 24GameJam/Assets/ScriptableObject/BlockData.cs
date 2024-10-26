using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "ScriptableObjects/BlockData")]
public class BlockData : ScriptableObject
{
    public string blockName;
    public Sprite icon; // �����ڱ����е�ͼ��
    public GameObject blockPrefab; // ����Ԥ����
    public int initialCount; // ��ʼ����
}
