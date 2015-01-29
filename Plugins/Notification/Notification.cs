using UnityEngine;
using System.Collections;
using System;

public class Notification : MonoBehaviour {

	public int id;
	public string title;
	public string message;
	public string tag;
	public DateTime date;
		
	public override string ToString ()
	{
		return string.Format ("[Notification id:{0} title:{1} message:{2} tag:{3} date:{4}]", id, title, message, tag, date);
	}	
}
