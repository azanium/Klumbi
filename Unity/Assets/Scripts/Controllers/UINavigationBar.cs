using UnityEngine;
using System.Collections;

public class UINavigationBar : MonoBehaviour 
{
	#region MemVars & Props
	
	private static UINavigationBar navigationBar;

	public bool isVisible = true;
	public UIButton homeButton;
	public UIButton myAvatarButton;
	public UIButton streamButton;
	public UIButton moreButton;

	#endregion


	#region MonoBehaviour's Methods

	protected void Awake()
	{
		navigationBar = this;
	}

	#endregion


	#region Public Methods

	public static void Show(bool visible)
	{
		if (navigationBar != null)
		{
			navigationBar.isVisible = visible;
			navigationBar.gameObject.SetActive(visible);
		} 
	}

	public static void ShowMenus(bool visible)
	{
		if (navigationBar != null)
		{
			navigationBar.homeButton.gameObject.SetActive(visible);
			navigationBar.myAvatarButton.gameObject.SetActive(visible);
			navigationBar.streamButton.gameObject.SetActive(visible);
			//navigationBar.moreButton.gameObject.SetActive(visible);
		}
	}

	#endregion
}
