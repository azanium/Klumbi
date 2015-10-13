using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GenderController : UIViewController
{
	#region MemVars & Props
	
	public UIToggle maleRadioButton;
	public UIToggle femaleRadioButton;
	private Gender currentGender;
	
	
	#endregion
	
	
	#region Mono Methods
	
	public override void OnAppear()
	{
		base.OnAppear();
		
		DressRoom.ShowCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Center);
		
		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdUserId);
		var data = GameData.Load();
		var userid = data.profile._id;
		
		NetworkController.DownloadFromUrl(Gender.GetGenderApiV2(userid), OnGetGenderDownloaded);
	}
	
	public override void OnDissapear()
	{
		base.OnDissapear();
		
		DressRoom.HideCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);
	}
	
	#endregion
	
	
	#region Internal Methods
	
	private void OnGetGenderDownloaded(WWW www)
	{
		string text = www.text;

		Debug.LogWarning("OnGetGender: " + text);

		maleRadioButton.value = false;
		femaleRadioButton.value = false;
		
		var genderObj = Gender.CreateObject(text);
		if (genderObj != null)
		{
			currentGender = genderObj;
			_playerGender = genderObj.gender;
			
			var gender = genderObj.GetGender();
			switch (gender)
			{
			case Gender.GenderType.Male:
				maleRadioButton.value = true;
				break;
				
			case Gender.GenderType.Female:
				femaleRadioButton.value = true;
				break;
			}
		}
	}
	
	
	#endregion
	
	
	#region Public Methods
	#endregion
	
	
	#region Events
	
	private string _playerGender = "male";
	
	public void OnGenderMale()
	{
		if (currentGender == null)
		{
			return;
		}
		
		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		
		_playerGender = "male";
		NetworkController.DownloadFromUrl(AvatarConfig.GetDefaultAvatarConfigByGenderApi(email, "male"), (www) =>
		                                  {
			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				DressRoom.ChangePlayerCharacter(genderSetObj.configurations);
			}
		});
	}
	
	public void OnGenderFemale()
	{
		if (currentGender == null)
		{
			return;
		}
		
		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		
		_playerGender = "female";
		NetworkController.DownloadFromUrl(AvatarConfig.GetDefaultAvatarConfigByGenderApi(email, "female"), (www) =>
		                                  {
			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				DressRoom.ChangePlayerCharacter(genderSetObj.configurations);
			}
		});
	}
	
	public void OnCancel()
	{
		UINavigationController.DismissController();
	}
	
	public void OnConfirm()
	{
		if (currentGender == null)
		{
			return;
		}
		
		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);

		NetworkController.DownloadFromUrl(GenderSet.GetSetGenderApi(email, _playerGender), (www) => 	{
			UINavigationController.DismissController();
		});
		
	}
	
	#endregion
}
