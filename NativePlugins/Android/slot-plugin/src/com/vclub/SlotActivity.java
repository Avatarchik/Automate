package com.vclub;

import android.app.NotificationManager;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;
import com.vclub.notification.NotificationTimerService;
import com.vclub.notification.db.dto.Notification;
import com.vclub.notification.db.service.NotificationService;

import java.text.MessageFormat;

/**
 * Created by Max Denissov on 04/12/14.
 */
public class SlotActivity extends UnityPlayerActivity {

    private static final String LOG_TAG = "SlotActivity";
    private String mTag;

    public String getTag() {
        return mTag;
    }

    public void setTag(String tag) {
        this.mTag = tag;
    }

    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
        Notification notification = NotificationTimerService.notifFromIntent(getIntent());
        Log.d(LOG_TAG, MessageFormat.format("onCreate {0}", notification));
        mTag = notification.getTag();
        NotificationManager notificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.cancelAll();
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        Notification notification = NotificationTimerService.notifFromIntent(intent);
        Log.d(LOG_TAG, MessageFormat.format("onNewIntent {0}", notification));
        mTag = notification.getTag();
        NotificationManager notificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.cancelAll();
    }
}
