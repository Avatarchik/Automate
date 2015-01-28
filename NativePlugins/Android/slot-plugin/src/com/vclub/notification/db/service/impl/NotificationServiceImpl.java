package com.vclub.notification.db.service.impl;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteStatement;
import android.text.TextUtils;
import android.util.Log;

import com.vclub.notification.db.dto.Notification;
import com.vclub.notification.db.service.NotificationService;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class NotificationServiceImpl extends AbstractBaseService<Notification> implements NotificationService {

    private static final String LOG_TAG = "Notification Service";

    public NotificationServiceImpl(Context context) {
        super(context);
    }

    @Override
    public Notification getById(int id) {
        final SQLiteDatabase db = mDatabaseHelper.getReadableDatabase();
        if (db != null) {
            final Cursor cursor = db.query(Notification.TABLE_NAME, null, Notification.ID + " = ?",
                    new String[]{String.valueOf(id)}, null, null, null, null);
            try {
                if (cursor.moveToNext()) {
                    return fromCursor(cursor);
                }
            } finally {
                cursor.close();
                db.close();
            }
        }
        return null;
    }

    @Override
    public void batchInsert(List<Notification> notificationList) {
        Log.d(LOG_TAG, "Insert list notification...");
        final SQLiteDatabase db = mDatabaseHelper.getWritableDatabase();
        try {
            SQLiteStatement statement = db.compileStatement(Notification.INSERT_NOTIF);
            db.beginTransaction();
            for (Notification notif : notificationList) {
                statement.clearBindings();
                statement.bindLong(1, notif.getId());
                statement.bindString(2, notif.getTitle());
                statement.bindString(3, notif.getMessage());
                statement.bindString(4, notif.getTag());
                statement.bindLong(5, notif.getFireDate().getTime());
                statement.execute();
                Log.d(LOG_TAG, "SAVE Notification!!! - " + notif.toString());
            }
            db.setTransactionSuccessful();
        } catch (Exception e) {
            Log.d(LOG_TAG, e.getMessage(), e);
        } finally {
            db.endTransaction();
            db.close();
        }
        Log.d(LOG_TAG, "Insert list notification items...ok");
    }

    @Override
    public void batchDelete(int[] ids) {
        String args = TextUtils.join(", ", toObject(ids)).toString();
        Log.d(LOG_TAG, "Delete list notification... " + args);
        final SQLiteDatabase db = mDatabaseHelper.getWritableDatabase();
        try {
            SQLiteStatement statement = db.compileStatement(Notification.DELETE_NOTIF);
            db.beginTransaction();
            statement.clearBindings();
            statement.bindString(1, args);
            statement.execute();
            db.setTransactionSuccessful();
        } catch (Exception e) {
            Log.d(LOG_TAG, e.getMessage(), e);
        } finally {
            db.endTransaction();
            db.close();
        }
        Log.d(LOG_TAG, "Delete list notification items...ok");
    }

    private static Integer[] toObject(int[] intArray) {
        Integer[] result = new Integer[intArray.length];
        for (int i = 0; i < intArray.length; i++) {
            result[i] = Integer.valueOf(intArray[i]);
        }
        return result;
    }

    @Override
    public void insertOrUpdate(Notification object) {
        //LogUtil.d(NotificationServiceImpl.class,
        //        MessageFormat.format("Add new notification... {0}", object.toString()));
        final SQLiteDatabase db = mDatabaseHelper.getWritableDatabase();
        final ContentValues values = new ContentValues();

        values.put(Notification.ID, object.getId());
        values.put(Notification.TITLE, object.getTitle());
        values.put(Notification.MESSAGE, object.getMessage());
        values.put(Notification.TAG, object.getTag());
        values.put(Notification.FIRE_TIME, object.getFireDate().getTime());

        if (db != null) {
            if (getById(object.getId()) == null) {
                db.insert(Notification.TABLE_NAME, null, values);
            } else {
                db.update(Notification.TABLE_NAME, values, Notification.ID + " = ?",
                        new String[]{String.valueOf(object.getId())});
            }
            db.close();
        }
        Log.d(LOG_TAG, "Add new notification... ok");
    }

    @Override
    public void remove(Notification object) {
        final SQLiteDatabase db = mDatabaseHelper.getWritableDatabase();
        if (db != null) {
            int result = db.delete(Notification.TABLE_NAME, Notification.ID + " = ?",
                    new String[]{String.valueOf(object.getId())});
            Log.d(LOG_TAG, MessageFormat.format("Removed timer [{0}]... ok", result));
            db.close();
        }
    }

    @Override
    public Notification fromCursor(Cursor cursor) {
        Notification notification = new Notification();
        notification.setId(cursor.getInt(cursor.getColumnIndex(Notification.ID)));
        notification.setTitle(cursor.getString(cursor.getColumnIndex(Notification.TITLE)));
        notification.setMessage(cursor.getString(cursor.getColumnIndex(Notification.MESSAGE)));
        notification.setFireDate(new Date(cursor.getLong(cursor.getColumnIndex(Notification.FIRE_TIME))));
        notification.setTag(cursor.getString(cursor.getColumnIndex(Notification.TAG)));
        return notification;
    }

    @Override
    public List<Notification> getAll() {
        Log.d(LOG_TAG, "Load all saved notification timer...");
        final SQLiteDatabase db = mDatabaseHelper.getReadableDatabase();
        List<Notification> notifications = new ArrayList<Notification>();
        if (db != null) {
            final Cursor cursor = db.query(Notification.TABLE_NAME,
                    null, null, null, null, null, null, null);
            try {
                while (cursor.moveToNext()) {
                    notifications.add(fromCursor(cursor));
                }
            } finally {
                cursor.close();
                db.close();
            }
        }
        Log.d(LOG_TAG, MessageFormat.format("Load all [{0}] saved notification timer...ok", notifications.size()));
        return notifications;
    }

}
