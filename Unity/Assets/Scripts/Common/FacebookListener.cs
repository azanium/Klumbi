using UnityEngine;
using System;
using System.Collections.Generic;
using Prime31;


public class FacebookListener : UIViewController
{

	public static event Action<bool> onLogin;

	#if UNITY_IPHONE || UNITY_ANDROID
	

	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	public override void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.preLoginSucceededEvent += preLoginSucceededEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedWithUrlEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;
		
		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
		
		FacebookManager.shareDialogFailedEvent += shareDialogFailedEvent;
		FacebookManager.shareDialogSucceededEvent += shareDialogSucceededEvent;
	}
	
	
	public override void OnDisable()
	{
		// Remove all the event handlers when disabled
		FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.preLoginSucceededEvent -= preLoginSucceededEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedWithUrlEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;
		
		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
		
		FacebookManager.shareDialogFailedEvent -= shareDialogFailedEvent;
		FacebookManager.shareDialogSucceededEvent -= shareDialogSucceededEvent;
	}
	
	public virtual void preLoginSucceededEvent()
	{
		Debug.Log("PreLogin Succeeded Event");
	}
	
	public virtual void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );

		if (onLogin != null)
		{
			onLogin(true);
		}
	}
	
	
	public virtual void loginFailedEvent( P31Error error )
	{
		Debug.Log( "Facebook login failed: " + error );

		if (onLogin != null)
		{
			onLogin(false);
		}
	}
	
	
	public virtual void dialogCompletedWithUrlEvent( string url )
	{
		Debug.Log( "dialogCompletedWithUrlEvent: " + url );
	}
	
	
	public virtual void dialogFailedEvent( P31Error error )
	{
		Debug.Log( "dialogFailedEvent: " + error );
	}
	
	
	public virtual void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}
	
	
	public virtual void graphRequestCompletedEvent( object obj )
	{
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	
	public virtual void facebookCustomRequestFailed( P31Error error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}
	
	
	public virtual void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}
	
	
	public virtual void reauthorizationSucceededEvent()
	{
		Debug.Log( "reauthorizationSucceededEvent" );
	}
	
	
	public virtual void reauthorizationFailedEvent( P31Error error )
	{
		Debug.Log( "reauthorizationFailedEvent: " + error );
	}
	
	
	public virtual void shareDialogFailedEvent( P31Error error )
	{
		Debug.Log( "shareDialogFailedEvent: " + error );
	}
	
	
	public virtual void shareDialogSucceededEvent( Dictionary<string,object> dict )
	{
		Debug.Log( "shareDialogSucceededEvent" );
		Prime31.Utils.logObject( dict );
	}
	
	#endif
}
