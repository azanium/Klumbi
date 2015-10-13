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

		var genderUrl = Gender.GetGenderApiV2(userid);

		NetworkController.DownloadFromUrl(genderUrl, OnGetGenderDownloaded);
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
			_playerGender = genderObj.gender;
			var gender = genderObj.GetGender();

			Debug.Log("Gender detected: " + gender);

			switch (gender)
			{
			case Gender.GenderType.Male:
				maleRadioButton.value = true;
				break;
				
			case Gender.GenderType.Female:
				femaleRadioButton.value = true;
				break;
			}
			currentGender = genderObj;
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
		
		//ar email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		//var oldUrl = AvatarConfig.GetDefaultAvatarConfigByGenderApi(email, "male");

		var data = GameData.Load();
		var userid = data.profile._id;

		_playerGender = "male";
		var url = AvatarConfig.GetDefaultAvatarConfigApiV2(userid, data.profile.bodytype, "male");

		NetworkController.DownloadFromUrl(url, (www) => {
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
		
		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		//var oldUrl = AvatarConfig.GetDefaultAvatarConfigByGenderApi(email, "female");

		var data = GameData.Load();
		var userid = data.profile._id;

		_playerGender = "female";
		var url = AvatarConfig.GetDefaultAvatarConfigApiV2(userid, data.profile.bodytype, "female");

		NetworkController.DownloadFromUrl(url, (www) =>
		                                  {
			var genderSetObj = AvatarConfig.CreateObject(www.text);
			if (genderSetObj != null)
			{
				//var config = "[{'tipe':'Skin','color':'1'},{'tipe':'Hand','element':'male_hand','material':'m_m_to_tshirt_green01','id':'53e8bf4609bf10a021000000'},{'tipe':'Gender','element':'male_base','material':'','id':'53e8c0b509bf10b81a000000'},{'tipe':'Hair','element':'male_c_ha_hair04','material':'m_ha_hair04','id':'53e8ce5209bf101423000000'},{'tipe':'Body','element':'male_c_m_m_to_tshirt01','material':'m_m_to_tshirt_green01','id':'53e8d02909bf101423000002'},{'tipe':'Pants','element':'male_c_m_l_bo_pantscasualslim01','material':'m_m_bo_pantscasualslim_blue01','id':'53e8aeda09bf108c20000005'},{'tipe':'Shoes','element':'male_c_m_fo_sneakersconverse01','material':'m_m_fo_sneakersconverse_black01','id':'53e8aa0009bf10bc1d000000'},{'tipe':'Face','element':'male_head','material':'','id':'53e8c51109bf10b807000004','eye_brows':'brows_11_01','id_EyeBrows':'53e8cd4b09bf10f013000004','eyes':'eyes_b_06_02','id_Eyes':'53e8cb7309bf10f013000000','lip':'lips_b_06_01','id_Lip':'53e8cc2b09bf10f013000002'}]";
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
		
		var data = GameData.Load();
		data.profile.sex = _playerGender;

		var url = GenderSet.SetGenderApi(data.profile._id, _playerGender);// UserProfile.ChangeProfileRequest(data.profile);

		NetworkController.DownloadFromUrl(url, (www) => 	{
			Debug.Log("GenderSet: " + www.text);

			var gender = GenderSet.CreateObject(www.text);
			if (gender != null)
			{
				UINavigationController.Popup("/NotificationPanel?notification=Gender Saved!", 3);
				UINavigationController.DismissController();
			}
			else
			{
				UINavigationController.Popup("/NotificationPanel?notification=Failed to save Gender!", 3);
			}
		});
		
	}
	
	#endregion
}
