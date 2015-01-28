package com.vclub.utils;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.ActivityManager.RunningAppProcessInfo;
import android.app.Application;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Environment;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.Log;
import android.util.Patterns;
import android.util.StringBuilderPrinter;

import com.tapjoy.TapjoyConnect;

import org.json.JSONObject;

import java.io.Closeable;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class AndroidUtil {

    private static final String TAG = "AndroidUtil";
    // Folders
    private static final String PROJECT_FULL_FOLDER = MessageFormat.format("{0}{1}Android/data{1}{2}{1}",
            Environment.getExternalStorageDirectory(), File.separator, "com.vclub");

    public static String getEmail(final Context context) {
        final List<String> emails = new ArrayList<String>();
        final Pattern emailPattern = Patterns.EMAIL_ADDRESS; // API level 8+
        final Account[] accounts = AccountManager.get(context).getAccounts();
        for (final Account account : accounts) {
            final String email = account.name;
            if (!TextUtils.isEmpty(email) && emailPattern.matcher(email).matches()) {
                if (!emails.contains(email)) {
                    emails.add(email);
                }
            }
        }
        Log.d(TAG, MessageFormat.format("Emails of device: {0}", emails));
        return !emails.isEmpty() ? emails.get(0) : "";
    }

    public static boolean isStorageWriteable() {
        final String status = Environment.getExternalStorageState();
        final boolean result = Environment.MEDIA_MOUNTED.equals(status);
        if (!result) {
            Log.w(TAG, MessageFormat.format("Storage state is not writeable: {0}", status));
        }
        return result;
    }

    public static boolean isProcess(final Application application, final String processName) {
        final Context context = application.getApplicationContext();
        final ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        final List<RunningAppProcessInfo> appList = manager.getRunningAppProcesses();
        for (final RunningAppProcessInfo info : appList) {
            if (info.pid == android.os.Process.myPid() && processName.equals(info.processName)) {
                return true;
            }
        }
        return false;
    }

    public static boolean isServiceRunning(final Context context, final Class serviceClass) {
        final ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        for (final ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }

    public static PackageInfo getPackageInfo(final Context context) {
        PackageInfo result = null;
        final String packageName = context.getPackageName();
        try {
            result = context.getPackageManager().getPackageInfo(packageName, 0);
        } catch (NameNotFoundException e) {
            Log.e(TAG, MessageFormat.format("Error getting package info for {0}", packageName), e);
        }
        return result;
    }

    public static String getApplicationInfoDump(final Context context) {
        String result = null;
        final PackageManager packageManager = context.getPackageManager();
        try {
            final ApplicationInfo info = packageManager.getApplicationInfo(context.getPackageName(), 0);
            final StringBuilder sb = new StringBuilder();
            final StringBuilderPrinter printer = new StringBuilderPrinter(sb);
            info.dump(printer, "    ");
            result = sb.toString();
        } catch (NameNotFoundException e) {
            Log.e(TAG, "Error getting application info", e);
        }
        return result;
    }

    private static Properties sProps = null;
    private static final String PROPS_FILE = "project.props";

    private static void loadPersistentSettings() {
        if (sProps == null) {
            sProps = new Properties();
            FileInputStream fis = null;
            try {
                fis = new FileInputStream(new File(PROJECT_FULL_FOLDER, PROPS_FILE));
                sProps.load(fis);
            } catch (FileNotFoundException e) {
                // File not found, so just create one
                savePersistentSettings();
            } catch (IOException e) {
                Log.e(TAG, "Error load persistent setting ", e);
            } finally {
                closeStream(fis);
            }
        }
    }

    private static void savePersistentSettings() {
        FileOutputStream fos = null;
        try {
            final File file = new File(PROJECT_FULL_FOLDER, PROPS_FILE);
            if (!file.exists()) {
                file.getParentFile().mkdirs();
            }
            fos = new FileOutputStream(file);
            sProps.store(fos, null);
            fos.flush();
        } catch (IOException e) {
            Log.e(TAG, "Error save persistent settings", e);
        } finally {
            closeStream(fos);
        }
    }

    private static void createDirectory(String path) {
        final File file = new File(path);
        if (!file.exists()) {
            file.mkdirs();
        }
    }

    /**
     * Closes the specified stream.
     *
     * @param pCloseable The stream to close.
     */
    public static final void closeStream(final Closeable pCloseable) {
        if (pCloseable != null) {
            try {
                pCloseable.close();
            } catch (final IOException e) {
                e.printStackTrace();
            }
        }
    }

    public static void writePersistentSetting(final String key, final String value) {
        loadPersistentSettings();
        sProps.put(key, value);
        savePersistentSettings();
    }

    public static String readPersistentSetting(final String key, final String defValue) {
        loadPersistentSettings();
        return sProps.getProperty(key, defValue);
    }

    public static void writePersistentSettingBoolean(final String key, final boolean value) {
        writePersistentSetting(key, String.valueOf(value));
    }

    public static void writePersistentSettingDouble(final String key, final double value) {
        writePersistentSetting(key, String.valueOf(value));
    }

    public static boolean readPersistentSettingBoolean(final String key, final boolean defValue) {
        final String value = AndroidUtil.readPersistentSetting(key, String.valueOf(defValue));
        return Boolean.valueOf(value);
    }

    public static double readPersistentSettingDouble(final String key, final double defValue) {
        final String value = AndroidUtil.readPersistentSetting(key, String.valueOf(defValue));
        return Double.valueOf(value);
    }

    public static void tapjoyAction(final SharedPreferences preferences, final String id) {
        if (!preferences.getBoolean(id, false)) {
            Log.i(TAG, MessageFormat.format("Tapjoy action complete: {0}", id));
            TapjoyConnect.getTapjoyConnectInstance().actionComplete(id);
            final SharedPreferences.Editor editor = preferences.edit();
            editor.putBoolean(id, true);
            editor.commit();
        }
    }

    public static void openUrl(final Context context, final String url) {
        final Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(url));
        context.startActivity(intent);
    }

    public static String getDeviceId(final Context context) {
        final TelephonyManager manager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
        return manager.getDeviceId();
    }

    public static boolean launchApp(final Context context, final String packageName) {
        boolean result = true;
        final PackageManager pm = context.getPackageManager();
        try {
            final Intent intent = pm.getLaunchIntentForPackage(packageName);
            context.startActivity(intent);
        } catch (Exception e) {
            Log.w(TAG, e);
            result = false;
        }
        return result;
    }

    public static String optString(final JSONObject o, final String name, final String defValue) {
        String result = defValue;
        if (!o.isNull(name)) {
            result = o.optString(name, defValue);
        }
        return result;
    }

    public static boolean isAppInstalled(final Context context, final String packageName) {
        final PackageManager pm = context.getPackageManager();
        boolean installed = false;
        try {
            pm.getPackageInfo(packageName, PackageManager.GET_ACTIVITIES);
            installed = true;
        } catch (NameNotFoundException e) {
        }
        return installed;
    }

    public static void openPlayMarket(final Activity activity) {
        String packageName = activity.getPackageName();
        Intent marketIntent;
        try {
            marketIntent = new Intent(Intent.ACTION_VIEW,
                    Uri.parse("market://details?id=" + packageName));
        } catch (android.content.ActivityNotFoundException anfe) {
            marketIntent = new Intent(Intent.ACTION_VIEW,
                    Uri.parse("http://play.google.com/store/apps/details?id=" + packageName));
        }
        marketIntent.addFlags(Intent.FLAG_ACTIVITY_NO_HISTORY |
                Intent.FLAG_ACTIVITY_CLEAR_WHEN_TASK_RESET | Intent.FLAG_ACTIVITY_MULTIPLE_TASK);
        activity.startActivity(marketIntent);
    }

    public static boolean isVersionActual(String currentVersion, String requiredVersion) {
        int[] currentVersionNumbers = getVersionNumbers(currentVersion);
        int[] requiredVersionNumbers = getVersionNumbers(requiredVersion);
        for (int i = 0; i < currentVersionNumbers.length; i++) {
            if (currentVersionNumbers[i] < requiredVersionNumbers[i]) {
                return false;
            }
        }
        return true;
    }

    private static int[] getVersionNumbers(String version) {
        Matcher m = Pattern.compile("(\\d+)\\.(\\d+)\\.(\\d+)")
                .matcher(version);
        if (!m.matches()) {
            return new int[]{0, 0, 0};
        }
        return new int[]{Integer.parseInt(m.group(1)),  // major
                Integer.parseInt(m.group(2)),             // minor
                Integer.parseInt(m.group(3))              // rev.
        };
    }

    public static boolean isNetworkAvailable(Context context) {
        ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetwork = cm.getActiveNetworkInfo();
        return activeNetwork != null && activeNetwork.isConnectedOrConnecting();
    }
}
