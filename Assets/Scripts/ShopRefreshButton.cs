using UnityEngine;

public class ShopRefreshButton : MonoBehaviour
{

    [SerializeField] private Resource _playerResource;
    [SerializeField] private bool _onCooldown;
    public void TryRefresh()
    {
        if (_onCooldown)
        {
            Debug.Log("Shop Refresh on Cooldown!");
        }
    }
}
