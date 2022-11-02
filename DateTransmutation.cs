public static DateTime GetFirstDayOfNextMonth(DateTime inputDate)
{
    if (inputDate.Month == 12)
        inputDate = new DateTime((inputDate.Year + 1), 1, 1);
    else
        inputDate = new DateTime(inputDate.Year, (inputDate.Month + 1), 1);

    return inputDate;
}

public static DateTime GetLastDayOfNextMonth(DateTime inputDate)
{
    return GetFirstDayOfNextMonth(inputDate).AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0, 0));
}

public static DateTime GetLastDayOfPreviousMonth(DateTime inputDate)
{
    return GetFirstDayOfNextMonth(inputDate).AddMonths(-1).Subtract(new TimeSpan(1, 0, 0, 0, 0));
}

public static DateTime GetLastDayOfCurrentMonth(DateTime inputDate)
{
    return GetFirstDayOfNextMonth(inputDate).Subtract(new TimeSpan(1, 0, 0, 0, 0));
}