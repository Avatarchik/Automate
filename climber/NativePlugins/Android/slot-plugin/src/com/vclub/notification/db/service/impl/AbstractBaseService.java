package com.vclub.notification.db.service.impl;

import android.content.Context;

import com.vclub.notification.db.DatabaseHelper;
import com.vclub.notification.db.service.BaseService;


public abstract class AbstractBaseService<T> implements BaseService<T> {

    protected DatabaseHelper mDatabaseHelper;

    protected AbstractBaseService(Context context) {
        mDatabaseHelper = DatabaseHelper.getInstance(context);
    }
}
