package com.vclub.notification.db.dto;

import java.io.Serializable;

public class AbstractTO implements Serializable {

    public static final String ID = "id";

    private int mId;

    public int getId() {
        return mId;
    }

    public void setId(int id) {
        mId = id;
    }
}
