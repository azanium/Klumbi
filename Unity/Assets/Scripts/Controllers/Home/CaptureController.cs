using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaptureController : UIViewController 
{
	#region MemVars & Props

	public UIToggle showHideToggle;
	public GameObject avatarObject;
	public GameObject starsObject;
	public GameObject diamondsObject;
	public GameObject detailPanel;
	public GameObject capturePanel;
	public GameObject mainMenuPanel;

	public UIToggle showNamePictureToggle;
	public UIToggle showStarToggle;
	public UIToggle showDiamondToggle;

	public UITexture previewTexture;

	protected Texture2D activeScreenshot;

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		UINavigationBar.Show(false);

		DressRoom.ShowCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Center);

		previewTexture.gameObject.SetActive(false);

		var paramFunc = GetParameter("f");
		if (paramFunc == "capture") 
		{
			Debug.Log("Capture");
			Capture();
		}
		else
		{
			capturePanel.gameObject.SetActive(false);
			mainMenuPanel.gameObject.SetActive(true);
		}
	}

	public override void OnDissapear ()
	{
		base.OnDissapear ();

		previewTexture.gameObject.SetActive(false);
	}

	public override void OnAppeared ()
	{
		base.OnAppeared ();

		if (showNamePictureToggle != null)
		{
			showNamePictureToggle.value = true;
		}

		if (showStarToggle != null)
		{
			showStarToggle.value = true;
		}

		if (showDiamondToggle != null)
		{
			showDiamondToggle.value = true;
		}
	}

	#endregion


	#region Internal Methods

	void Capture()
	{
		detailPanel.gameObject.SetActive(false);
		mainMenuPanel.gameObject.SetActive(false);
		//capturePanel.gameObject.SetActive(true);

		StartCoroutine(Shoot());
	}

	protected IEnumerator Shoot()
	{
		if (activeScreenshot != null)
		{
			Destroy(activeScreenshot);
		}
		
		capturePanel.gameObject.SetActive(false);
		
		yield return new WaitForEndOfFrame();
		
		activeScreenshot = new Texture2D(Screen.width, Screen.height);
		activeScreenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		activeScreenshot.Apply();
		
		previewTexture.mainTexture = activeScreenshot;
		previewTexture.gameObject.SetActive(true);

		capturePanel.gameObject.SetActive(true);
		DressRoom.HideCharacter();
		
		//UINavigationController.PushController("/Capture?f=submit");
	}

	#endregion


	#region Public Methods

	#endregion


	#region Events

	public void ToggleShowNamePicture()
	{
		if (showNamePictureToggle != null)
		{
			avatarObject.gameObject.SetActive(showNamePictureToggle.value);
		}
	}

	public void ToggleShowStar()
	{
		if (showStarToggle != null)
		{
			starsObject.gameObject.SetActive(showStarToggle.value);
		}
	}

	public void ToggleShowDiamonds()
	{
		if (showDiamondToggle != null)
		{
			diamondsObject.gameObject.SetActive(showDiamondToggle.value);
		}
	}

	public void ToggleInfo() 
	{
		if (showHideToggle != null && detailPanel != null)
		{
			detailPanel.SetActive(showHideToggle.value);
		}
	}

	#endregion
}
