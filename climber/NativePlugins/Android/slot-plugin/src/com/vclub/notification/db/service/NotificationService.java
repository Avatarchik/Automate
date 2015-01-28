package com.vclub.notification.db.service;

import com.vclub.notification.db.dto.Notification;

import java.util.List;

public interface NotificationService extends BaseService<Notification> {

    List<Notification> getAll();

    void batchInsert(List<Notification> notificationList);

    void batchDelete(int[] ids);

}
