using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDescriptionObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string moduleName, moduleDescription;
    protected GameObject textDescriptionBox;

    void Start()
    {
        GameObject[] textBoxes = GameObject.FindGameObjectsWithTag("ModuleInfoUI");
        if(textBoxes[0] != null){
            textDescriptionBox = textBoxes[0];
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(textDescriptionBox!=null){
            textDescriptionBox.GetComponent<Hovertext>().SetDescriptionText(moduleDescription);
            textDescriptionBox.GetComponent<Hovertext>().SetNameText(moduleName);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(textDescriptionBox!=null){
            textDescriptionBox.GetComponent<Hovertext>().SetDescriptionText("");
            textDescriptionBox.GetComponent<Hovertext>().SetNameText("");
        }
    }

    public void SetInfo(string s1, string s2){
        moduleName = s1;
        moduleDescription = s2;
    }


}
