﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateAvatarController : UIViewController 
{
	#region MemVars & Props

	public GameObject view1;
	public GameObject view2;

	public UIViewController homeViewController;

	#endregion


	#region Mono Methods

	public override void OnAppeared()
	{
		base.OnAppeared();

		view2.SetActive(false);

		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Center);
		//StartCoroutine(LoadAvatar());

		var data = GameData.Load();
		data.profile.bodytype = "medium";
		var password = data.password;

		NetworkController.DownloadFromUrl(UserProfile.RegisterProfile(password, data.profile), (www) => {

			UserProfile userProfile = UserProfile.CreateObject(www.text);
			if (userProfile.success == false)
			{
				UINavigationController.Popup("/NotificationPanel?notification="+userProfile.message, 3);
				UINavigationController.DismissController();
				UINavigationController.DismissController();
			}
			else
			{
				Debug.Log("Register Profile: " + userProfile.success.ToString() + ", id: " + userProfile.data._id);
				DressRoom.ShowCharacter(userProfile.data._id);

				var gameData = GameData.Load();
				gameData.profile = userProfile.data;
				GameData.Save();

				// Inform the settings, that we are registered now
				Konstants.GetSettingsProfile().SetBool(Konstants.kdUserRegistered, true);
				Konstants.GetSettingsProfile().SetString(Konstants.kdUserId, userProfile.data._id);
				Konstants.GetSettingsProfile().SetString(Konstants.kdEmail, data.profile.email);

				view1.SetActive(false);
				view2.SetActive(true);
			}
		});
	}
	
	#endregion


	#region Internal Methods

	private IEnumerator LoadAvatar()
	{
		yield return new WaitForSeconds(2);

		view1.SetActive(false);
		view2.SetActive(true);
	}

	#endregion


	#region Public Methods

	public void OnContinue()
	{
		Debug.Log("Continue");

		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);

		UINavigationController.PushController("/HomePage?page=1");
		//UINavigationController.PushController(homeViewController);
	}
	
	#endregion
}
