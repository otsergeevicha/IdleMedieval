using UnityEngine;

public class FormatMoneys : MonoBehaviour
{
    static string[] names = { "", "K", "M", "B", "T", "Q" };

    public static string FormatMoney(float digit)
    {
        int n = 0;
        while (n + 1 < names.Length && digit >= 1000)
        {
            digit /= 1000;
            n++;
        }
        return digit.ToString("#.##") + names[n];
    }

    public static string TotalEnemyText(int enemyCount)
    {
        if (enemyCount < 10)
            return ("1-10");
        else if (enemyCount >= 10 && enemyCount <= 50)
            return ("10-50");
        else if (enemyCount >= 50 && enemyCount <= 100)
            return ("50-100");
        else if (enemyCount >= 100 && enemyCount <= 500)
            return ("100-500");
        else if (enemyCount >= 500 && enemyCount <= 1000)
            return ("500-1K");
        else if (enemyCount >= 500 && enemyCount <= 1000)
            return ("500-1K");
        else if (enemyCount >= 1000 && enemyCount <= 5000)
            return ("1K-5K");
        else if (enemyCount >= 5000 && enemyCount <= 10000)
            return ("5K-10K");
        else
            return (">10K");
    }
}
