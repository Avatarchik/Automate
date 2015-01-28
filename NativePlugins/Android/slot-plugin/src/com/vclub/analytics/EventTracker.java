//package com.vclub.analytics;
//
//import android.content.Context;
//import android.os.Bundle;
//
//import com.core.Constants;
//import com.core.DebugConstants;
//import com.core.engine.utils.LogUtil;
//import com.core.engine.utils.SessionData;
//import com.facebook.AppEventsLogger;
//import com.flurry.android.FlurryAgent;
//import com.mobileapptracker.MATResponse;
//import com.mobileapptracker.MobileAppTracker;
//
//import org.json.JSONObject;
//
//import java.text.MessageFormat;
//import java.util.HashMap;
//import java.util.Map;
//
//public class EventTracker {
//
//    //Constants for Fun version.
//    public static final String INVITE = "INVITE";
//    public static final String INVITE_OK = "INVITE_OK";
//    public static final String JOIN_TO_OK_GROUP = "JOIN_TO_OK_GROUP";
//    public static final String GOOGLE_ADD_LIKE = "GOOGLE+_ADD_LIKE";
//    public static final String INVITE_EMAIL = "INVITE_EMAIL";
//    public static final String SPIN = "SPIN";
//    public static final String DOUBLE = "DOUBLE";
//    public static final String AUTOPLAY = "AUTOPLAY";
//    public static final String BONUS_GAINED = "BONUS_GAINED";
//    public static final String CASHIER_POPUP = "CASHIER_POPUP";
//    public static final String LIMITED_OFFER_POPUP = "LIMITED_OFFER_POPUP";
//    public static final String CASHIER_POPUP_PROMO = "CASHIER_POPUP_PROMO";
//    public static final String TAPJOY = "TAPJOY";
//    public static final String VALIDATION_RESULT_MISMATCH = "VALIDATION_RESULT_MISMATCH";
//
//    private static final String STATUS_LEVEL_UP = "STATUS_LEVEL_UP";
//    private static final String PURCHASE_START = "PURCHASE_START";
//    private static final String PURCHASE_FINISH = "PURCHASE_FINISH";
//    private static final String SLOT_OPEN = "SLOT_OPEN";
//    private static final String CASHIER_GAMEHALL = "CASHIER_GAMEHALL";
//    private static final String CASHIER_SLOT = "CASHIER_SLOT";
//    private static final String OPEN_BY_NOTIFICATION = "OPEN_APP_BY_NOTIFICATION";
//    public static final String NOTIFICATION_COUNTER = "notificationCounter";
//    public static final String SUCCESS_PAY_SPECIAL_OFFER = "SUCCESS_PAID_SPECIAL_OFFER";
//    public static final String OPEN_RATING_DIALOG = "OPEN_RATING_DIALOG";
//    public static final String SEND_RATING_OK = "SEND_RATING_OK";
//    public static final String OPEN_GPLUS_DIALOG = "OPEN_GPLUS_DIALOG";
//    public static final String CLICK_OPEN_GPLUS = "CLICK_OPEN_GPLUS";
//    public static final String OPEN_UPDATE_APP_POPUP = "OPEN_UPDATE_APP_POPUP";
//    public static final String REDIRECT_TO_MARKET_FOR_UPDATE_APP = "REDIRECT_TO_MARKET_FOR_UPDATE_APP";
//    public static final String GET_REQUIRED_BUILD_VERSION = "GET_REQUIRED_BUILD_VERSION";
//
//    //Constants for Real version.
//    public static final String SLOT_OPEN_DEMO = "SLOT_OPEN_DEMO";
//    public static final String SLOT_OPEN_REAL = "SLOT_OPEN_REAL";
//    public static final String POPUP_DEMO_CONDITIONS = "POPUP_DEMO_CONDITIONS";
//    public static final String POPUP_DEMO_COMPLETED = "POPUP_DEMO_COMPLETED";
//    public static final String CASHIER_CLICK_REAL = "CASHIER_CLICK";
//    public static final String POPUP_BANKRUPT = "POPUP_BANKRUPT";
//    public static final String POPUP_BANKRUPT_TO_CASHIER = "POPUP_BANKRUPT_TO_CASHIER";
//
//    // Cashier
//    public static final String NEW_CASHIER_OPEN = "NEW_CASHIER_OPEN";
//    public static final String NEW_CASHIER_PAYMENT_SELECT = "NEW_CASHIER_PAYMENT_SELECT";
//    public static final String NEW_CASHIER_PAYMENT_SELECT_VISA = "NEW_CASHIER_PAYMENT_SELECT_VISA";
//    public static final String NEW_CASHIER_PAYMENT_SELECT_QIWI = "NEW_CASHIER_PAYMENT_SELECT_QIWI";
//    public static final String NEW_CASHIER_PAYMENT_SELECT_SMS = "NEW_CASHIER_PAYMENT_SELECT_SMS";
//    public static final String NEW_CASHIER_PAYMENT_SELECT_MOBILE = "NEW_CASHIER_PAYMENT_SELECT_MOBILE";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT = "NEW_CASHIER_PAYMENT_SUBMIT";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM = "NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_VISA = "NEW_CASHIER_PAYMENT_SUBMIT_VISA";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_VISA = "NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_VISA";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_QIWI = "NEW_CASHIER_PAYMENT_SUBMIT_QIWI";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_QIWI = "NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_QIWI";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_MOB_COMM = "NEW_CASHIER_PAYMENT_SUBMIT_CUSTOM_MOBILE_COMMERCE";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_MOB_COMM = "NEW_CASHIER_PAYMENT_SUBMIT_MOBILE_COMMERCE";
//    public static final String NEW_CASHIER_PAYMENT_SUBMIT_SMS = "NEW_CASHIER_PAYMENT_SUBMIT_SMS";
//    public static final String NEW_CASHIER_PAYMENT_REQUEST_SUCCESS = "NEW_CASHIER_PAYMENT_REQUEST_SUCCESS";
//    public static final String NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_VISA = "NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_VISA";
//    public static final String NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_QIWI = "NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_QIWI";
//    public static final String NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_SMS = "NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_SMS";
//    public static final String NEW_CASHIER_PAYMENT_REQUEST_SUCCESS_MOB_COMM = "NEW_CASHIER_PAYMENT_REQUEST_MOBILE_COMMERCE";
//    public static final String NEW_CASHIER_PAYMENT_SUCCESS = "NEW_CASHIER_PAYMENT_SUCCESS";
//    public static final String NEW_CASHIER_PAYMENT_SUCCESS_VISA = "NEW_CASHIER_PAYMENT_SUCCESS_VISA";
//    public static final String NEW_CASHIER_PAYMENT_SUCCESS_QIWI = "NEW_CASHIER_PAYMENT_SUCCESS_QIWI";
//    public static final String NEW_CASHIER_PAYMENT_SUCCESS_SMS = "NEW_CASHIER_PAYMENT_SUCCESS_SMS";
//    public static final String FIRST_SUCCESS_CASHIER_PAYMENT_BY_VISA = "FIRST_SUCCESS_CASHIER_PAYMENT_BY_VISA";
//
//    //FB
//    public static final String FB_EVENT_INSTALL_FUN = "Fun app install";
//    public static final String FB_EVENT_INSTALL_REAL = "Real app install";
//    public static final String FB_EVENT_CASHIER_FUN = "Open fun cashier";
//    public static final String FB_EVENT_CASHIER_REAL = "Open real cashier";
//    public static final String FB_EVENT_PURCHASE_FUN = "Success fun purchase";
//    public static final String FB_EVENT_PURCHASE_REAL = "Success real purchase";
//
//    private static Context sContext;
//    private static MobileAppTracker sMobileAppTracker;
//    private static AppEventsLogger sFbEventsLogger;
//
//    public static void start(final Context context) {
//        sContext = context;
//        FlurryAgent.setLogEnabled(true);
//        FlurryAgent.setLogEvents(true);
//        if (SessionData.getInstance().isFunVersion()) {
//            FlurryAgent.onStartSession(context, Constants.FLURRY_SECRET_KEY_FUN);
//        } else {
//            FlurryAgent.onStartSession(context, Constants.FLURRY_SECRET_KEY_REAL);
//            initMAT(context);
//        }
//        initFbLogger(sContext);
//    }
//
//    public static void initFbLogger(Context context) {
//        if (context != null) {
//            sFbEventsLogger = AppEventsLogger.newLogger(context);
//        }
//    }
//
//    private static void initMAT(final Context context) {
//        if (sMobileAppTracker == null) {
//            sMobileAppTracker = new MobileAppTracker(context, Constants.MAT_ADVERTISER_ID, Constants.MAT_CONVERSATION_KEY, true, false);
//            if (DebugConstants.IS_LOGGING) {
//                sMobileAppTracker.setMATResponse(new MATResponse() {
//                    @Override
//                    public void didSucceedWithData(final JSONObject jsonObject) {
//                        LogUtil.d(this, MessageFormat.format("MAT RESPONSE: {0}", jsonObject));
//                    }
//                });
//                sMobileAppTracker.setDebugMode(true);
//            }
//            sMobileAppTracker.trackInstall();
//            sMobileAppTracker.setEventReferrer(context.getPackageName());
//            LogUtil.i(context, MessageFormat.format("MAT initialized: {0}", sMobileAppTracker.getMatId()));
//        }
//    }
//
//    public static void stop(final Context context) {
//        FlurryAgent.onEndSession(context);
//    }
//
//    public static void setBaseInformation(final int age, final Boolean isMale) {
//        FlurryAgent.setAge(age);
//        FlurryAgent.setUserId(SessionData.getInstance().getCurrentUserLogin());
//        if (isMale) {
//            FlurryAgent.setGender(com.flurry.android.Constants.MALE);
//        } else {
//            FlurryAgent.setGender(com.flurry.android.Constants.FEMALE);
//        }
//    }
//
//    public static void logPurchaseStart(final double item) {
//        Map<String, String> params = new HashMap<String, String>();
//        params.put("Item", String.valueOf(item));
//        logEvent(PURCHASE_START, params);
//    }
//
//    public static void logPurchaseFinish(final double item) {
//        Map<String, String> params = new HashMap<String, String>();
//        params.put("Item", String.valueOf(item));
//        logEvent(PURCHASE_FINISH, params);
//    }
//
//    public static void logOpenSlot(final String slotName) {
//        Map<String, String> params = new HashMap<String, String>();
//        params.put("Slot", slotName);
//        if (SessionData.getInstance().isFunVersion()) {
//            logEvent(SLOT_OPEN, params);
//        } else if (SessionData.getInstance().isFunCurrency()) {
//            logRealEvent(SLOT_OPEN_DEMO, params);
//        } else if (!SessionData.getInstance().isFunCurrency()) {
//            logRealEvent(SLOT_OPEN_REAL, params);
//        }
//    }
//
//    public static void logOpenCashier(final boolean isSlot) {
//        if (isSlot) {
//            logEvent(CASHIER_SLOT);
//        } else {
//            logEvent(CASHIER_GAMEHALL);
//        }
//    }
//
//    public static void logStatusEvent(final String status) {
//        Map<String, String> params = new HashMap<String, String>();
//        params.put("Status", status);
//        logEvent(STATUS_LEVEL_UP, params);
//    }
//
//    public static void logEvent(final String eventId) {
//        if (SessionData.getInstance().isFunVersion()) {
//            FlurryAgent.logEvent(eventId);
//        }
//    }
//
//    public static void logEvent(final String eventId, Map<String, String> params) {
//        if (SessionData.getInstance().isFunVersion()) {
//            FlurryAgent.logEvent(eventId, params);
//        }
//    }
//
//    public static void logRealEvent(final String eventId) {
//        if (!SessionData.getInstance().isFunVersion()) {
//            FlurryAgent.logEvent(eventId);
//            logMatEvent(eventId);
//        }
//    }
//
//    public static void logRealEvent(final String eventId, Map<String, String> params) {
//        if (!SessionData.getInstance().isFunVersion()) {
//            FlurryAgent.logEvent(eventId, params);
//            logMatEvent(eventId);
//        }
//    }
//
//    public static void logOpenByNotifEvent(final Context context, final Bundle extras) {
//        if (extras != null) {
//            final int count = extras.getInt(NOTIFICATION_COUNTER);
//            LogUtil.d(context, MessageFormat.format("Count notification before open application by it [{0}]", count));
//            if (count > 0) {
//                Map<String, String> params = new HashMap<String, String>();
//                params.put("Count", String.valueOf(count));
//                FlurryAgent.logEvent(OPEN_BY_NOTIFICATION, params);
//                context.getSharedPreferences("SlotEngine", Context.MODE_PRIVATE)
//                        .edit().putInt(NOTIFICATION_COUNTER, 0).commit();
//            }
//        }
//    }
//
//    public static void logSpecialOfferPurchaseFinish(final String item) {
//        if (item.toLowerCase().contains("so_")) {
//            Map<String, String> params = new HashMap<String, String>();
//            params.put("orderId", item);
//            logEvent(SUCCESS_PAY_SPECIAL_OFFER, params);
//        }
//    }
//
//    public static void logMatEvent(String eventId) {
//        if (sMobileAppTracker != null) {
//            if (eventId.equals(POPUP_DEMO_CONDITIONS)) {
//                sMobileAppTracker.trackAction("Tutorial started");
//            } else if (eventId.equals(POPUP_DEMO_COMPLETED)) {
//                sMobileAppTracker.trackAction("Tutorial completed");
//            } else if (eventId.equals(NEW_CASHIER_OPEN)) {
//                sMobileAppTracker.trackAction("Checkout started");
//            } else if (eventId.equals(NEW_CASHIER_PAYMENT_REQUEST_SUCCESS) ||
//                    eventId.equals(FIRST_SUCCESS_CASHIER_PAYMENT_BY_VISA)) {
//                sMobileAppTracker.trackAction("Checkout completed");
//            }
//        }
//    }
//
//    public static void registerFacebookEvent(String event) {
//        if (sFbEventsLogger == null) {
//            initFbLogger(sContext);
//        }
//        sFbEventsLogger.logEvent(event);
//        LogUtil.d(EventTracker.class, MessageFormat.format("Facebook event: {0}", event));
//    }
//}
