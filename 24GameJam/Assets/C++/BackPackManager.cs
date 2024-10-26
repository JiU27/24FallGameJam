using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    public BlockData[] player1Blocks;
    public BlockData[] player2Blocks;
    public GameObject[] player1Slots;
    public GameObject[] player2Slots;
    public Transform player1BuildArea;
    public Transform player2BuildArea;
    public Transform player1SpawnPoint; // 玩家1的生成参考点
    public Transform player2SpawnPoint; // 玩家2的生成参考点

    private GameObject selectedBlockInstance;
    private int selectedSlotPlayer1 = -1;
    private int selectedSlotPlayer2 = -1;

    private int[] player1Counts;
    private int[] player2Counts;

    void Start()
    {
        player1Counts = new int[player1Blocks.Length];
        player2Counts = new int[player2Blocks.Length];
        for (int i = 0; i < player1Blocks.Length; i++) player1Counts[i] = player1Blocks[i].initialCount;
        for (int i = 0; i < player2Blocks.Length; i++) player2Counts[i] = player2Blocks[i].initialCount;

        UpdateBackpackUI(); // 初始化背包界面，显示图标和数量
    }

    void Update()
    {
        HandlePlayerInput();
        HandlePlayerInput();

    if (selectedBlockInstance != null)
    {
        float move = Input.GetAxisRaw("Horizontal") * Time.deltaTime * 5f;
        Vector3 newPosition = selectedBlockInstance.transform.position + Vector3.right * move;

        // 根据玩家不同设置横向移动范围
        if (selectedSlotPlayer1 != -1) // 玩家1的范围
        {
            newPosition.x = Mathf.Clamp(newPosition.x, -8.63f, 0.26f);
        }
        else if (selectedSlotPlayer2 != -1) // 玩家2的范围
        {
            newPosition.x = Mathf.Clamp(newPosition.x, 0.859f, 9.326f);
        }

        selectedBlockInstance.transform.position = newPosition;
    }
    }

    void HandlePlayerInput()
    {
        for (int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) SelectBlock(1, i);
            if (Input.GetKeyDown(KeyCode.Keypad1 + i)) SelectBlock(2, i);
        }

        if (Input.GetKeyDown(KeyCode.S)) PlaceBlock(player1BuildArea);
        if (Input.GetKeyDown(KeyCode.DownArrow)) PlaceBlock(player2BuildArea);

        if (selectedBlockInstance != null)
        {
            float move = Input.GetAxisRaw("Horizontal");
            selectedBlockInstance.transform.position += Vector3.right * move * Time.deltaTime * 5f;
        }
    }

    void SelectBlock(int player, int slotIndex)
    {
        BlockData blockData = player == 1 ? player1Blocks[slotIndex] : player2Blocks[slotIndex];
        int[] counts = player == 1 ? player1Counts : player2Counts;

        if (counts[slotIndex] > 0)
        {
            if (selectedBlockInstance != null)
            {
                Destroy(selectedBlockInstance);
            }

            // 创建新的方块实例
            selectedBlockInstance = Instantiate(blockData.blockPrefab);

            // 使用生成参考点的位置
            Vector3 spawnPosition = player == 1 ? player1SpawnPoint.position : player2SpawnPoint.position;
            selectedBlockInstance.transform.position = spawnPosition;

            // 输出调试信息
            Debug.Log("Expected spawn position (player " + player + "): " + spawnPosition);
            Debug.Log("Actual position of spawn point (player " + player + "): " + (player == 1 ? player1SpawnPoint.position : player2SpawnPoint.position));
            Debug.Log("Position where block was instantiated: " + selectedBlockInstance.transform.position);

            // 设置悬浮状态：不受物理效果影响
            Rigidbody2D rb = selectedBlockInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // 悬浮状态
            }

            // 更新当前选择的格子索引
            if (player == 1) selectedSlotPlayer1 = slotIndex;
            else selectedSlotPlayer2 = slotIndex;

            UpdateSlotColors(player == 1 ? player1Slots : player2Slots, slotIndex);
        }
    }


    void PlaceBlock(Transform buildArea)
    {
        if (selectedBlockInstance != null)
        {
            // 激活物理效果，使方块下坠
            Rigidbody2D rb = selectedBlockInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // 开始受重力影响，下坠
            }

            // 将方块的父级设置为 buildArea
            selectedBlockInstance.transform.SetParent(buildArea);

            // 减少背包计数
            BlockData blockData = buildArea == player1BuildArea ? player1Blocks[selectedSlotPlayer1] : player2Blocks[selectedSlotPlayer2];
            int[] counts = buildArea == player1BuildArea ? player1Counts : player2Counts;
            counts[selectedSlotPlayer1]--; // 更新背包中的数量

            UpdateBackpackUI();

            // 将 selectedBlockInstance 设为 null，不再选择新的方块
            selectedBlockInstance = null;
        }
    }


    void UpdateBackpackUI()
    {
        UpdatePlayerBackpackUI(player1Slots, player1Blocks, player1Counts);
        UpdatePlayerBackpackUI(player2Slots, player2Blocks, player2Counts);
    }

    void UpdatePlayerBackpackUI(GameObject[] slots, BlockData[] blocks, int[] counts)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            slotImage.sprite = blocks[i].icon;

            TextMeshProUGUI quantityText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            quantityText.text = counts[i].ToString();
        }
    }

    void UpdateSlotColors(GameObject[] slots, int selectedSlotIndex)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Image slotImage = slots[i].GetComponent<Image>();
            slotImage.color = (i == selectedSlotIndex) ? Color.yellow : Color.white;
        }
    }

}
