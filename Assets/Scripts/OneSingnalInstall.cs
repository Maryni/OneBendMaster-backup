using OneSignalSDK;
using UnityEngine;

public class OneSingnalInstall : MonoBehaviour
{
    [SerializeField] private string appId;

    private void Start()
    {
        OneSignal.Initialize(appId);
    }
}
