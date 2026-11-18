using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopButton[] shopButtons;
    [SerializeField] private Structure[] shopItems;
    [SerializeField] private Resource PlayerResource;

    void OnEnable()
    {
        shopButtons = GetComponentsInChildren<ShopButton>();
        RefreshShop();
    }

    private void RefreshShop()
    {
        foreach (ShopButton button in shopButtons)
        {
            int randomMax = shopItems.Length + 1;
            int item = Random.Range(0, shopItems.Length);
            string name = shopItems[item].name;
            Sprite icon = shopItems[item].structureIcon;
            int price = 0;
            button.UpdateModule(icon, name, price);
        }
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
