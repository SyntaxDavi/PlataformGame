using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject InventoryBackground;
    public GameObject PrefabInventoryItem;
    public PlayerController PlayerController;
    public Text GoldCount;
    public int CurrentInventoryGold;

    public List<Weapons> Inventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        SelectWeapon(1);
        RefreshInventory();
    }

    public void Update()
    {
        SelectWeapon(SelectHotkey());
    }

    void RefreshInventory()
    {
        int HotKeyNum = 1;
        int SelectSlot = SelectHotkey();
        GoldCount.text = CurrentInventoryGold.ToString();

        foreach (Transform child in InventoryBackground.transform)
        {
            Destroy(child.gameObject); // Destroy
        }

        foreach (Weapons w in Inventory) // Instantiate
        {
            GameObject SlotInstance = Instantiate(PrefabInventoryItem, InventoryBackground.transform);

            if (w == null)
            {
                SlotInstance.GetComponentInChildren<Image>().enabled = false;
                SlotInstance.GetComponentInChildren<Text>().text = HotKeyNum.ToString();

                SlotInstance.GetComponent<Outline>().enabled = false;
            }
            else
            {
                SlotInstance.GetComponentInChildren<Image>().enabled = true;
                SlotInstance.GetComponentInChildren<Image>().sprite = w.WeaponIcon;
                SlotInstance.GetComponentInChildren<Text>().text = HotKeyNum.ToString();

                SlotInstance.GetComponent<Outline>().enabled = false;
            }

            if (HotKeyNum == SelectSlot || HotKeyNum == 0)
            {
                SlotInstance.GetComponent<Outline>().enabled = true;
            }
            HotKeyNum++;
        }
    }

    public void SelectWeapon(int HotKey)
    {
        if (HotKey > 0 && HotKey <= Inventory.Count)
        {
            Weapons SelectedWeapon = Inventory[HotKey - 1];
            PlayerController.Attack.SetWeapon(SelectedWeapon);
            RefreshInventory();
        }
    }

    public int SelectHotkey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) return 9;
        else { return 0; }
    }

    public void AddGold(int Gold)
    {
        if (Instance != null)
        {
            CurrentInventoryGold += Gold;
            RefreshInventory();
        }
    }
}
