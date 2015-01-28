using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using System;
using System.Globalization;
using UnityEngine;

public static class NumberUtil {

    public static  double EPSILON = 0.00001;
    public static  NumberFormatInfo NFI;

    static NumberUtil() {
        NFI = new NumberFormatInfo ();
        NFI.NumberDecimalSeparator = ".";
        NFI.NumberDecimalDigits = 2;
    }

    public static double RoundFloat (float value, int decimals) {
        double pow = Math.Pow (10, decimals);
        double tmp = Math.Floor (value * pow);
        return tmp / pow;
    }
    
    public static string FormatDouble (double value) {
        return value.ToString ("N", NFI);
    }
    
    public static int GetProgressLineIndex (float progress, float minValue,
                                           float maxValue, int maxLines) {
        int lineIndex = (int)((progress - minValue) / (maxValue - minValue) * (maxLines - 1));
        return Math.Min (lineIndex, maxLines - 1);
    }
    
    public static int GetProgressLineIndex (float progressPct, int maxLines) {
        int lineIndex = (int)(progressPct / 100.0f * (maxLines - 1));
        return Math.Min (lineIndex, maxLines - 1);
    }
    
    public static float Limit (float value, float min, float max) {
        return Math.Min (Math.Max (value, min), max);
    }
    
    public static int Limit (int value, int min, int max) {
        return Math.Min (Math.Max (value, min), max);
    }
    
    public static bool Equals (double lhs, double rhs) {
        return Math.Abs (lhs - rhs) < EPSILON;
    }
}
