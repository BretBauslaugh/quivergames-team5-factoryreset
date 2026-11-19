using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopButton[] shopButtons;
    [SerializeField] private Structure[] shopItems;
    [SerializeField] private Resource PlayerResource;
    [SerializeField] private ResourceType _coin;

    void OnEnable()
    {
        shopButtons = GetComponentsInChildren<ShopButton>();
        RefreshShop();
    }

    public void RefreshShop()//Changed to public to gain access from ShopRefreshButton
    {
        foreach (ShopButton button in shopButtons)
        {
            int randomMax = shopItems.Length + 1;
            int item = Random.Range(0, shopItems.Length);
            string name = shopItems[item].DisplayName;
            string description = shopItems[item].Description;
            Sprite icon = shopItems[item].structureIcon;
            int price = GetItemCost(shopItems[item],_coin);
            GameObject prefab = shopItems[item].prefab;
            button.UpdateModule(icon, name, price, prefab, description);
        }
    }

    private int GetItemCost(Structure struc, ResourceType type)
    {
        foreach (var cost in struc.Cost)
        {
            if (cost.Type == type) return cost.Amount;
        }
        return 0;
    }
        
    //public void RefreshShop()
    //{
    //    for (int button = 0; button < shopButtons.Length; button++)
    //    {
    //        int randomMax = shopItems.Length + 1;
    //        int item = Random.Range(0, randomMax);
    //        Debug.Log($"ID: {item}, Shop Item: {shopItems[item]}");
    //        string name = shopItems[item].name;
    //        Sprite icon = shopItems[item].structureIcon;
    //        int price = 0;
    //        shopButtons[button].UpdateModule(icon, name, price);
    //    }
    }
