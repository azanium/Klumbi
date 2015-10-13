using UnityEngine;
using System.Collections;

public class CreateAvatarController : UIViewController 
{
	#region MemVars & Props

	private string gender = "male";

	public UIInput avatarNameInput;
	public UIToggle maleRadio;
	public UIToggle femaleRadio;

	public UICenterOnChild centerChild;
	 
	public UIViewController generateAvatarController;
	
	#endregion


	#region Mono Methods

	public override void Start()
	{
		maleRadio.value = true;
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

	public void OnCreateAvatar()
	{
		if (UIHelper.AssertBlankInput(avatarNameInput))
		{
			centerChild.CenterOn(avatarNameInput.transform);
		}
		else
		{
			var data = GameData.Load();
			data.profile.avatarname = avatarNameInput.value;
			data.profile.avatargender = gender;
			GameData.Save();

			UINavigationController.PushController(generateAvatarController);
		}
	}

	#endregion
}
