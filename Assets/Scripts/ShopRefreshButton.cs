using UnityEngine;

public class ShopRefreshButton : MonoBehaviour
{

    [SerializeField] private Resource _playerResource;
    [SerializeField] private Shop _shop;
    [SerializeField] private bool _onCooldown;
    [SerializeField] private ResourceAmount[] _costAmount;
    public void TryRefresh()//Complete function
    {
        if (!_onCooldown)
        {
            if (_playerResource.TryGetMissing(_costAmount, out var missing))
            {

                ErrorText.Show($"Not enough {missing.DisplayName}");
            }
            else
            {
                _playerResource.Spend(_costAmount);
                _shop.RefreshShop();
            }
        }
        else
        {
            ErrorText.Show("Shop refresh on cooldown");
            Debug.Log("Shop Refresh on Cooldown!");
            return;
        }
    }
}
