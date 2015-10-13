using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserProfile : JsonModel<UserProfile> 
{
	/* API
	 * $_id = isset($_GET['id'])?$_GET['id']:"";     
        $sex = isset($_GET['sex'])?$_GET['sex']:"male";   
        $birthday = isset($_GET['birthday'])?$_GET['birthday']:"";    
        $avatarname = isset($_GET['avatarname'])?$_GET['avatarname']:""; 
        $fullname = isset($_GET['fullname'])?$_GET['fullname']:""; 
        $website = isset($_GET['website'])?$_GET['website']:""; 
        $link = isset($_GET['link'])?$_GET['link']:""; 
        $about = isset($_GET['about'])?$_GET['about']:""; 
        $location = isset($_GET['location'])?$_GET['location']:"";
        $phone = isset($_GET['phone'])?$_GET['phone']:"";
        $picture = isset($_GET['picture'])?$_GET['picture']:"";
	 * 
	 * 
	 * RESPONSE:
	 * {"username":"syuaibi@gmail.com","email":"syuaibi@gmail.com","id":"50b44aa8a99657f820000000",
	 * "avatarname":"suhendra","fullname":"suhendra","sex":"female","website":null,"link":null,
	 * "handphone":"123123","location":"bandung","picture":null,"about":null,"birthday":"27-11-2012","birthday_dd":"27-11-2012","birthday_mm":"","birthday_yy":""}}
	 */ 

	public class Profile
	{
		public string _id { get; set; }
		public string username { get; set; }
		public string email { get; set; }
		public string avatarname { get; set; }
		public string avatargender { get; set; }
		public string bodytype { get; set; }
		public string fullname { get; set; }
		public string sex { get; set; }
		public string website { get; set; }
		public string link { get; set; }
		public string handphone { get; set; }
		public string location { get; set; }
		public string picture { get; set; }
		public string about { get; set; }
		public string birthday { get; set; }
		public string birthday_dd { get; set; }
		public string birthday_mm { get; set; }
		public string birthday_yy { get; set; }
		public string state_of_mind { get; set; }
		public bool artist { get; set; }
	}

	public bool success { get; set; }
	public bool logged_in { get; set; }
	public string message { get; set; }
	public Profile data	 { get; set; }
	
	public UserProfile()
	{
		data = new Profile();
	}
	
	public static string GetProfileRequest(string email)
	{
		string api = string.Format("{0}user/get_profile?email={1}", Api.getApiUrl(), email);
		
		return api;
	}

	public static string GetProfileByIdRequest(string userid)
	{
		string api = string.Format("{0}user/get_profile?id={1}", Api.getApiUrl(), userid);
		
		return api;
	}
	
	public static string ChangeProfileRequest(Profile profile)
	{
		/* _id, sex, birthday, avatarname, fullname, website, link, about, location, phone, picture */
		Dictionary<string, string> param = new Dictionary<string, string>() 
		{
			{ "email", profile.email },
			{ "id", profile._id },
			{ "sex", profile.sex },
			{ "birthday", profile.birthday },
			{ "avatarname", profile.avatarname },
			{ "fullname", profile.fullname },
			{ "website", profile.website },
			{ "link", profile.link },
			{ "about", profile.about },
			{ "location", profile.location },
			{ "phone", profile.handphone },
			{ "picture", profile.picture },
			{ "state_of_mind", profile.state_of_mind },
			{ "bodytype", profile.bodytype }
		};

		return string.Format("{0}user/change_profile?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}

	public static string ChangeUserPassword(string newPassword, Profile profile)
	{
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "password", newPassword },
			{ "id" , profile._id }
		};

		return string.Format("{0}user/change_password?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}

	public static string RegisterProfile(string password, Profile profile)
	{
		/*
		 * Methode : GET
	     * API Registration User
	     * Parameter :
	     * 1. email
	     * 2. password
	     * 3. username
	     * 4. sex
	     * 5. birthday
	     * 6. twitterid
	     * 7. facebookid
	     * 8. avatarname
	     * 9. fullname
	     * 10. website
	     * 11. link
	     * 12. about
	     * 13. location
	     * 14. phone
	     * 15. bodytype
	     * Return JSON
	     */ 
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "email", profile.email },
			{ "password", password },
			{ "username", profile.username },
			{ "sex", profile.sex },
			{ "birthday", profile.birthday },
			{ "twitterid", "" },
			{ "facebookid", "" },
			{ "avatarname", profile.avatarname },
			{ "fullname", profile.fullname },
			{ "website", profile.website },
			{ "link", profile.link },
			{ "about", profile.about },
			{ "location", profile.location },
			{ "phone", profile.handphone },
			{ "bodytype", profile.bodytype },
			{ "state_of_mind", profile.state_of_mind }
		};

		return string.Format("{0}user/register?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}
}
