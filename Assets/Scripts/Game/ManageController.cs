using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageController : MonoBehaviour
{
    [SerializeField] private UniWebView uniWebView;
    [SerializeField] private AppsFlyerObjectScript scriptAndroid;
    [SerializeField] private AppsFlyerObjectScript scriptIOS;
    [SerializeField] private UIController uiController;
    [SerializeField] private MatchThreeController_v2 controller_V2;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        SetActions();
    }

    private void SetActions()
    {
#if UNITY_ANDROID
        scriptAndroid.SetOnSuccessAction(
            () => uniWebView.Show(),
            () => uniWebView.Load(scriptAndroid.neededWebEye)
        );
        controller_V2.SetActionsOnSuccessCombination(
            () => controller_V2.Text.text = controller_V2.GameScore.ToString()
        );
#endif
        
#if UNITY_IOS
        scriptIOS.SetOnSuccessAction(
            () => uniWebView.Show(),
            () => uniWebView.Load(scriptIOS.neededWebEye)
        );
        controller_V2.SetActionsOnSuccessCombination(
            () => controller_V2.Text.text = controller_V2.GameScore.ToString()
        );
#endif

    }

    private void Update()
    {
        text.text = scriptAndroid.resultUserData;
    }
}
