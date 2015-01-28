using UnityEngine;
using System.Collections;
using System;

public class NotificationAttr : Attribute {

	public int id;
	public string title;
	public string message;
	public long periodSecond;
	public int rangeHourFrom;
	public int rangeHourTo;
	public string tag;

	internal NotificationAttr(int id, string title, string message, long periodSecond, int rangeHourFrom, int rangeHourTo, string tag) 
	{
		this.id = id;
		this.title = title;
		this.message = message;
		this.periodSecond = periodSecond;
		this.rangeHourFrom = rangeHourFrom;
		this.rangeHourTo = rangeHourTo;
		this.tag = tag;
	}
}
