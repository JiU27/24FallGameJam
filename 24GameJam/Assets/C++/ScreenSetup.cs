using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    public GameObject player1Area;
    public GameObject player2Area;
    public GameObject centerWall;

    void Start()
    {
        SetupBoundaries();
    }

    private void SetupBoundaries()
    {
        // ��ȡ������ Box Collider
        BoxCollider2D wallCollider = centerWall.GetComponent<BoxCollider2D>();
        if (wallCollider != null)
        {
            wallCollider.isTrigger = false; // ȷ��ǽ�ڲ��ɴ�Խ
        }
    }
}
