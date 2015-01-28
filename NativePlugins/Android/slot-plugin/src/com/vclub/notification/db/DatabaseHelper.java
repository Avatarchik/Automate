package com.vclub.notification.db;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.vclub.notification.db.dto.Notification;

public class DatabaseHelper extends SQLiteOpenHelper {

    private static final String DATABASE_NAME = "vclub-local.db";
    private static final int DATABASE_VERSION = 1;
    protected static DatabaseHelper sStorage;

    public static synchronized DatabaseHelper getInstance(final Context context) {
        if (sStorage == null) {
            sStorage = new DatabaseHelper(context);
        }
        return sStorage;
    }

    public DatabaseHelper(final Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase sqLiteDatabase) {
        sqLiteDatabase.execSQL(Notification.CREATE_SQL);
    }

    @Override
    public void onUpgrade(SQLiteDatabase sqLiteDatabase, int oldVersion, int newVersion) {
        sqLiteDatabase.execSQL(Notification.DROP_SQL);
        onCreate(sqLiteDatabase);
    }
}
