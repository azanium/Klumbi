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

	private string userid = "";

	#endregion


	#region Mono Methods



	public override void OnAppear ()
	{
		base.OnAppear ();
	
		var data = GameData.Load();
		this.userid = data.profile._id;
		
		var iduser = GetParameter("userid");
		if (iduser.Trim() != "")
		{
			this.userid = iduser;
		}
		//ReloadProfile();

		NetworkController.DownloadFromUrl(UserProfile.GetProfileByIdRequest(this.userid), onProfileDownloaded);
	}

	#endregion


	#region Internal Methods

	private void onProfileDownloaded(WWW www)
	{
		UserProfile userProfile = UserProfile.CreateObject(www.text); 

		Debug.LogWarning("EditProfile: " + www.text + ", Error: " + www.error);

		if (userProfile != null)
		{
			ReloadProfile(userProfile);
		}
	}

	private void ReloadProfile(UserProfile userProfile)
	{
		var profile = userProfile.data;//GameData.Load();

		profilePicture.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");
		stateOfMind.value = profile.state_of_mind;
		avatarName.value = profile.avatarname;
		if (profile.fullname != null && profile.fullname != "")
		{
			string[] names = profile.fullname.Split(' ');
			firstName.value = names.Length > 0 ? names[0] : "";
			lastName.value = names.Length > 1 ? names[1] : "";
		}
		aboutMe.value = profile.about;
		birthday.value = profile.birthday_dd;//.birthday;
		birthdayMonth.value = profile.birthday_mm;
		birthdayYear.value = profile.birthday_yy;
		labelPasswordValidation.text = "";
		
		if (profile.sex == "male")
		{
			genderMale.value = true;
		}
		else 
		{
			genderFemale.value = true;
		}
		
		location.value = profile.location;
		mobileNumber.value = profile.handphone;
		email.value = profile.email;
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
