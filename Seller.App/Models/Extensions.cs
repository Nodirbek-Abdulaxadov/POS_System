using System.Linq;

namespace Seller.App.Models;

public static class Extensions
{
    public static string ToMoneyFormat(this string text)
    {
        if (text.Length < 3 || string.IsNullOrEmpty(text)) return text;

        text = Reverse(text.Replace(" ", ""));
        string result = string.Empty;
        for (int i = 0; i < text.Length; i++)
        {
            result += text[i];
            if ((i + 1) % 3 == 0)
            {
                result += " ";
            }
        }

        return Reverse(result).Trim();
    }

    private static string Reverse(string text)
    {
        string reversed = string.Empty;
        foreach (char c in text.Reverse())
        {
            reversed += c;
        }

        return reversed;
    }
}