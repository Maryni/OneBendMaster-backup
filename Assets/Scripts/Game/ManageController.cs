using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageController : MonoBehaviour
{
    [SerializeField] private UniWebView uniWebView;
    [SerializeField] private AppsFlyerObjectScript script;
    [SerializeField] private UIController uiController;
    [SerializeField] private MatchThreeController_v2 controller_V2;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        SetActions();
    }

    private void SetActions()
    {
        script.SetOnSuccessAction(
            () => uniWebView.Show(),
            () => uniWebView.Load(script.neededWebEye)
        );
        controller_V2.SetActionsOnSuccessCombination(
            () => controller_V2.Text.text = controller_V2.GameScore.ToString()
        );
    }

    public void UpdateText()
    {
        text.text += script.BigData;
    }
}
