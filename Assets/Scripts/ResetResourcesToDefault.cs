using UnityEngine;

public class ResetResourcesToDefault : MonoBehaviour
{
    [SerializeField] private Resource _playerResource;

    private void Start()
    {
        _playerResource.ResetToStart();
    }

}
