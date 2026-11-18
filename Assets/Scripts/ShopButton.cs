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

    public void UpdateModule(Sprite sprite, string name, int price, GameObject prefab)
    {
        moduleIcon.sprite = sprite;
        moduleName.text = name;
        modulePriceLabel.text = $"{price} Coins";
        modulePrefab = prefab;
    }

    public void SetPlacerPriority()
    {
        placer.shopButton = this;
    }
}
