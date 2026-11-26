using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Image moduleIcon;
    public TextMeshProUGUI moduleName;
    public TextMeshProUGUI modulePriceLabel;
    public GameObject modulePrefab;
    public PlaceObject placer;
    public int itemCost;
    private string modDescription;
    private bool itemBought;
    public Sprite lockedIcon;

    private Button _button;
    private EventTrigger _eventTrigger;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _eventTrigger = GetComponent<EventTrigger>();
    }

    public void UpdateModule(Sprite sprite, string name, int price, GameObject prefab, string mDescription)
    {
        moduleIcon.sprite = sprite;
        moduleName.text = name;
        if (price != 0) modulePriceLabel.text = $"{price} Coins";
        else modulePriceLabel.text = "Free";
        itemCost = price;
        modulePrefab = prefab;
        modDescription = mDescription;
        HoverInfo(name, mDescription);
        itemBought = false;
        _button.enabled = true; //button and event trigger get re-enabled so they can actually be clicked after a sold item was there
        _eventTrigger.enabled = true;
        //BuyItem();
    }

    public void SetPlacerPriority()
    { //to talk to the placeobject script, I made a cheap referencing system where PlaceObject has a slot for the shop button to talk to,
        // when this item is selected and can be bought, the shop button will grab itself and place itself in the slot of the placeobject script so PlaceObject..
        // ..can know which prefab to instantiate.
        if (!itemBought) placer.shopButton = this;
    }

    private void HoverInfo(string moduleName, string moduleDescription){
        this.GetComponent<HoverDescriptionObject>().SetInfo(moduleName,moduleDescription);
    }

    //Disables a time slot
    public void BuyItem(){
        itemBought = true;
        _button.enabled = false; //button and event trigger get disabled so SFX and VFX from Button Component cannot be played
        _eventTrigger.enabled = false;
        moduleName.text = "SOLD";
        modulePriceLabel.text = "";
        moduleIcon.sprite = lockedIcon;
        HoverInfo("SOLD", "This item is sold out. Try refreshing the shop");
    }

    //When a player tries to build a thing, they can't if they have already bought it
    public bool GetBought(){
        return itemBought;
    }
}
