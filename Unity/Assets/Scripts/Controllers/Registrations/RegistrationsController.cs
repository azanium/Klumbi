using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegistrationsController : UIViewController 
{
	#region MemVars & Props

	public override bool StackPushable 
	{
		get 
		{
			return false;
		}
	}

	public UIViewController startController;
	public UIViewController connectedSocMedController;
	public UIViewController profileRegistrationController;
	public UIViewController avatarHomeController;

	private UIButton[] buttons;

	public bool forceRegistration;

	#endregion


	#region Mono Methods

	public override void Awake ()
	{
		base.Awake ();

		FacebookListener.onLogin += OnFBLoggedIn;
		UITwitter.onLogin += onTwitterLoggedIn;
		UITwitter.onRequestFailed += (error) => { ShowButton(true); };
	}

	public override void Start()
	{
		buttons = GetComponentsInChildren<UIButton>();

		Caching.CleanCache();
		 
		var isRegistered = Konstants.GetSettingsProfile().GetBool(Konstants.kdUserRegistered);
		//var isRegistered = false;

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			if (isRegistered && !forceRegistration)
			{
				ShowStart();
			}
		}
		else
		{
			if (isRegistered)
			{
				ShowStart();
			}

		}

		//DressRoom.ShowCharacter("5339371b85261efb20d63af4");
	}

	public override void OnEnable()
	{
		UINavigationBar.Show(false);


	}

	public override void OnDisable()
	{
	}

	private void ShowButton(bool enabled)
	{
		if (buttons != null)
		{
			foreach (UIButton btn in buttons)
			{
				btn.isEnabled = enabled;
			}
		}
	}

	#endregion


	#region UIViewController's Methods
	
	#endregion


	#region Internal 

	protected void ShowStart()
	{
		UINavigationController.ActivateController(startController, true);
		UINavigationController.ActivateController(this, false);
	}
	
	#endregion


	#region Events

	protected void OnRegisterProfileAppear(UIViewController controller)
	{
	}

	#region FB Request

	protected void OnFBLoggedIn(bool state)
	{
		if (state)
		{
			ShowButton(false);

			UIFacebook.GetEmail((success, email) => {
				if (success)
				{
					// Check if the facebook email already registered to our server
					Api.CallApi(Api.UserApiRequest.GetProfile, string.Format("email={0}", email), (result) => {
						bool isValid = (bool)result["success"];

						DressRoom.ShowInfiniteProgres(false);
						ShowButton(true);

						SetConnection(Konstants.kSocFacebook);

						// User is registered
						if (isValid)
						{

							Debug.Log("User is valid, got profile");

							Prime31.Utils.logObject(result);
							var data = GameData.Load();
							var dict = (Dictionary<string, object>)result["data"];

							Prime31.Utils.logObject(dict);

							data.email = (string)dict["email"];
							data.profile._id = (string)dict["_id"];
							data.profile.username = (string)dict["username"];
							data.profile.avatarname = (string)dict["avatarname"];
							data.profile.avatargender = (string)dict["sex"];
							data.profile.fullname = (string)dict["fullname"];
							data.profile.sex = (string)dict["sex"];
							data.profile.website = (string)dict["website"];
							data.profile.link = (string)dict["link"];
							data.profile.bodytype = (string)dict["bodytype"];
							data.profile.handphone = (string)dict["handphone"];
							data.profile.location = (string)dict["location"];
							data.profile.picture = (string)dict["picture"];
							data.profile.about = (string)dict["about"];
							data.profile.birthday = (string)dict["birthday"];
							data.profile.birthday_dd = (string)dict["birthday_dd"];
							data.profile.birthday_mm = (string)dict["birthday_mm"];
							data.profile.birthday_yy = (string)dict["birthday_yy"];
							GameData.Save();

							Konstants.GetSettingsProfile().SetBool(Konstants.kdUserRegistered, true);
							Konstants.GetSettingsProfile().SetString(Konstants.kdEmail, email);
							Konstants.GetSettingsProfile().SetString(Konstants.kdUserId, data.profile._id);

							// Get Profile
							//UINavigationController.PushController(avatarHomeController);
							UINavigationController.PushController("/HomePage?page=1");
						}
						else
						{
							Debug.Log("User not found: " + email);

							// Store email at our Game Data
							var data = GameData.Load();
							data.email = email;
							GameData.Save();

							UINavigationController.PushController(connectedSocMedController);
						}
					});
				}

			});
		}
	}

	#endregion


	#region Twitter Request

	protected void onTwitterLoggedIn(bool success)
	{
		if (success)
		{
			//UITwitter.loggedinUsername
			Debug.Log("Logged In: " + success + ", " + UITwitter.loggedinUsername);


			UITwitter.onRequestFinished += onTwitterProfileRequestFinished;

			UITwitter.performRequest("get", "/1.1/account/verify_credentials.json", new Dictionary<string, string>()  {
				{ "entities", "false" },
				{ "skip_status", "false" }
			});
		}
		else
		{
			ShowButton(true);
		}
	}

	private void onTwitterProfileRequestFinished(object result)
	{
		UITwitter.onRequestFinished -= onTwitterProfileRequestFinished;

		Debug.Log("OnTwitterProfile Request Finished ");

		Dictionary<string, object> json = (Dictionary<string, object>)result;
		var data = GameData.Load();
		data.email = (string)json["screen_name"]+"@twitter.com";
		data.profile.fullname = (string)json["name"];
		data.profile.location = (string)json["location"];
		data.profile.about = (string)json["description"];
		data.profile.avatargender = "male";
		data.profile.avatarname = (string)json["screen_name"];
		data.profile.bodytype = "medium";
		data.profile.link = (string)json["url"];
		GameData.Save();

		Api.CallApi(Api.UserApiRequest.GetProfile, string.Format("email={0}", data.email), (res) => {
			bool isValid = (bool)res["success"];

			ShowButton(true);

			DressRoom.ShowInfiniteProgres(false);

			SetConnection(Konstants.kSocTwitter);

			if (isValid)
			{
				Debug.Log("User is valid, got profile");
				
				Prime31.Utils.logObject(result);
				data = GameData.Load();
				var dict = (Dictionary<string, object>)res["data"];
				
				Prime31.Utils.logObject(dict);
				
				data.email = (string)dict["email"];
				data.profile._id = (string)dict["_id"];
				data.profile.username = (string)dict["username"];
				data.profile.avatarname = (string)dict["avatarname"];
				data.profile.avatargender = (string)dict["avatargender"];
				data.profile.fullname = (string)dict["fullname"];
				data.profile.sex = (string)dict["sex"];
				data.profile.website = (string)dict["website"];
				data.profile.link = (string)dict["link"];
				data.profile.bodytype = (string)dict["bodytype"];
				data.profile.handphone = (string)dict["handphone"];
				data.profile.location = (string)dict["location"];
				data.profile.picture = (string)dict["picture"];
				data.profile.about = (string)dict["about"];
				data.profile.birthday = (string)dict["birthday"];
				data.profile.birthday_dd = (string)dict["birthday_dd"];
				data.profile.birthday_mm = (string)dict["birthday_mm"];
				data.profile.birthday_yy = (string)dict["birthday_yy"];
				GameData.Save();
				
				Konstants.GetSettingsProfile().SetBool(Konstants.kdUserRegistered, true);
				Konstants.GetSettingsProfile().SetString(Konstants.kdEmail, data.email);
				Konstants.GetSettingsProfile().SetString(Konstants.kdUserId, data.profile._id);
				
				// Get Profile
				//UINavigationController.PushController(avatarHomeController);
				UINavigationController.PushController("/HomePage?page=1");
			}
			else
			{
				Debug.Log("User not found: " + data.email);
				UINavigationController.PushController(connectedSocMedController);
			}
		});

	}

	#endregion

	public void FBLogin()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			OnFBLoggedIn(true);
			return;
		}

		DressRoom.ShowInfiniteProgres(true);

		if (UIFacebook.GetAccessToken() != string.Empty)
		{
			Debug.Log("GetAccessToken() empty: " + UIFacebook.GetAccessToken());
			OnFBLoggedIn(true);
		}
		else
		{
			Debug.Log("Logging in facebook");
			UIFacebook.Login();
		}
	}

	public void TwitterLogin()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			onTwitterLoggedIn(true);
			return;
		}

		DressRoom.ShowInfiniteProgres(true);

		if (UITwitter.isLoggedIn() == false)
		{
			Debug.Log("User is not logged in, logging in to twitter...");
			ShowButton(false);
			UITwitter.login();
		}
		else
		{
			Debug.Log("User is already logged in, checking user...");
			ShowButton(false);
			onTwitterLoggedIn(true);
		}
	}

	public void EmailSignup()
	{
		SetConnection(Konstants.kSocEmail);

		UINavigationController.PushController(profileRegistrationController, (c) => {
			RegistrationProfileController profileController = (RegistrationProfileController)c;
			profileController.isRegisterUsingFacebook = false;

		}, null);
	}

	private void SetConnection(int connectionType)
	{
		Konstants.GetSettingsProfile().SetInt(Konstants.kdSocMedConnection, connectionType);
	}

	public static void PostImage(string message, byte[] images, System.Action<string, object> onCompletion)
	{
#if UNITY_ANDROID
		int connectionType = Konstants.GetSettingsProfile().GetInt(Konstants.kdSocMedConnection);

		if (connectionType == Konstants.kSocFacebook)
		{
			Debug.Log("Post Image via facebook...");
			UIFacebook.postImage(images, message, onCompletion);
		}
		else if (connectionType == Konstants.kSocTwitter)
		{
			Debug.Log("Post Image via twitter.....");
			TwitterAndroid.postStatusUpdate( message, images );
			UITwitter.postImage(images, message, (obj) => {
				if (onCompletion != null)
				{
					onCompletion(null, obj);
				}
			});
		}
#endif
	}

	#endregion
}
