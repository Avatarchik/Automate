package com.vclub.notification.db.dto;

import java.text.MessageFormat;
import java.util.Date;

public class Notification extends AbstractTO {

    public static final String TABLE_NAME = "NotificationTimers";
    public static final String TITLE = "title";
    public static final String MESSAGE = "message";
    public static final String TAG = "tag";
    public static final String FIRE_TIME = "fireTime";

    public static final String CREATE_SQL = MessageFormat.format("CREATE TABLE {0} ({1} INTEGER PRIMARY KEY, " +
            "{2} VARCHAR , {3} VARCHAR, {4} VARCHAR, {5} NUMERIC)", TABLE_NAME, ID, TITLE, MESSAGE, TAG, FIRE_TIME);
    public static final String DROP_SQL =  MessageFormat.format("DROP TABLE {0}", TABLE_NAME);
    public static final String INSERT_NOTIF = "INSERT or Replace INTO NotificationTimers VALUES (?,?,?,?,?);";
    public static final String DELETE_NOTIF = "DELETE FROM NotificationTimers WHERE id IN (?);";

    private String mTitle;
    private String mMessage;
    private Date mFireDate;
    private String mTag;

    public String getTitle() {
        return mTitle;
    }

    public void setTitle(String title) {
        mTitle = title;
    }

    public String getMessage() {
        return mMessage;
    }

    public void setMessage(String message) {
        mMessage = message;
    }

    public Date getFireDate() {
        return mFireDate;
    }

    public void setFireDate(Date fireDate) {
        mFireDate = fireDate;
    }

    public String getTag() {
        return mTag;
    }

    public void setTag(String tag) {
        mTag = tag;
    }

    @Override
    public String toString() {
        return "Notification{" +
                "id=" + getId() +
                ", title='" + mTitle + '\'' +
                ", message='" + mMessage + '\'' +
                ", fireDate=" + mFireDate +
                ", tag='" + mTag + '\'' +
                '}';
    }
}
