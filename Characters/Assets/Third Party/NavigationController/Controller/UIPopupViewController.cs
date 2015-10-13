using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPopupViewController : UIViewController 
{
	#region MemVars & Props

	public float popupDuration = 3f;

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public IEnumerator DismissAutomatically()
	{
		yield return new WaitForSeconds(popupDuration);

		gameObject.SetActive(false);
	}

	#endregion
}
