using UnityEngine;
using System.Collections;

public class Konstants
{
	public const int kSocEmail = 0;
	public const int kSocFacebook = 1;
	public const int kSocTwitter = 2;

	public const string kSettings = "kSettings";
	public const string kdEmail = "kEmail";
	public const string kFBLoggedIn = "kFBLoggedIn";
	public const string kTwitterLoggedIn = "kTwitterLoggedIn";
	public const string kSocMedLoggedIn = "kSocmedLoggedIn";
	public const string kdUserRegistered = "kdUserRegistered";
	public const string kdUserId = "kduserId";
	public const string kdSocMedConnection = "kdSocMedConnection";

	public static PBDefaults GetSettingsProfile()
	{
		return PBDefaults.GetProfile(kSettings);
	}
}
