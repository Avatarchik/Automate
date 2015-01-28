using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

public static class NotificationTypes {

	public static NotificationAttr GetAttr(NotificationType type)
	{
		return (NotificationAttr)Attribute.GetCustomAttribute(ForValue(type), typeof(NotificationAttr));
	}

	private static MemberInfo ForValue(NotificationType p)
	{
		return typeof(NotificationType).GetField(Enum.GetName(typeof(NotificationType), p));
	}


	public static List<NotificationType> funTypes() {
		return types ("FUN_BONUS");
    }

	public static List<NotificationType> realTypes() {
		return types ("REAL_");	
    }

	private static List<NotificationType> types(string startWith) {
		List<NotificationType> types = new List<NotificationType>(); 
		foreach (NotificationType type in Enum.GetValues(typeof(NotificationType))) {
			string name = Enum.GetName(typeof(NotificationType), type);
			if (name.StartsWith(startWith)) {
				types.Add(type);
			}
        }
		return types;	
    }
}
