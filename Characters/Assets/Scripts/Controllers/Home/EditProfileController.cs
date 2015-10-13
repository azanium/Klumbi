using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditProfileController : UIViewController 
{
	#region MemVars & Props

	public UIViewController loginController;

	public UITexture profilePicture;
	public UIInput stateOfMind;
	public UIInput avatarName;
	public UIInput firstName;
	public UIInput lastName;
	public UIInput aboutMe;
	public UIInput birthday;
	public UIInput birthdayMonth;
	public UIInput birthdayYear;
	public UIToggle genderMale;
	public UIToggle genderFemale;
	public UIInput location;
	public UIInput mobileNumber;
	public UIInput email;
	public UIInput password;
	public UIInput currentPassword;
	public UIInput confirmPassword;
	public UILabel labelPasswordValidation;

	#endregion


	#region Mono Methods

	private void ReloadProfile()
	{
		var data = GameData.Load();

		profilePicture.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");
		stateOfMind.value = data.profile.state_of_mind;
		avatarName.value = data.profile.avatarname;
		if (data.profile.fullname != null && data.profile.fullname != "")
		{
			string[] names = data.profile.fullname.Split(' ');
			firstName.value = names.Length > 0 ? names[0] : "";
			lastName.value = names.Length > 1 ? names[1] : "";
		}
		aboutMe.value = data.profile.about;
		birthday.value = data.profile.birthday_dd;//.birthday;
		birthdayMonth.value = data.profile.birthday_mm;
		birthdayYear.value = data.profile.birthday_yy;
		labelPasswordValidation.text = "";
		
		if (data.profile.avatargender == "male")
		{
			genderMale.value = true;
		}
		else 
		{
			genderFemale.value = true;
		}
		
		location.value = data.profile.location;
		mobileNumber.value = data.profile.handphone;
		email.value = data.email;
	}

	public override void OnAppear ()
	{
		base.OnAppear ();
	
		ReloadProfile();

		//var data = GameData.Load();

		//NetworkController.DownloadFromUrl(UserProfile.GetProfileRequest(data.email), onProfileDownloaded);
	}

	#endregion


	#region Internal Methods

	private void onProfileDownloaded(WWW www)
	{
		UserProfile profile = UserProfile.CreateObject(www.text); 
		
		var data = GameData.Load();
		data.profile = profile.data;
		GameData.Save();
		
		ReloadProfile();
	}

	#endregion


	#region Public Methods

	public void OnLogout()
	{

		UINavigationController.DismissToFirstController();
		UINavigationController.ActivateController(loginController, true);
	}

	public void UpdateProfile()
	{
		var data = GameData.Load();

		string oldPass = password.value;
		if (oldPass != "")
		{
			Debug.LogWarning(data.password);
			
			if (oldPass != data.password)
			{
				labelPasswordValidation.text = "Invalid password";
				password.isSelected = true;
				
				return;
			}

			string currentPass = currentPassword.value;
			string confirmPass = confirmPassword.value;

			labelPasswordValidation.text = "";
			if (currentPass != confirmPass || currentPass == "")
			{
				labelPasswordValidation.text = "New password is not confirmed!";
				currentPassword.isSelected = true;
				return;
			}
		}

		data.profile.about = aboutMe.value;
		data.profile.avatargender = genderMale.value ? "male" : "female";
		data.profile.avatarname = avatarName.value;
		data.profile.birthday = string.Format("{0}-{1}-{2}", birthdayYear.value, birthdayMonth.value, birthday.value);
		data.profile.email = email.value;
		data.profile.fullname = firstName.value + " " + lastName.value;
		data.profile.handphone = mobileNumber.value;
		data.profile.location = location.value;
		data.profile.sex = data.profile.avatargender;
		data.profile.state_of_mind = stateOfMind.value;	

		var request = UserProfile.ChangeProfileRequest(data.profile);
		Debug.LogWarning("EditProfile Request: " + request);
		NetworkController.DownloadFromUrl(request, onProfileChanged);

	}

	private void onProfileChanged(WWW www)
	{
		Debug.LogWarning(www.text);
		UINavigationController.DismissController();
		
		UINavigationController.Popup("/NotificationPanel?notification=Your profile is updated!", 3);

	}

	#endregion
}
