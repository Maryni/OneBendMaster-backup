using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageController : MonoBehaviour
{
    [SerializeField] private SampleWebView sampleWebView;
    [SerializeField] private AppsFlyerObjectScript script;

    private void Start()
    {
        SetActions();
    }

    private void SetActions()
    {
        script.SetOnSuccessAction(
            () => sampleWebView.gameObject.SetActive(true),
            () => sampleWebView.SetWebEye());
    }
}
