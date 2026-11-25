using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public LayerMask placeableMask;
    public PlaceObject placeObject;
    public QuotaManager quotaM;
    Camera cam;
    BuildingInstance hovered, selected;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;
        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        BuildingInstance hit = null;

        if(!placeObject.IsPlacing && !overUI)
        {
            Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit info, 500f, placeableMask))
            {
                hit = info.collider.GetComponentInParent<BuildingInstance>();
            }
        }

        if(hit != hovered)
        {
            if (hovered != null) hovered.SetHovered(false);
            hovered = hit;
            if (hovered != null) hovered.SetHovered(true);
        }

        if(mouse.leftButton.wasPressedThisFrame && !overUI)
        {
            if (selected != null) selected.SetSelected(false);
            selected = hovered;
            if (selected != null) selected.SetSelected(true);
        }
    }

    public void ClickSelect()
    {
        if (selected != null) selected.SetSelected(false);
        selected = hovered;
        if (selected != null)
        {
            selected.SetSelected(true);
            //selected.OnClicked();

            //if(selected.TryGetComponent(out Generator gen)) //Click to Collect
            //{
            //    gen.Collect();
            //}
        }
    }

    public void RemoveSelected()    //delete function
    {
        if (selected == null) return;
        if (placeObject.IsPlacing) return;
        if (hovered == selected) hovered = null;
        //Sell items for half price back
        SendCoins(selected.GetPrice()/2);
        Destroy(selected.gameObject);
        selected = null;
    }

    public void SendCoins(int i){
        Debug.Log("Selling for:" + i);
        GameObject[] quotaM = GameObject.FindGameObjectsWithTag("QuotaManager");
        if(quotaM[0]!=null){
            quotaM[0].GetComponent<QuotaManager>().AddCoins(i);
        }
    }
}
