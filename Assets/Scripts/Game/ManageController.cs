using System;
using System.Collections;
using System.Collections.Generic;
using Gpm.WebView;
using TMPro;
using UnityEngine;

public class ManageController : MonoBehaviour
{
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
        //ShowDefaultUrlFullScreen();
        script.SetOnSuccessAction(
            () => ShowUrlFullScreen(script.neededWebEye)
        );
        controller_V2.SetActionsOnSuccessCombination(
            () => controller_V2.Text.text = controller_V2.GameScore.ToString()
        );
    }

    private void ShowDefaultUrlFullScreen()
    {
        GpmWebView.ShowUrl(
            "https://google.com/",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = false,
                isCloseButtonVisible = false,
                supportMultipleWindows = false,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            },
            // See the end of the code example
            OnCallback,
            new List<string>()
            {
                "USER_ CUSTOM_SCHEME"
            });
    }
    
    public void ShowUrlFullScreen(string url)
    {
    GpmWebView.ShowUrl(
        url,
        new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.FULLSCREEN,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            backgroundColor = "#FFFFFF",
            isNavigationBarVisible = false,
            isBackButtonVisible = false,
            isForwardButtonVisible = false,
            isCloseButtonVisible = false,
            supportMultipleWindows = false,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE
#endif
        },
        // See the end of the code example
        OnCallback,
        new List<string>()
        {
            "USER_ CUSTOM_SCHEME"
        });
}

// Popup default
public void ShowUrlPopupDefault(string url)
{
    GpmWebView.ShowUrl(
        url,
        new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            isNavigationBarVisible = false,
            isCloseButtonVisible = false,
            supportMultipleWindows = false,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
        },
        // See the end of the code example
        OnCallback,
        new List<string>()
        {
            "USER_ CUSTOM_SCHEME"
        });
}

// Popup custom position and size
public void ShowUrlPopupPositionSize(string url)
{
    GpmWebView.ShowUrl(
        url,
        new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            isNavigationBarVisible = false,
            isCloseButtonVisible = false,
            position = new GpmWebViewRequest.Position
            {
                hasValue = true,
                x = (int)(Screen.width * 0.1f),
                y = (int)(Screen.height * 0.1f)
            },
            size = new GpmWebViewRequest.Size
            {
                hasValue = true,
                width = (int)(Screen.width * 0.8f),
                height = (int)(Screen.height * 0.8f)
            },
            supportMultipleWindows = false,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
        }, null, null);
}

// Popup custom margins
public void ShowUrlPopupMargins(string url)
{
    GpmWebView.ShowUrl(
        url,
        new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.UNSPECIFIED,
            isClearCookie = true,
            isClearCache = true,
            isNavigationBarVisible = false,
            isCloseButtonVisible = false,
            margins = new GpmWebViewRequest.Margins
            {
                hasValue = true,
                left = (int)(Screen.width * 0.1f),
                top = (int)(Screen.height * 0.1f),
                right = (int)(Screen.width * 0.1f),
                bottom = (int)(Screen.height * 0.1f)
            },
            supportMultipleWindows = false,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
        }, null, null);
}

private void OnCallback(
    GpmWebViewCallback.CallbackType callbackType,
    string data,
    GpmWebViewError error)
{
    Debug.Log("OnCallback: " + callbackType);
    switch (callbackType)
    {
        case GpmWebViewCallback.CallbackType.Open:
            if (error != null)
            {
                Debug.LogFormat("Fail to open WebView. Error:{0}", error);
            }
            break;
        case GpmWebViewCallback.CallbackType.Close:
            if (error != null)
            {
                Debug.LogFormat("Fail to close WebView. Error:{0}", error);
            }
            break;
        case GpmWebViewCallback.CallbackType.PageStarted:
            if (string.IsNullOrEmpty(data) == false)
            {
                Debug.LogFormat("PageStarted Url : {0}", data);
            }
            break;
        case GpmWebViewCallback.CallbackType.PageLoad:
            if (string.IsNullOrEmpty(data) == false)
            {
                Debug.LogFormat("Loaded Page:{0}", data);
            }
            break;
        case GpmWebViewCallback.CallbackType.MultiWindowOpen:
            Debug.Log("MultiWindowOpen");
            break;
        case GpmWebViewCallback.CallbackType.MultiWindowClose:
            Debug.Log("MultiWindowClose");
            break;
        case GpmWebViewCallback.CallbackType.Scheme:
            if (error == null)
            {
                if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
                {
                    Debug.Log(string.Format("scheme:{0}", data));
                }
            }
            else
            {
                Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
            }
            break;
        case GpmWebViewCallback.CallbackType.GoBack:
            Debug.Log("GoBack");
            break;
        case GpmWebViewCallback.CallbackType.GoForward:
            Debug.Log("GoForward");
            break;
        case GpmWebViewCallback.CallbackType.ExecuteJavascript:
            Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
            break;
#if UNITY_ANDROID
        case GpmWebViewCallback.CallbackType.BackButtonClose:
            Debug.Log("BackButtonClose");
            break;
#endif
    }
}
    
    public void UpdateText()
    {
        text.text += script.BigData;
    }
}
