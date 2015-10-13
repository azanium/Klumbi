using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateMixController : UIViewController 
{
	#region MemVars & Props

	public GameObject previewMale;
	public GameObject previewFemale;
	public UITexture previewTexture;
	public UIInput mixNameInput;
	public GameObject captureMenu;
	public GameObject bottomMenu;
	public GameObject[] huds;
	public GameObject notification;
	public UILabel successNotificationLabel;
	public UILabel mixNameNotificationLabel;


	private Texture2D activeScreenshot;
	private string avatarGender = "";

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		DressRoom.ShowCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Center);

		captureMenu.gameObject.SetActive(true);
		notification.gameObject.SetActive(false);
		mixNameInput.value = "";

		ShowHUD(false);

//		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		var data = GameData.Load();
		var userid = data.profile._id;

		//string genderApi = Gender.GetGenderApi(email);
		string genderApi = Gender.GetGenderApiV2(userid);

		NetworkController.DownloadFromUrl(genderApi, (www) => {
			Debug.Log("Get Gender: " + www.text);
			Gender gender = Gender.CreateObject(www.text);
			avatarGender = gender.gender;
		});
	}

	public override void Update ()
	{
		base.Update ();

		if (mixNameInput != null && bottomMenu != null)
		{
			if (mixNameInput.value != "" && !bottomMenu.gameObject.activeSelf)
			{
				bottomMenu.gameObject.SetActive(true);
			}
			else if (mixNameInput.value.Trim() == "" && bottomMenu.gameObject.activeSelf)
			{

				bottomMenu.gameObject.SetActive(false);
			}
		}
	}

	#endregion


	#region Internal Methods

	private void ShowHUD(bool visible)
	{
		if (huds != null)
		{
			foreach (GameObject go in huds)
			{
				go.SetActive(visible);
			}
		}
		UINavigationBar.Show(visible);
		UINavigationBar.ShowMenus(false);
	}

	private void SetGender(string gender)
	{
		previewFemale.gameObject.SetActive(false);
		previewMale.gameObject.SetActive(false);

		if (gender == "male")
		{
			previewMale.gameObject.SetActive(true);
		}
		else if (gender == "female")
		{
			previewFemale.gameObject.SetActive(true);
		}
	}

	#endregion


	#region Public Methods

	public void OnCapture()
	{
		StartCoroutine(Shoot());
	}

	protected IEnumerator Shoot()
	{
		if (activeScreenshot != null)
		{
			Destroy(activeScreenshot);
		}
		captureMenu.gameObject.SetActive(false);

		yield return new WaitForEndOfFrame();
		
		activeScreenshot = new Texture2D(Screen.width, Screen.height);
		activeScreenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		activeScreenshot.Apply();

		if (previewTexture != null)
		{
			previewTexture.mainTexture = activeScreenshot;
			previewTexture.gameObject.SetActive(true);
		}

		SetGender(avatarGender);

		DressRoom.HideCharacter();

		ShowHUD(true);
	}

	private IEnumerator ShowNotification(bool success)
	{
		if (notification) 
		{
			notification.gameObject.SetActive(true);
			if (successNotificationLabel != null)
			{
				successNotificationLabel.text = success ? "New mix created" : "Failed to create mix";
			}
			if (mixNameNotificationLabel != null)
			{
				mixNameNotificationLabel.text = "\"" + mixNameInput.value + "\"";
			}
		}
		yield return new WaitForSeconds(3);
		if (notification)
		{
			notification.gameObject.SetActive(false);
		}
	}

	public void OnSubmit()
	{
		if (activeScreenshot == null)
		{
			return;
		}
		
		var bytes = activeScreenshot.EncodeToPNG();
		//Destroy(activeScreenshot);
		//activeScreenshot = null;
		
		var mixName = mixNameInput.value;
		var config = DressRoom.GetCurrentCharacterConfig();

		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		var data = GameData.Load();
		var userid = data.profile._id;
		
		Dictionary<string, object> post = new Dictionary<string, object>();
		post.Add("iduser", userid);
		post.Add("title", mixName);
		post.Add("gender", data.profile.sex);
		post.Add("bodysize", data.profile.bodytype);
		post.Add("fileimage", bytes);
		post.Add("configuration", config);
		
		var api = AvatarMix.CreateMixApiV2();
		Debug.LogWarning("API: " + api);

		NetworkController.DownloadFromUrl(api, post, (www) => {
			Debug.Log("Uploaded with response: " + www.text);

			var mix = AvatarMix.CreateObject(www.text);
			StartCoroutine(ShowNotification(mix.success));
			mixNameInput.value = "";
		});

		/*
		RegistrationsController.PostImage("Testing", bytes, (error, result) => {
			if (error == null)
			{
				Prime31.Utils.logObject(result);
				UINavigationController.Popup("/NotificationPanel?notification=Your profile is updated!", 3);
			}
		});*/
	}
	#endregion
}
