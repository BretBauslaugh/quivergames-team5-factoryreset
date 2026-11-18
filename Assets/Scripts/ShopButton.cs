using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Image moduleIcon;
    public TextMeshProUGUI moduleName;
    public TextMeshProUGUI modulePriceLabel;

    public void UpdateModule(Sprite sprite, string name, int price)
    {
        moduleIcon.sprite = sprite;
        moduleName.text = name;
        modulePriceLabel.text = $"{price} Coins";
    }
}
