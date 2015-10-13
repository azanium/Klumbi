using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using PB.Client;

/*
 * API for Get List Banner
 * Methode: GET
 * Urutan Parameter
 * 1. Start Page from
 * 2. Limit Data
 * Example : [server host]/[server path]/api/mobile/banner/list/0/10/
 * Return : JSON
 */

public class Banners : JsonModel<Banners>
{
    public class Banner
    {
		public string _id { get; set; }
        public string ID { get; set; }
        public string name { get; set; }
		public string tags { get; set; }
        public string Descriptions { get; set; }
		public bool textcolor { get; set; }	
        public string type { get; set; }
		public string date { get; set; }
        public string url_picture { get; set; }
        public string dataValue { get; set; }
        public string picture { get; set; }
		public string brand_id { get; set; }
    }

	public bool success { get; set; }
	public bool logged_in { get; set; }
    public int count { get; set; }
    public List<Banner> data { get; set; }

    public static string GetBannersRequest(int startIndex, int limitData)
    {
		string api = string.Format("{0}banner/list_data/{1}/{2}", Api.getApiUrl(), startIndex, limitData);

        return api;
    }

    public static string GetBannersImagePath(string image)
    {
        return string.Format("{0}bundles/preview_images/{1}", PopBloopSettings.WebServerUrl, image);
    }

}
