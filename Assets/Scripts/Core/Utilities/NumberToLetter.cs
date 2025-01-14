using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class NumberToLetter
{
    public static string FormatNumber(double num)
    {
        if (num >= 1e66)
            return num.ToString("0.00e0", CultureInfo.InvariantCulture);

        // Vigintillion
        if (num >= 1e63)
            return FormatNumber((num / 1e63)) + "*";

        // Novemdecillion
        if (num >= 1e60)
            return FormatNumber((num / 1e60)) + "&";

        // Octodecillion
        if (num >= 1e57)
            return FormatNumber((num / 1e57)) + "^";

        // Septendecillion
        if (num >= 1e54)
            return FormatNumber((num / 1e54)) + "%";

        // Sexdecillion
        if (num >= 1e51)
            return FormatNumber((num / 1e51)) + "$";

        // Quindecillion
        if (num >= 1e48)
            return FormatNumber((num / 1e48)) + "#";

        // Quattuordecillion
        if (num >= 1e45)
            return FormatNumber((num / 1e45)) + "@";

        // Tredecillion
        if (num >= 1e42)
            return FormatNumber((num / 1e42)) + "!";

        // Duodecillion
        if (num >= 1e39)
            return FormatNumber((num / 1e39)) + "D";

        // Undecillion
        if (num >= 1e36)
            return FormatNumber((num / 1e36)) + "U";

        // Decillion
        if (num >= 1e33)
            return FormatNumber((num / 1e33)) + "d";

        // Nonillion
        if (num >= 1e30)
            return FormatNumber((num / 1e30)) + "N";

        // Octillion
        if (num >= 1e27)
            return FormatNumber((num / 1e27)) + "O";

        // Septillion
        if (num >= 1e24)
            return FormatNumber((num / 1e24)) + "S";

        // Sextillion
        if (num >= 1e21)
            return FormatNumber((num / 1e21)) + "s";

        // Quintillion
        if (num >= 1e18)
            return FormatNumber((num / 1e18)) + "Q";

        // Quadrillion
        if (num >= 1e15)
            return FormatNumber((num / 1e15)) + "q";

        if (num >= 1e12)
            return FormatNumber((num / 1e12)) + "T";

        if (num >= 1e9)
            return FormatNumber((num / 1e9)) + "B";

        if (num >= 1e6)
            return FormatNumber((num / 1e6)) + "M";

        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";

        if (num >= 1000)
            return (num / 1000D).ToString("0.#") + "K";

        if (num >= 0)
            return num.ToString("0.00");

        return num.ToString("#,0");
    }
}
