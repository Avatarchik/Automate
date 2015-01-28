package com.vclub.notification.db.service;

import android.database.Cursor;

public interface BaseService<T> {

    T getById(int id);

    void insertOrUpdate(T object);

    void remove(T object);

    T fromCursor(Cursor cursor);
}
