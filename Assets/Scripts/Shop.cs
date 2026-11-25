using System;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopButton[] shopButtons;
    [SerializeField] private Structure[] shopItems;
    [SerializeField] private Resource PlayerResource;
    [SerializeField] private ResourceType _coin;
    [Header("Shop Pools")]
    public int poolKey;
    [SerializeField] private Structure[] pool0;
    [SerializeField] private Structure[] pool1;
    [SerializeField] private Structure[] pool2;
    [SerializeField] private Structure[] pool3;

    private Dictionary<int, Structure[]> shopPools = new Dictionary<int, Structure[]>();
    
    [Header("Freebie Mode")]
    public bool freebie;
    [SerializeField] private Image _refreshShopButtonImage;
    [SerializeField] private Button _refreshShopButtonButton;
    [SerializeField] private GameObject _refreshShopPrice;

    void OnEnable()
    {
        shopButtons = GetComponentsInChildren<ShopButton>();
        shopPools[0] = pool0; //adding pools to dictionary
        shopPools[1] = pool1;
        shopPools[2] = pool2;
        shopPools[3] = pool3;
    }

    private void Start()
    {
        freebie = true;
        UpdateShopPool();
    }
    
    public void UpdateShopPool()
    {
        shopItems = Array.Empty<Structure>();
        shopItems = shopPools[poolKey];
        UpdateRefreshButtons();
        RefreshShop();
    }

    private void UpdateRefreshButtons()
    {
        //hide the refresh buttons for the free shop
        if (freebie)
        {
            _refreshShopPrice.SetActive(false);
            _refreshShopButtonImage.enabled = false;
            _refreshShopButtonButton.enabled = false;
        }
        else
        {
            _refreshShopPrice.SetActive(true);
            _refreshShopButtonImage.enabled = true;
            _refreshShopButtonButton.enabled = true;
        }
    }

    public void TurnOffFreebieMode()
    {
        freebie = false;
        if (poolKey == 0) poolKey = 1;
        UpdateShopPool();
    }

    public void RefreshShop()//Changed to public to gain access from ShopRefreshButton
    {
        // if (freebie && shopButtons != null)
        // {
        // foreach (ShopButton button in shopButtons)
        // {
        //     int randomMax = freeItems.Length + 1;
        //     int item = Random.Range(0, freeItems.Length);
        //     string name = freeItems[item].DisplayName;
        //     string description = freeItems[item].Description;
        //     Sprite icon = freeItems[item].structureIcon;
        //     int price = 0;
        //     GameObject prefab = freeItems[item].prefab;
        //     button.UpdateModule(icon, name, price, prefab, description);
        // }
        // }
        if (shopButtons != null)
        {
            foreach (ShopButton button in shopButtons)
            {
                int item = Random.Range(0, shopItems.Length);
                string name = shopItems[item].DisplayName;
                string description = shopItems[item].Description;
                Sprite icon = shopItems[item].structureIcon;
                int price = GetItemCost(shopItems[item], _coin);
                GameObject prefab = shopItems[item].prefab;
                button.UpdateModule(icon, name, price, prefab, description);
            }
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
}
