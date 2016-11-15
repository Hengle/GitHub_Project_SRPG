using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{

    public Image mask;
    private RectTransform maskRect;

    public float maxHP;
    private float currentHP;
    private float maxHpBarWidth;

    // Use this for initialization
    void Start()
    {
        maskRect = mask.GetComponent<RectTransform>();
        maxHpBarWidth = maskRect.sizeDelta.x;
        currentHP = maxHP;
    }

    public void HpUP()
    {
        currentHP += 10;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        float deltaSize = currentHP / maxHP;

        maskRect.sizeDelta = new Vector2(maxHpBarWidth * deltaSize, maskRect.sizeDelta.y);
    }

    public void HpDown()
    {
        currentHP -= 10;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        float deltaSize = currentHP / maxHP;

        maskRect.sizeDelta = new Vector2(maxHpBarWidth * deltaSize, maskRect.sizeDelta.y);
    }
}