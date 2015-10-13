using UnityEngine;
using System.Collections;

public class RegistrationProfileController : UIViewController 
{

	#region MemVars & Props

	public bool isRegisterUsingFacebook = false;

	public UIInput firstNameInput;
	public UIInput lastNameInput;
	public UIInput emailInput;
	public UIInput passwordInput;
	public UIInput locationInput;
	public UIInput birthdayInput;
	public UIInput mobileNumberInput;
	public UI2DSprite profilePicture;
	public UIInput aboutMeInput;
	public UIToggle maleRadio;
	public UIToggle femaleRadio;

	public UICenterOnChild centerChild;

	public UIViewController createAvatarController;


	private string gender = "male";
	
	#endregion


	#region Virtual Methods

	public override void OnAppear()
	{
		base.OnAppear();

		int socType = Konstants.GetSettingsProfile().GetInt(Konstants.kdSocMedConnection);

		switch (socType)
		{
			case Konstants.kSocFacebook:
			{
				UIFacebook.GraphRequest("/me?fields=id,name,email,about,birthday,address", (e, dic) => {
					string name = (string)dic["name"];
					string email = (string)dic["email"];
					string birthday = (string)dic["birthday"];

					string[] names = name.Split(' ');
					if (names.Length > 1)
					{
						firstNameInput.value = names[0];
						string lastName = "";
						for (int i = 1; i < names.Length; i++)
						{
							lastName += names[i] + " ";
						}
						Debug.Log("Last Name: " + lastName);
						lastNameInput.value = lastName;
					}
					else
					{
						firstNameInput.value = name;
					}
					emailInput.value = email;
					birthdayInput.value = birthday;
				});
			}
			break;

		case Konstants.kSocTwitter:
		{
			/*data.email = (string)json["screen_name"]+"@twitter.com";
			data.profile.fullname = (string)json["name"];
			data.profile.location = (string)json["location"];
			data.profile.about = (string)json["description"];
			data.profile.avatargender = "male";
			data.profile.avatarname = (string)json["screen_name"];
			data.profile.bodytype = "medium";
			data.profile.link = (string)json["url"];*/
			var data = GameData.Load();
			string[] fullname = data.profile.fullname.Split(' ');
			firstNameInput.value = fullname.Length > 0 ? fullname[0] : "";
			lastNameInput.value = fullname.Length > 1 ? fullname[1] : "";
			locationInput.value = data.profile.location;
			aboutMeInput.value = data.profile.about;
			emailInput.value = data.email;
		}
		break;
		}
	}

	#endregion


	#region Public Methods

	public void OnGenderChange()
	{
		var toggle = UIToggle.GetActiveToggle(1);

		if (toggle == maleRadio)
		{
			gender = "male";
		}
		else
		{
			gender = "female";
		}
		Debug.Log("Gender Change: " + gender);
	}

	public void OnSignup()
	{
		centerChild.enabled = true;
		if (UIHelper.AssertBlankInput(firstNameInput))
		{

			centerChild.CenterOn(firstNameInput.transform);
		}
		else
		{
			if (UIHelper.AssertBlankInput(lastNameInput))
			{
				centerChild.CenterOn(lastNameInput.transform);
			}
			else
			{
				if (UIHelper.AssertBlankInput(emailInput))
				{
					centerChild.CenterOn(emailInput.transform);
				}
				else
				{
					if (UIHelper.AssertBlankInput(passwordInput))
					{
						centerChild.CenterOn(passwordInput.transform);
					}
					else
					{
						if (UIHelper.AssertBlankInput(locationInput))
						{
							centerChild.CenterOn(locationInput.transform);
						}
						else
						{
							if (UIHelper.AssertBlankInput(birthdayInput))
							{
								centerChild.CenterOn(birthdayInput.transform);
							}
							else
							{
								SignupNow();
							}
						}
					}
				}
			}
		}
		centerChild.enabled = false;

	}

	public void SignupNow()
	{
		var data = GameData.Load();

		data.profile.fullname = string.Format("{0} {1}", firstNameInput.value, lastNameInput.value);
		data.profile.email = emailInput.value;
		data.profile.sex = gender;
		data.profile.location = locationInput.value;
		data.profile.birthday = birthdayInput.value;
		data.profile.handphone = mobileNumberInput.value;
		data.profile.picture = "";
		data.profile.about = aboutMeInput.value;

		data.password = passwordInput.value;

		GameData.Save();

		UINavigationController.PushController(createAvatarController);
	}

	#endregion
}
