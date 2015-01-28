using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Globalization;
using System;

public static class StringUtil {

    public const int MinRadix = 2;
    public const int MaxRadix = 36;
    private static NumberFormatInfo Formatter = new NumberFormatInfo ();

    const string MoneyFormat = "#,##0.00";
    const string ShortMoneyFormat = "#,##0";
    const string BigMoneyFormat = "###0";

    public static string BytesToString (byte[] bytes) {
        int length = bytes.Length;
        StringBuilder sb = new StringBuilder (length << 1);
        foreach (byte aByte in bytes) {
            sb.Append (ForDigit ((aByte & 0xf0) >> 4, 16));
            sb.Append (ForDigit (aByte & 0x0f, 16));
        }
        return sb.ToString ();
    }

    public static char ForDigit (int digit, int radix) {
        if (MinRadix <= radix && radix <= MaxRadix) {
            if (digit >= 0 && digit < radix) {
                return (char)(digit < 10 ? digit + '0' : digit + 'a' - 10);
            }
        }
        return (char)0;
    }

    //TODO перенести потом в утильный класс
    /// <summary>
    /// Generate the md5 password.
    /// </summary>
    /// <returns>The M d5.</returns>
    /// <param name="input">Input.</param>
    public static string CreateMD5 (string input) {
        // Use input string to calculate MD5 hash
        MD5 md5 = System.Security.Cryptography.MD5.Create ();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input);
        byte[] hashBytes = md5.ComputeHash (inputBytes);
        
        // Convert the byte array to hexadecimal string
        StringBuilder sb = new StringBuilder ();
        for (int i = 0; i < hashBytes.Length; i++) {
            sb.Append (hashBytes [i].ToString ("X2"));
        }
        return sb.ToString ();
    }

    public static string FormatMoney (double value, bool withSign) {
        Formatter.NumberDecimalSeparator = ".";
        StringBuilder sb = new StringBuilder ();
        if (withSign) {
            sb.Append ("$");
        }
        if (value % 1 == 0) {
            sb.Append(String.Format(ShortMoneyFormat, value));
        } else {
            sb.Append(String.Format(MoneyFormat, value));
        }
        return sb.ToString ();
    }
}
