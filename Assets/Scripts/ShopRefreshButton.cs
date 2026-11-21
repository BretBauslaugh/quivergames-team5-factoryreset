using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class ShopRefreshButton : MonoBehaviour
{

    [SerializeField] private Resource _playerResource;
    [SerializeField] private Shop _shop;
    [SerializeField] private bool _onCooldown;
    [SerializeField] private ResourceAmount[] _costAmount;
    [Header("FMOD")]
    [SerializeField] private EventReference _resetSFX; 
    [SerializeField] private EventReference _errorSFX; 

    public void TryRefresh()//Complete function
    {
        if (!_onCooldown)
        {
            if (_playerResource.TryGetMissing(_costAmount, out var missing))
            {

                ErrorText.Show($"Not enough {missing.DisplayName}s!");
                PlaySFX(_errorSFX);
            }
            else
            {
                _playerResource.Spend(_costAmount);
                _shop.RefreshShop();
                PlaySFX(_resetSFX);
            }
        }
        else
        {
            ErrorText.Show("Shop refresh on cooldown");
            Debug.Log("Shop Refresh on Cooldown!");
            return;
        }
    }
    public void PlaySFX(EventReference SFX)
    {
        EventInstance instance = RuntimeManager.CreateInstance(SFX);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position)); //added to prevent warnings
        RuntimeManager.PlayOneShot(SFX, transform.position);
    }
}
