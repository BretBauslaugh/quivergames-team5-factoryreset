using System;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour
{
    private ShopButton[] shopButtons;
    public List<Structure> shopItems = new List<Structure>();
    [SerializeField] private Resource PlayerResource;
    [SerializeField] private ResourceType _coin;
    [Header("Shop Pools")]
    public int currentPool;
    [SerializeField] private Structure[] pool0;
    [SerializeField] private Structure[] pool1;
    [SerializeField] private Structure[] pool2;
    [SerializeField] private Structure[] pool3;
    private int _poolsLength;

    private Dictionary<int, Structure[]> shopPools = new Dictionary<int, Structure[]>();
    
    [Header("Freebie Mode")]
    public bool freebie;
    [SerializeField] private Image _refreshShopButtonImage;
    [SerializeField] private Button _refreshShopButtonButton;
    [SerializeField] private GameObject _refreshShopPrice;

    void OnEnable()
    {
        shopButtons = GetComponentsInChildren<ShopButton>();
        InitalizeShopPools();
    }

    private void InitalizeShopPools()
    {
        shopPools[0] = pool0; //adding pools to dictionary
        shopPools[1] = pool1;
        shopPools[2] = pool2;
        shopPools[3] = pool3;
        _poolsLength = pool0.Length + pool1.Length + pool2.Length + pool3.Length;
    }

    private void Start()
    {
        UpdateShopPool();
        RefreshShop();
    }
    
    public void UpdateShopPool()
    {
        foreach (Structure item in shopPools[currentPool])
        {
            shopItems.Add(item);
        }
        UpdateRefreshButtons();
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
        if (currentPool == 0) currentPool = 1;
        UpdateShopPool();
    }

    public void RefreshShop()//Changed to public to gain access from ShopRefreshButton
    {
        if (shopButtons != null)
        {
            Structure[] shopItemsArray = shopItems.ToArray();
            foreach (ShopButton button in shopButtons)
            {
                int item = Random.Range(0, shopItemsArray.Length);
                string name = shopItemsArray[item].DisplayName;
                string description = shopItemsArray[item].Description;
                Sprite icon = shopItemsArray[item].structureIcon;
                int price = GetItemCost(shopItemsArray[item], _coin);
                GameObject prefab = shopItemsArray[item].prefab;
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

    public void PlacedObject(){
        if(currentPool == 0){
            currentPool++;
            UpdateShopPool();
        }
    }
}
