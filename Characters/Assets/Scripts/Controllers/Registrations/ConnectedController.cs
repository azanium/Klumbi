using UnityEngine;
using System.Collections;

public class ConnectedController : UIViewController 
{
	private const string title = "Congratulations!\nYou've successfully\nconnected your {SOCMED} Account to Klumbi.";

	public void OnProfileAppear(UIViewController controller)
	{
		RegistrationProfileController profileController = (RegistrationProfileController)controller;
		profileController.isRegisterUsingFacebook = true;
	}

	public override void OnAppear ()
	{
		base.OnAppear ();

		UILabel label = GetComponentInChildren<UILabel>();

		int conType = Konstants.GetSettingsProfile().GetInt(Konstants.kdSocMedConnection);
		switch (conType)
		{
		case Konstants.kSocFacebook:
		{
			if (label != null)
			{
				label.text = title.Replace("{SOCMED}", "Facebook");
			}
		} break;

		case Konstants.kSocTwitter:
		{
			if (label != null)
			{
				label.text = title.Replace("{SOCMED}", "Twitter");
			}
		} break;

		}
	}
}
