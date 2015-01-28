package com.vclub.notification;

import android.app.ActivityManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

import com.vclub.notification.db.dto.Notification;

import java.text.MessageFormat;
import java.util.List;

/**
 * Created by Max Denissov on 26/11/14.
 */
public class NotificationTimerReceiver extends BroadcastReceiver {

    private static final String LOG_TAG = "Notification TimerReceiver";
    private NotificationTimerService mNotificationTimerService;

    @Override
    public void onReceive(Context context, Intent intent) {
        boolean isOnForeground = isAppOnForeground(context);
        Log.d(LOG_TAG, MessageFormat.format("onReceive is application on foreground {0}", isOnForeground));
        final Notification notification = getNotification(intent);
        mNotificationTimerService = new NotificationTimerService(context);
        if (!isOnForeground) {
            Log.d(LOG_TAG,"onReceive show notification");
            mNotificationTimerService.showNotification(notification);
        } else {
            Log.d(LOG_TAG,"onReceive remove notification from storage");
            mNotificationTimerService.removeNotifications(notification.getId());
        }
    }

    private Notification getNotification(Intent intent) {
        Log.d(LOG_TAG, "getNotification");
        return NotificationTimerService.notifFromIntent(intent);
    }

    private boolean isAppOnForeground(Context context) {
        ActivityManager activityManager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        List<ActivityManager.RunningAppProcessInfo> appProcesses = activityManager.getRunningAppProcesses();
        if (appProcesses == null) {
            return false;
        }
        final String packageName = context.getPackageName();
        for (ActivityManager.RunningAppProcessInfo appProcess : appProcesses) {
            if (appProcess.importance == ActivityManager.RunningAppProcessInfo.IMPORTANCE_FOREGROUND && appProcess.processName.equals(packageName)) {
                return true;
            }
        }
        return false;
    }

}
