package com.vclub.notification;

import android.app.Activity;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.vclub.SlotActivity;
import com.vclub.notification.db.dto.Notification;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.TimeZone;

/**
 * Created by Max Denissov on 26/11/14.
 */
public class NotificationPlugin {

    private static final String LOG_TAG = "Notification Plugin";
    private NotificationTimerService mNotificationTimerService;

    public NotificationPlugin() {
        final Activity activity = UnityPlayer.currentActivity;
        mNotificationTimerService = new NotificationTimerService(activity);
    }

    public void initNotifications(String json) {
        try {
            JSONArray array = new JSONArray(json);
            initNotifications(array);
        } catch (JSONException e) {
            Log.d(LOG_TAG, MessageFormat.format("Error while parse data from Unity {0} {1}", e, json));
        }

    }
    private void initNotifications(JSONArray jsonNotifications) {
        int count = jsonNotifications.length();
        List<Notification> notifications = new ArrayList<Notification>();
        try {
            for (int i = 0; i < count; i++) {
                JSONObject notification = jsonNotifications.getJSONObject(i);
                int id = notification.getInt("id");
                String title = notification.getString("title");
                String content = notification.getString("content");
                String tag = notification.getString("tag");
                long timeSec = notification.getLong("timeSec");
                Notification notif = prepareNotif(id, title, content, tag, timeSec);
                notifications.add(notif);
            }
        } catch (JSONException e) {
            Log.d(LOG_TAG, MessageFormat.format("Error while init notifications {0}", e));
        }

        mNotificationTimerService.scheduleNotifications(notifications);
    }

    private Notification prepareNotif(int id, String title, String content, String tag, long timeSec) {
        //Convert GMT to UTC
        TimeZone timeZone = TimeZone.getDefault();
        Date fireDate = new Date(timeSec * 1000 - timeZone.getRawOffset());
        Log.d(LOG_TAG, MessageFormat.format("DefaultTimeZone {0}", timeZone));
        Log.d(LOG_TAG, MessageFormat.format("initNotification id={0} title={1} content={2} fireDate={3,date} time={4}",
                id, title, content, fireDate, timeSec));
        Notification notification = new Notification();
        notification.setId(id);
        notification.setTitle(title);
        notification.setMessage(content);
        notification.setFireDate(fireDate);
        notification.setTag(tag);
        return notification;
    }

    public void removeNotif(int id) {
        Log.d(LOG_TAG, MessageFormat.format("remove notification with id {0}", id));
        mNotificationTimerService.removeNotifications(id);
    }

    public String getTag() {
        final SlotActivity activity = (SlotActivity) UnityPlayer.currentActivity;
        String tag = activity.getTag();
        return (tag != null ? tag : "");
    }

    public void clearTag() {
        final SlotActivity activity = (SlotActivity) UnityPlayer.currentActivity;
        activity.setTag("");
    }
}
