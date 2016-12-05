using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text title;
    public Text desciption;

    // ======================= 창 드래그 이동 =======================
    private float offsetX;
    private float offsetY;

    public void BeginDrag()
    {
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }

    public void OnDrag()
    {
        transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y, 0f);
    }
    // ======================= ============== =======================

    public void MessageBoxOpen(string t, string d)
    {
        title.text = t;
        desciption.text = d;
        this.gameObject.SetActive(true);
    }

    public void MessageBoxClose()
    {
        this.gameObject.SetActive(false);
    }
}
