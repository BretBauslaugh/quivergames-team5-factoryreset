using TMPro;
using UnityEngine;
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
    }

    public void SetPlacerPriority()
    {
        placer.shopButton = this;
    }

    private void HoverInfo(string moduleName, string moduleDescription){
        this.GetComponent<HoverDescriptionObject>().SetInfo(moduleName,moduleDescription);
    }
}
