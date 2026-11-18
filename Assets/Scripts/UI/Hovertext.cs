using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hovertext : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void SetDescriptionText(string s){
        ToggleBG(s);
        descriptionText.text = s;
    }

    public void SetNameText(string s){
        ToggleBG(s);
        nameText.text = s;
    }

    public void ToggleBG(string s){
        if(s != ""){
            this.GetComponent<Image>().enabled = true;
        }
        else{
            this.GetComponent<Image>().enabled = false;
        }
    }
}
