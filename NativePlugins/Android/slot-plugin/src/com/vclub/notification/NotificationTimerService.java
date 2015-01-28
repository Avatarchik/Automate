package com.vclub.notification;

import android.app.AlarmManager;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.net.Uri;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.vclub.SlotActivity;
import com.vclub.notification.db.dto.Notification;
import com.vclub.notification.db.service.NotificationService;
import com.vclub.notification.db.service.impl.NotificationServiceImpl;

import java.text.MessageFormat;
import java.util.Date;
import java.util.List;

/**
 * Created by Max Denissov on 26/11/14.
 */
public class NotificationTimerService {

    private static final String LOG_TAG = "Notification Timer Service";
    public static final String INTENT_NOTIFICATION_ID = "NotificationId";
    public static final String INTENT_NOTIFICATION_TITLE = "NotificationTitle";
    public static final String INTENT_NOTIFICATION_MESSAGE = "NotificationMessage";
    public static final String INTENT_NOTIFICATION_FIRE_TIME = "NotificationFireTime";
    public static final String INTENT_NOTIFICATION_TAG = "NotificationTag";

    private static Integer sNotificationIcon;
    private static Uri sNotificationSound;
    private NotificationService mStorage;
    private Context mContext;

    public NotificationTimerService(Context context) {
        this.mContext = context;
        this.mStorage = new NotificationServiceImpl(context);
    }

    public void scheduleNotifications(List<Notification> notifications) {
        scheduleNotifications(notifications, false);
    }

    private void scheduleNotifications(List<Notification> notifications, boolean isAfterReboot) {
        int size = notifications.size();
        int ids[] = new int[size];
        int index = 0;
        for (Notification notification : notifications) {
            ids[index++] = notification.getId();
        }
        removeNotifications(ids);
        //
        if (!isAfterReboot) {
            mStorage.batchInsert(notifications);
        }
        for (Notification notif : notifications) {
            scheduleTimer(notif);
        }
    }

    private void scheduleTimer(Notification notification) {
        final PendingIntent pi = preparePendingIntent(notification);
        final AlarmManager am = (AlarmManager) mContext.getSystemService(Context.ALARM_SERVICE);
        final long startTime = notification.getFireDate().getTime();
        am.set(AlarmManager.RTC, startTime, pi);
    }

    public boolean isActive(int id) {
        return mStorage.getById(id) != null;
    }

    public void removeNotifications(int id) {
        removeNotifications(new int[]{id});
    }

    public void removeNotifications(int ids[]) {
        Log.d(LOG_TAG, MessageFormat.format("removeNotification with ids {0}", ids));
        final AlarmManager am = (AlarmManager) mContext.getSystemService(Context.ALARM_SERVICE);
        mStorage.batchDelete(ids);
        for (int id : ids) {
            Intent intent = new Intent(mContext, NotificationTimerReceiver.class);
            intent.setAction(MessageFormat.format("action_{0}", id));
            final PendingIntent pi = PendingIntent.getBroadcast(mContext, id, intent, PendingIntent.FLAG_UPDATE_CURRENT);
            try {
                am.cancel(pi);
            } catch (Exception ex) {
                Log.d(LOG_TAG, "removeNotification update was not canceled. " + ex.toString());
            }
        }
    }

    private PendingIntent preparePendingIntent(Notification notification) {
        Intent intent = new Intent(mContext, NotificationTimerReceiver.class);
        intent = notifToIntent(intent, notification);
        intent.setAction(MessageFormat.format("action_{0}", notification.getId()));
        final PendingIntent pi = PendingIntent.getBroadcast(mContext, notification.getId(), intent, PendingIntent.FLAG_UPDATE_CURRENT);
        return pi;
    }

    protected void rescheduleNotifications() {
        Log.d(LOG_TAG, "scheduleNotifications");
        scheduleNotifications(mStorage.getAll(), true);
    }

    protected void showNotification(Notification notif) {
        Log.d(LOG_TAG, MessageFormat.format("showNotification {0}", notif));
        mStorage.remove(notif);
        NotificationManager notificationManager =
                (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE);
        Intent intent = new Intent(mContext, SlotActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
        intent = notifToIntent(intent, notif);
        PendingIntent pIntent = PendingIntent.getActivity(mContext, notif.getId(), intent,
                PendingIntent.FLAG_UPDATE_CURRENT);
        NotificationCompat.Builder noti = new NotificationCompat.Builder(mContext);

        noti.setContentTitle(notif.getTitle());
        noti.setContentText(notif.getMessage());
        noti.setSmallIcon(getNotificationIcon());
        noti.setSound(getNotificationSound());
        noti.setContentIntent(pIntent);
        android.app.Notification notification = noti.build();
        notification.flags |= android.app.Notification.FLAG_AUTO_CANCEL;
        notificationManager.notify(notif.getId(), notification);
    }

    private Uri getNotificationSound() {
        if (sNotificationSound == null) {
            Resources res = mContext.getResources();
            int identifier = res.getIdentifier("notification", "raw", "com.vclub");
            Log.d(LOG_TAG, MessageFormat.format("getNotificationSound {0}", identifier));
            sNotificationSound = Uri.parse("android.resource://" + mContext.getPackageName() + "/" + identifier);
        }
        return sNotificationSound;
    }

    private int getNotificationIcon() {
        if (sNotificationIcon == null) {
            Resources res = mContext.getResources();
            sNotificationIcon = res.getIdentifier("app_icon", "drawable", "com.vclub");
        }
        return sNotificationIcon;
    }

    public static Intent notifToIntent(Intent intent, Notification notification) {
        intent.putExtra(INTENT_NOTIFICATION_ID, notification.getId());
        intent.putExtra(INTENT_NOTIFICATION_TITLE, notification.getTitle());
        intent.putExtra(INTENT_NOTIFICATION_MESSAGE, notification.getMessage());
        intent.putExtra(INTENT_NOTIFICATION_FIRE_TIME, notification.getFireDate().getTime());
        intent.putExtra(INTENT_NOTIFICATION_TAG, notification.getTag());
        return intent;
    }

    public static Notification notifFromIntent(Intent intent) {
        Notification notification = new Notification();
        notification.setId(intent.getIntExtra(INTENT_NOTIFICATION_ID, 0));
        notification.setTitle(intent.getStringExtra(INTENT_NOTIFICATION_TITLE));
        notification.setMessage(intent.getStringExtra(INTENT_NOTIFICATION_MESSAGE));
        notification.setFireDate(new Date(intent.getLongExtra(INTENT_NOTIFICATION_FIRE_TIME, 0)));
        notification.setTag(intent.getStringExtra(INTENT_NOTIFICATION_TAG));
        return notification;
    }

    public void clearNotificationAtBar() {
        NotificationManager notificationManager =
                (NotificationManager) mContext.getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.cancelAll();
    }

}
