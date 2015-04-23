// 参考
// [Unityで位置情報を取得 - チラ裏Unity](http://hwks.hatenadiary.jp/entry/2014/07/06/175224)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class Gps: MonoBehaviour {

    IEnumerator Start() {
        if (!Input.location.isEnabledByUser) {
            yield break;
        }
        Input.location.Start();
        int maxWait = 120;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1) {
            print("Timed out");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed) {
            print("Unable to determine device location");
            yield break;
        } else {
        	double latitude = Input.location.lastData.latitude;
        	double longitude = Input.location.lastData.longitude;
    		WWW results = new WWW("http://maps.googleapis.com/maps/api/geocode/json?latlng=" + latitude + "," + longitude + "&sensor=true_or_false"); // 逆ジオコーディング

    		yield return results;

    		var search  = Json.Deserialize(results.text) as IDictionary;
            var result = search["results"] as IList;
            var formatted_address  = result[1] as IDictionary;
            
		    GameObject.Find("Text").GetComponent<Text>().text = (string)formatted_address["formatted_address"];
        }
        
        Input.location.Stop();
    }
}