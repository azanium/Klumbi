using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class BodyTypeController : UIViewController
{
	#region MemVars & Props
	
	public UIButton skinnyBodyButton;
	public UIButton averageBodyButton;
	public UIButton chubbyBodyButton;
	private Gender currentGender;
	
	#endregion
	
	
	#region Mono Methods
	
	
	#endregion
	
	
	#region UIViewController's Methods
	
	public override void OnAppeared()
	{
		base.OnAppear();
		
		DressRoom.ShowCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Center);

//		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		
		var param = this.controllerParameters;
		if (param.Count == 0 || param.ContainsKey("f") == false)
		{
			return;
		}
		var func = param["f"];
		
		if (string.IsNullOrEmpty(func) == false)
		{
			if (func == "skinny")
			{
				skinnyBodyButton.isEnabled = false;
			}
			else if (func == "average")
			{
				averageBodyButton.isEnabled = false;
			}
			else if (func == "chubby")
			{
				chubbyBodyButton.isEnabled = false;
			}

			Debug.Log("BodyType: " + func);

			var data = GameData.Load();
			var userid = data.profile._id;

			var request = Gender.GetGenderApiV2(userid);
			NetworkController.DownloadFromUrl(request, OnGetGenderDownloaded, null);
		}
	}
	
	public override void OnDissapear()
	{
		base.OnDissapear();
		
		DressRoom.HideCharacter();
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);
	}
	
	#endregion
	
	
	#region Internal Methods
	
	private string _playerBodyType = "";
	
	private void OnGetGenderDownloaded(WWW www)
	{
		string text = www.text;

		Debug.LogWarning("BodyType Gender Downloaded: " + www.text);

		var genderObj = Gender.CreateObject(text);
		if (genderObj != null)
		{
			currentGender = genderObj;
			
			var btype = genderObj.GetBodyType();
			switch (btype)
			{
			case Gender.BodyType.Thin:
				skinnyBodyButton.isEnabled = false;
				_playerBodyType = "small";
				break;
				
			case Gender.BodyType.Medium:
				averageBodyButton.isEnabled = false;
				_playerBodyType = "medium";
				break;
				
			case Gender.BodyType.Fat:
				chubbyBodyButton.isEnabled = false;
				_playerBodyType = "big";
				break;
			}
		}
	}
	
	#endregion
	
	
	#region Public Methods
	#endregion
	
	
	#region Events

	protected void ResetButtons()
	{
		skinnyBodyButton.isEnabled = true;
		averageBodyButton.isEnabled = true;
		chubbyBodyButton.isEnabled = true;
	}

	public void OnBodySkinny()
	{
		if (currentGender == null)
		{
			return;
		}

		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		var data = GameData.Load();
		var userid = data.profile._id;
		
		_playerBodyType = "thin";

		var request = AvatarConfig.GetDefaultAvatarConfigByBodyTypeApiV2(userid, _playerBodyType);
		Debug.LogWarning("Request : " + request);

		NetworkController.DownloadFromUrl(request, (www) => {
			Debug.LogWarning("Change Body Type: " + www.text);

			ResetButtons();
			skinnyBodyButton.isEnabled = false;

			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				DressRoom.ChangePlayerCharacter(genderSetObj.configurations);
			}
		}, null);
	}
	
	public void OnBodyAverage()
	{
		if (currentGender == null)
		{
			return;
		}
		
		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		var data = GameData.Load();
		var userid = data.profile._id;
		
		_playerBodyType = "medium";
		NetworkController.DownloadFromUrl(AvatarConfig.GetDefaultAvatarConfigByBodyTypeApiV2(userid, _playerBodyType), (www) => {
			Debug.LogWarning("Change Body Type: " + www.text);

			ResetButtons();
			averageBodyButton.isEnabled = false;

			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				DressRoom.ChangePlayerCharacter(genderSetObj.configurations);
			}
		}, null);
	}
	
	public void OnBodyChubby()
	{
		if (currentGender == null)
		{
			return;
		}
		
		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		var data = GameData.Load();
		var userid = data.profile._id;
		
		_playerBodyType = "fat";
		NetworkController.DownloadFromUrl(AvatarConfig.GetDefaultAvatarConfigByBodyTypeApiV2(userid, _playerBodyType), (www) => {

			Debug.LogWarning("Change Body Type: " + www.text);

			ResetButtons();
			chubbyBodyButton.isEnabled = false;

			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				DressRoom.ChangePlayerCharacter(genderSetObj.configurations);
			}
		}, null);
	}
	
	public void OnConfirm(GameObject sender)
	{
		if (currentGender == null)
		{
			return;
		}
		
		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		
		NetworkController.DownloadFromUrl(GenderSet.GetSetBodyTypeApi(email, _playerBodyType), (www) =>
		                                  {
			UINavigationController.DismissController();
		}, null);
	}
	
	public void OnCancel(GameObject sender)
	{
		UINavigationController.DismissController();
	}
	
	#endregion
}
