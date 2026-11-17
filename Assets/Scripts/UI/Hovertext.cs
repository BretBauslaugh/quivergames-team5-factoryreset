using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hovertext : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void SetDescriptionText(string s){
        descriptionText.text = s;
    }

    public void SetNameText(string s){
        nameText.text = s;
    }
}
