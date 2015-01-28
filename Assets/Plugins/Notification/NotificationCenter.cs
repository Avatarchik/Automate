using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NotificationCenter {

  private const string SUPER_BONUS = "СУПЕР БОНУС";
  private const string MEGA_BONUS = "МЕГА БОНУС";
  private const int SUPER_BONUS_LVL = 3;
  private const int MEGA_BONUS_LVL = 4;
  private static NotificationCenter instance;
  private System.Random random = new System.Random();

#if UNITY_ANDROID
  AndroidJavaObject notifier;
	#elif UNITY_IPHONE
  IntPtr iOSNotifier;
  #endif

	#if UNITY_IPHONE
  [DllImport("__Internal")]
private static extern IntPtr _NCNotificationPlugin_Init();
  [DllImport("__Internal")]
  private static extern void _NCNotificationPlugin_InitNotifications(IntPtr instance, string jsonNotifications);
  [DllImport("__Internal")]
  private static extern void _NCNotificationPlugin_RemoveNotification(IntPtr instance, int id);
  [DllImport("__Internal")]
  private static extern string _NCNotificationPlugin_GetTag(IntPtr instance);
  [DllImport("__Internal")]
  private static extern string _NCNotificationPlugin_ClearTag(IntPtr instance);
  #endif

  public static NotificationCenter GetInstance() {
    if (instance == null) {
      instance = new NotificationCenter();
    }
    return instance;
  }

  public NotificationCenter () {
    if (isMobileRuntime) {
      Init ();
    }
  }

  private bool isMobileRuntime {
    get {
      return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
  }

  private void Init () {
    if (!isMobileRuntime) {
      return;
    }
		#if UNITY_ANDROID
    notifier = new AndroidJavaObject("com.vclub.notification.NotificationPlugin");
		#elif UNITY_IPHONE
    iOSNotifier = _NCNotificationPlugin_Init();
    #endif

  }

  public string GetTag() {
    string result = "";
    Debug.Log ("Unity: GetTag");
		#if UNITY_ANDROID
    result = notifier.Call<string>("getTag");
		#elif UNITY_IPHONE
    result = _NCNotificationPlugin_GetTag (iOSNotifier);
    #endif
    return result;
  }

  public void ClearTag() {
    Debug.Log ("Unity: ClearTag");
		#if UNITY_ANDROID
    notifier.Call<string>("clearTag");
		#elif UNITY_IPHONE
    _NCNotificationPlugin_ClearTag (iOSNotifier);
    #endif

  }

  public void ScheduleFunCumulative(long timeSecOffset) {
    if (!isMobileRuntime) {
      return;
    }
    Debug.Log ("Unity: ScheduleFunCumulative");
    ArrayList notifications = new ArrayList();
    foreach (NotificationType type in NotificationTypes.funTypes()) {
      Notification notification = PrepareFunNotification(type, timeSecOffset);
      if (isFuture(notification.date)) {
        notifications.Add(notification);
      }
    }
    ScheduleNotifications(notifications);
  }


  public void ScheduleNotifications(ArrayList notifications) {
    if (!isMobileRuntime) {
      return;
    }
    Debug.Log (String.Format("Unity: ScheduleNotifications {0}", notifications));
    string jsonString = "[";
    bool isFirst = true;
    foreach (Notification notification in notifications) {
      long timeSec = ConvertToSec (notification.date);
      string jsonObject = NotificationToJSON(notification.id, notification.title, notification.message, notification.tag, timeSec);
      if (!isFirst) {
        jsonString += ",";
      }
      jsonString += jsonObject;
      isFirst = false;
    }
    jsonString += "]";
		#if UNITY_ANDROID
    notifier.Call("initNotifications", jsonString);
		#elif UNITY_IPHONE
    _NCNotificationPlugin_InitNotifications (iOSNotifier, jsonString);
		#endif

  }

  private string NotificationToJSON(int id, string title, string message, string tag, long timeSec) {
    return string.Format("{{\"id\":{0},\"title\":\"{1}\",\"content\":\"{2}\",\"tag\":\"{3}\",\"timeSec\":{4}}}",id, title, message, tag, timeSec);
  }

  public void ScheduleFunSpecial () {
    Notification notification = PrepareFunNotification(NotificationType.FUN_SPECIAL_OFFER, 0);
    ArrayList notifications = new ArrayList();
    notifications.Add(notification);
    ScheduleNotifications(notifications);
  }

  public void CancelFunSpecial() {
    RemoveNotification(NotificationType.FUN_SPECIAL_OFFER);
  }

  private Notification PrepareFunNotification (NotificationType type, long timeSecOffset) {
    Notification notification = new Notification ();
    NotificationAttr attr = NotificationTypes.GetAttr (type);
    notification.id = attr.id;
    notification.tag = attr.tag;
    notification.date = CalculateFireDate(attr, timeSecOffset);
    notification.title = FunPrefixByLevel (attr.title);
    notification.message = FunPrefixByLevel (attr.message);
    return notification;
  }

  private bool isFuture(DateTime date) {
    return DateTime.Now.CompareTo(date) < 0;
  }

  private static string FunPrefixByLevel(string text) {
    int currentLevel = - 1;
    if (currentLevel <= SUPER_BONUS_LVL) {
      return String.Format(text, SUPER_BONUS);
    } else if (currentLevel == MEGA_BONUS_LVL) {
      return String.Format(text, MEGA_BONUS);
    }
    return "";
  }

  public void RemoveNotification(NotificationType type) {
    if (!isMobileRuntime) {
      return;
    }
    NotificationAttr attr = NotificationTypes.GetAttr (type);
    RemoveNotification (attr.id);
  }


  public void RemoveNotification(int id) {
    if (!isMobileRuntime) {
      return;
    }
    Debug.Log (String.Format("Unity: RemoveNotification with id {0}", id));
		#if UNITY_ANDROID
    notifier.Call("removeNotif", id);
		#elif UNITY_IPHONE
    _NCNotificationPlugin_RemoveNotification (iOSNotifier, id);
		#endif

  }

  private DateTime CalculateFireDate (NotificationAttr attr, long timeSecOffset) {
    DateTime date;
    if (attr.rangeHourFrom == 0 && attr.rangeHourTo == 0) {
      // set date time
      date = DateTime.Now.AddSeconds(attr.periodSecond + timeSecOffset);
    } else {
      // shift date time
      date = DateTime.Now.AddSeconds(attr.periodSecond + timeSecOffset);
      Debug.Log(String.Format("Unity: CalculateFireDate fire date {0} timeSecOffset {1}", date, timeSecOffset));
      if (IsAfter(date, attr.rangeHourFrom) && IsBefore(date, attr.rangeHourTo)) {
        Debug.Log("Unity: CalculateFireDate time in a range");
      } else {
        date = ShiftDate(date, attr);
        Debug.Log(String.Format("Unity: CalculateFireDate time out a range, shift date {0}", date));
      }
    }
    return date;
  }

  private DateTime ShiftDate (DateTime date, NotificationAttr attr) {
    DateTime dateTime = date;
    Debug.Log(String.Format("Unity: ShiftDate {0} range from {1} to {2}", date, attr.rangeHourFrom, attr.rangeHourTo));
    if (IsBefore(date, attr.rangeHourFrom)) {
      Debug.Log(String.Format("Unity: ShiftDate {0} isBefore {1}", date, attr.rangeHourFrom));
      dateTime = ChangeTime(date, attr.rangeHourFrom, 0, 0);
    } else if (IsAfter(date, attr.rangeHourTo)) {
      Debug.Log(String.Format("Unity: ShiftDate {0} isAfter {1}", date, attr.rangeHourTo));
      dateTime = ChangeTime(date, attr.rangeHourFrom, 0, 0);
      dateTime = dateTime.AddDays(1);
    }
    return dateTime;
  }

  private bool IsAfter(DateTime dateTime, int hours) {
    bool result = dateTime.Hour >= hours;
    Debug.Log(String.Format("Unity: IsAfter date time {0} with hours {1} range from {2}, result isAfter {3}",
    dateTime, dateTime.Hour, hours, result));
    return result;
  }

  private bool IsBefore(DateTime dateTime, int hours) {
    bool result;
    if (dateTime.Hour <= hours) {
      if (dateTime.Hour == hours && dateTime.Minute != 0) {
        result = false;
      } else {
        result = true;
      }
    } else {
      result = false;
    }

    Debug.Log(String.Format("Unity: IsBefore date time {0} with hours {1} and minutes {2} range to {3}, result isBefore {4}",
    dateTime, dateTime.Hour, dateTime.Minute, hours, result));
    return result;
  }

  //
  // Util methods
  //

  private long ConvertToSec(DateTime value) {
    long epoch = (value.Ticks - 621355968000000000) / 10000000;
    return epoch;
  }


  private DateTime ChangeTime(DateTime dateTime, int hours, int minutes, int seconds) {
    return new DateTime(
    dateTime.Year,
    dateTime.Month,
    dateTime.Day,
    hours,
    minutes,
    seconds,
    dateTime.Millisecond,
    dateTime.Kind);
  }





  //  public void ScheduleRealWelcome (bool isOn) {
  //    ScheduleRealStaticNotification (NotificationType.WELCOME_BONUS, isOn, 0);
  //  }
  //
  //  public void ScheduleRealMobile (bool isOn) {
  //    ScheduleRealStaticNotification (NotificationType.MOBILE_BONUS, isOn, 0);
  //  }

  //  private void ScheduleRealStaticNotification(NotificationType type, bool isOn, long timeSecOffset) {
  //    RemoveNotification (type);
  //    if (isOn) {
  //      Notification notification = PrepareRealStaticNotification(type, timeSecOffset);
  //      InitNotification(notification);
  //    }
  //  }


  //  private Notification PrepareRealStaticNotification (NotificationType type, long timeSecOffset) {
  //    Notification notification = new Notification ();
  //    NotificationAttr attr = NotificationTypes.GetAttr (type);
  //    notification.id = attr.id;
  //    notification.tag = attr.tag;
  //    notification.date = CalculateFireDate(attr, timeSecOffset);
  //    notification.title = attr.title;
  //    notification.message = attr.message;
  //    return notification;
  //  }

  //	public void ScheduleRealNotifications(List<BaseBonusData> bonuses, List<BaseBonusInfo> bonusInfos) {
  //		if (!isMobileRuntime) {
  //			return;
  //		}
  //		Debug.Log ("Unity: ScheduleRealNotifications");
  //		int bonusIndex = 0;
  //		if (bonuses.Count = 0) {
  //			return;
  //		}
  //
  //		foreach (NotificationType type in NotificationTypes.realTypes()) {
  //			RemoveNotification(type);
  //			if (bonusIndex >= bonuses.Count) {
  //				bonusIndex = 0;
  //			}
  //			Notification notification = PrepareRealNotification(type, bonuses[bonusIndex], bonusInfos);
  //			if (notification != null) {
  //				notification = LvlUpNotification();
  //			}
  //			InitNotification(notification);
  //			bonusIndex++;
  //		}
  //	}
  //
  //	private Notification PrepareRealNotification (NotificationType type, BaseBonusData bonusData, List<BaseBonusInfo> bonusInfos) {
  //		if ("lvlup".Equals (bonusData.Sysname)) {
  //			return LvlUpNotification(type);
  //		}
  //
  //		BaseBonusInfo bonusInfo = findBonusInfo (bonusData.SysName, bonusInfos);
  //		Notification notification = null;
  //        if (bonusInfo != null) {
  //			notification = new Notification ();
  //			NotificationAttr attr = NotificationTypes.GetAttr (type);
  //			notification.id = attr.id;
  //			notification.tag = attr.tag + ":" + bonusData.SysName;
  //			notification.title = String.Format(attr.title, bonusInfo.Title);
  //			string message = bonusInfo.Messages[random.Next(0, bonusInfo.Messages.Count-1)];
  //			notification.message = String.Format(attr.message, message);
  //			notification.date = CalculateFireDate (attr, 0);
  //		}
  //        return notification;
  //	}
  //
  //	private BaseBonusInfo findBonusInfo (string sysName, List<BaseBonusInfo> bonusInfos) {
  //		foreach(BaseBonusInfo bonusInfo in bonusInfos) {
  //			if (sysName.Equals(bonusInfo.SysName)) {
  //				return bonusInfo;
  //			}
  //		}
  //		return null;
  //	}
  //
  //  private Notification LvlUpNotification(NotificationType type) {
  //    Notification notification = new Notification ();
  //    NotificationAttr attr = NotificationTypes.GetAttr (type);
  //    notification.id = attr.id;
  //    notification.tag = attr.tag + ":" + "lvlup";
  //    notification.title = "БОНУС за УРОВЕНЬ!";
  //    notification.message = "Повышайте УРОВЕНЬ и получайте щедрые БОНУСЫ! Заходите";
  //    notification.date = CalculateFireDate (attr, 0);
  //    return notification;
  //  }
  //
  //  public void UnscheduleRealNotifications() {
  //    if (!isMobileRuntime) {
  //      return;
  //    }
  //    Debug.Log ("Unity: UnscheduleRealNotifications");
  //    foreach (NotificationType type in NotificationTypes.realTypes()) {
  //      RemoveNotification(type);
  //    }
  //  }

}
