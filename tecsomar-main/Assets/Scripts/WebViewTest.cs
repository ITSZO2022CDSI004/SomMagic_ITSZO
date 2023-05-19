using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.WebView;

public class WebViewTest : MonoBehaviour
{
// FullScreen
    public void ShowUrlFullScreen()
    {
        GpmWebView.ShowUrl(
            "https://goo.gl/maps/iivk2yoKzFXg5Xtu8",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                navigationBarColor = "#FF7000",
                title = "Lista SomMagic.",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                supportMultipleWindows = true,
    #if UNITY_IOS
                contentMode = GpmWebViewContentMode.MOBILE
    #endif
            },
            OnCallback,
            new List<string>()
            {
                "USER_ CUSTOM_SCHEME"
            });
    }

    // Popup default
    public void ShowUrlPopupDefault()
    {
        GpmWebView.ShowUrl(
            "https://goo.gl/maps/xdNYBorBKN9KbnBYA",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.PORTRAIT,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = false,
                supportMultipleWindows = true,
    #if UNITY_IOS
                contentMode = GpmWebViewContentMode.MOBILE,
                isMaskViewVisible = true,
    #endif
            },
            OnCallback,
            new List<string>()
            {
                "USER_ CUSTOM_SCHEME"
            });
    }

    // Popup custom position and size
    public void ShowUrlPopupPositionSize()
    {
        GpmWebView.ShowUrl(
            "https://goo.gl/maps/xdNYBorBKN9KbnBYA",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.PORTRAIT,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = false,
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
                supportMultipleWindows = true,
    #if UNITY_IOS
                contentMode = GpmWebViewContentMode.MOBILE,
                isMaskViewVisible = true,
    #endif
            }, null, null);
    }

    // Popup custom margins
    public void ShowUrlPopupMargins()
    {
        GpmWebView.ShowUrl(
            "https://studio.onirix.com/projects/be831c316b1c4bf8bbadf21523f009d6/webar?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjU5MjMsInByb2plY3RJZCI6MTY2MjAsInJvbGUiOjMsImlhdCI6MTYyMzkwNTYzOX0.VHCvELHkwnBM1XXPNhS_qWWDwHdSQX2fD6ZwDDC2VWc",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.PORTRAIT,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = false,
                margins = new GpmWebViewRequest.Margins
                {
                    hasValue = true,
                    left = (int)(Screen.width * 0.1f),
                    top = (int)(Screen.height * 0.1f),
                    right = (int)(Screen.width * 0.1f),
                    bottom = (int)(Screen.height * 0.1f)
                },
                supportMultipleWindows = true,
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
        }
    }
}