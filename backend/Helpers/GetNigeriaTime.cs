public static class TimeHelper
{
    public static DateTime GetNigeriaTime()
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime nigeriaTime = utcNow.AddHours(1);
        return nigeriaTime;
    }
}
