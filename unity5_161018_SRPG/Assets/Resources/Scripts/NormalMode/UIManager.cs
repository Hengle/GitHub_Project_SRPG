using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject skilltree;
    public GameObject status;
    public GameObject setup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InventoryOpen()
    {
        Debug.Log("Inventory Open.");
        inventory.SetActive(true);
    }

    public void InventoryClose()
    {
        Debug.Log("Inventory Close.");
        inventory.SetActive(false);
    }

    public void SkillTreeOpen()
    {
        Debug.Log("SkillTree Open.");
        skilltree.SetActive(true);
    }

    public void SkillTreeClose()
    {
        Debug.Log("SkillTree Close.");
        skilltree.SetActive(false);
    }

    public void StatusOpen()
    {
        Debug.Log("Status Open.");
        status.SetActive(true);
    }

    public void StatusClose()
    {
        Debug.Log("Status Close.");
        status.SetActive(false);
    }

    public void SetUpOpen()
    {
        Debug.Log("Setup Open.");
        setup.SetActive(true);
    }

    public void SetUpClose()
    {
        Debug.Log("Setup Close.");
        setup.SetActive(false);
    }
}