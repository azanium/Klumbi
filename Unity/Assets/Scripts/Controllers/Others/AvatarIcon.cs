using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarIcon : MonoBehaviour 
{
	#region MemVars & Props

	public UILabel fullNameLabel;
	public UILabel avatarName;
	public UITexture profilePicture;

	#endregion


	#region Mono Methods

	public void OnEnable()
	{
		var data = GameData.Load();

		if (fullNameLabel != null)
		{
			fullNameLabel.text = data.profile.fullname;
		}

		if (avatarName != null)
		{
			avatarName.text = data.profile.avatarname;
		}


	}

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	#endregion
}
