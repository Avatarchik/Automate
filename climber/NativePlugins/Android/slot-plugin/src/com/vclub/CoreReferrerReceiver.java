package com.vclub;

import android.annotation.TargetApi;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;

import com.google.analytics.tracking.android.CampaignTrackingReceiver;
import com.google.analytics.tracking.android.EasyTracker;
import com.mobileapptracker.Tracker;
import com.vclub.utils.AndroidUtil;
import com.vclub.utils.Base64;
import com.vclub.utils.Base64DecoderException;

import java.io.UnsupportedEncodingException;
import java.net.URLDecoder;
import java.text.MessageFormat;
import java.util.HashMap;
import java.util.Map;

public class CoreReferrerReceiver extends BroadcastReceiver {

    private static final String TAG = "CoreReferrerReceiver";
    private static final String SETTINGS_GOOGLE_REFERRER_PARSE = "googleReferrerParse";
    private static final String SETTINGS_GOOGLE_REFERRER = "googleReferrer";
    private static final String SETTINGS_REFER = "refer";
    private static final String SETTINGS_TR = "tr";
    private static final String SETTINGS_FKEY = "fkey";

    private static final String MAT_PREFIX = "=&mat_";
    private static final String REF_ID = "refId";
    private static final String TRANSACTION_ID = "transaction_id";
    private static final String INSTALL_DATE = "install_date";
    private static final String ADVERTISER_ID = "advertiser_id";
    public static SharedPreferences mPreferences;

    @Override
    public void onReceive(final Context context, final Intent intent) {
        mPreferences = context.getSharedPreferences(context.getPackageName(), Context.MODE_PRIVATE);
        boolean parseReferrer = mPreferences.getInt(SETTINGS_GOOGLE_REFERRER_PARSE, 1) == 1;
        if (parseReferrer) {
            Log.w(TAG, "--------======== REFERRER RECEIVE START ========--------");
            final Bundle extras = intent.getExtras();
            if (extras != null) {
                try {
                    parseReferrer(extras);
                } catch (UnsupportedEncodingException e) {
                    Log.e(TAG, "Error decode referrer: ", e);
                }
            }
            // Pass data to GA
            new CampaignTrackingReceiver().onReceive(context, intent);

            // Pass data to MAT
            new Tracker().onReceive(context, intent);
            Log.w(TAG, "--------======== REFERRER RECEIVE END ========--------");
        }
    }

    @TargetApi(Build.VERSION_CODES.GINGERBREAD)
    private void parseReferrer(Bundle extras) throws UnsupportedEncodingException {
        String referrer = extras.getString("referrer");
        if (!TextUtils.isEmpty(referrer)) {
            referrer = URLDecoder.decode(referrer, "UTF-8");
            Log.w(TAG, MessageFormat.format("Get referrer from extras [{0}] ", referrer));
            try {
                Map<String, String> matReferrer = parseMATreferrer(referrer);
                String decodedRef = new String(Base64.decode(referrer));
                if (matReferrer != null && !matReferrer.isEmpty()) {
                    if (!matReferrer.containsKey(REF_ID) || TextUtils.isEmpty(matReferrer.get(REF_ID))) {
                        decodedRef = matReferrer.get(REF_ID);
                    }
                }
                Log.w(TAG, MessageFormat.format("Referrer [{0}] decode value [{1}]", referrer, decodedRef));
                final String refUri = MessageFormat.format("{0}?{1}", "http://volcano-club-api.com", decodedRef);
//                final String refUri = MessageFormat.format("{0}?{1}", SlotsServerClient.getAbsoluteUrl(""), decodedRef);
                final SharedPreferences.Editor edit = mPreferences.edit();
                edit.putString(SETTINGS_GOOGLE_REFERRER, refUri).apply();
                if (!TextUtils.isEmpty(refUri)) {
                    final Uri uri = Uri.parse(refUri);
                    Log.d(TAG, MessageFormat.format("Google referrer uri: {0}", uri));
                    setReferrer(uri);
                }
                Log.w(TAG, MessageFormat.format("Google referrer uri: {0}", refUri));
            } catch (Exception e) {
                Log.w(TAG, e.getMessage(), e);
            }
        }
    }

    private Map<String, String> parseMATreferrer(String referrer) throws Base64DecoderException {
        Map<String, String> map = new HashMap<String, String>();
        if (referrer.contains(MAT_PREFIX)) {
            String[] refParams = referrer.split("&");
            for (String refItem : refParams) {
                String[] pair = refItem.split("=");
                if (pair.length > 1) {
                    String[] transactionParam = pair[1].split("-");
                    map.put(TRANSACTION_ID, transactionParam[0]);
                    map.put(INSTALL_DATE, transactionParam[1]);
                    map.put(ADVERTISER_ID, transactionParam[2]);
                } else {
                    final byte[] decoded = Base64.decode(refItem);
                    map.put(REF_ID, new String(decoded));
                }
            }
            Log.w(TAG, MessageFormat.format("Referrer [{0}] from MAT transaction_id [{1}], install_date [{2}], " +
                            "advertiser_id [{2}], refId [{3}]", referrer, map.get(TRANSACTION_ID), map.get(INSTALL_DATE),
                    map.get(ADVERTISER_ID), map.get(REF_ID)));
        }
        return map;
    }

    /**
     * Checks saved referral and updates game state according to it
     */
    private void setReferrer(final Uri uri) {
        final String referrer = uri.getQueryParameter("refid");
        final String fkey = uri.getQueryParameter("fkey");
        final String tr = uri.getQueryParameter("tr");
        Log.d(TAG, MessageFormat.format("REFERRER: saving value [{0}], fkey [{1}], tr [{2}]",
                referrer, fkey, tr));
        writeReferrer(referrer, fkey, tr);
        if (!TextUtils.isEmpty(referrer)) {
            EasyTracker.getTracker().setCampaign(referrer);
        }
    }

    private void writeReferrer(final String referrer, final String fkey, final String tr) {
        AndroidUtil.writePersistentSetting(SETTINGS_REFER, referrer == null ? "" : referrer);
        // Save referral for future use
        final SharedPreferences.Editor editor = mPreferences.edit();
        editor.putString(SETTINGS_FKEY, fkey == null ? "" : fkey);
        editor.putString(SETTINGS_TR, tr == null ? "" : tr);
        editor.commit();
    }

}
