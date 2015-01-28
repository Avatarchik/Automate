package com.vclub.notification;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

/**
 * Created by Max Denissov on 26/11/14.
 */
public class NotificationBootReceiver extends BroadcastReceiver {

    private static final String LOG_TAG = "Notification BootReceiver";

    private NotificationTimerService mNotificationTimerService;

    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals(Intent.ACTION_BOOT_COMPLETED)) {
            mNotificationTimerService = new NotificationTimerService(context);
            Log.d(LOG_TAG, "ACTION_BOOT_COMPLETED");
            mNotificationTimerService.rescheduleNotifications();
        }
    }

}
