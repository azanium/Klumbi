using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Navigate Controller")]
public class UIButtonNavigate : MonoBehaviour
{
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnDoubleClick,
    }

    /// <summary>
    /// Leave it blank if you already set the controller somewhere, it will auto detect
    /// </summary>
    public UINavigationController navigationController;
    public UIViewController viewController;
    public Trigger trigger = Trigger.OnClick;
    public GameObject callbackTarget;
    public string appearMethod;
    public string appearedMethod;
	public string parameter;

    bool mStarted = false;
    bool mHighlighted = false;

    void Start() 
    { 
        mStarted = true;
        if (navigationController == null)
        {
            navigationController = UINavigationController.navigationController;
        }
    }

    void OnEnable() { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

    void OnHover(bool isOver)
    {
        if (enabled)
        {
            if (((isOver && trigger == Trigger.OnMouseOver) ||
                (!isOver && trigger == Trigger.OnMouseOut))) Send();
            mHighlighted = isOver;
        }
    }

    void OnPress(bool isPressed)
    {
        if (enabled)
        {
            if (((isPressed && trigger == Trigger.OnPress) ||
                (!isPressed && trigger == Trigger.OnRelease))) Send();
        }
    }

    void OnClick() { if (enabled && trigger == Trigger.OnClick) Send(); }

    void OnDoubleClick() { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }

    void Update()
    {
        // Keep finding if we failed the first time
        if (navigationController == null)
        {
            navigationController = UINavigationController.navigationController;
        }
    }

    void Send()
    {
        if (viewController == null) viewController = gameObject.GetComponent<UIViewController>();

        if (navigationController != null && viewController != null)
        {
			//Dicti/onary<string, string> param = UINavigationController.GetControllerPathParams(parameter);

			string path = string.Format("{0}{1}", viewController.controllerPath, parameter);

			navigationController.pushController(path, (c) => {
				if (callbackTarget != null && string.IsNullOrEmpty(appearMethod) == false)
				{
					callbackTarget.SendMessage(appearMethod, c, SendMessageOptions.DontRequireReceiver);
				}
			}, (c) => {
				if (callbackTarget != null && string.IsNullOrEmpty(appearedMethod) == false)
				{
					callbackTarget.SendMessage(appearedMethod, c, SendMessageOptions.DontRequireReceiver);
				}
			});
            /*navigationController.pushController(viewController, (c) =>
            {
				c.controllerParameters = param;
                if (callbackTarget != null && string.IsNullOrEmpty(appearMethod) == false)
                {
                    callbackTarget.SendMessage(appearMethod, c, SendMessageOptions.DontRequireReceiver);
                }
            }, (c) =>
            {
				c.controllerParameters = param;
                if (callbackTarget != null && string.IsNullOrEmpty(appearedMethod) == false)
                {
                    callbackTarget.SendMessage(appearedMethod, c, SendMessageOptions.DontRequireReceiver);
                }
            });*/
        }
    }
}