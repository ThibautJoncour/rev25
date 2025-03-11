using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace MagicNumber.Pages
{
    public class LoanScheduleBase : ComponentBase
    {
        protected DateTime StartDate { get; set; } = DateTime.Today;
        protected DateTime EndDate { get; set; } = DateTime.Today.AddMonths(6);
        protected int RollDay { get; set; } = 15;

        protected float Rate { get; set; } = 5f;
        protected float Notional { get; set; } = 1000f;
        protected string BDC { get; set; }

        protected float BasisCalcul { get; set; } = 360f;


        protected List<Dictionary<string, object>> ScheduleList { get; set; } = new();

        protected void GenerateSchedule()
        {
            ScheduleList.Clear();

            DateTime currentDate = StartDate;
            DateTime nextDate = new DateTime(StartDate.Year, StartDate.Month, RollDay);
            float dailyRate = Rate / BasisCalcul;
            float coupons;

            while (currentDate < EndDate)
            {
                if (nextDate > EndDate)
                {
                    float lastdelta;
                    nextDate = EndDate;
                    lastdelta = (nextDate - currentDate).Days;
                    coupons = Notional + lastdelta * dailyRate * Notional;
                    ScheduleList.Add(new Dictionary<string, object>
                    {
                    { "Start Date", currentDate.ToString("yyyy-MM-dd") },
                    { "End Date", nextDate.ToString("yyyy-MM-dd") },
                    { "Amount", Math.Round(coupons, 2) },
                    { "Status", "Paid" }
                    });
                    break;

                }
                float delta = (nextDate - currentDate).Days;
                coupons = delta * dailyRate * Notional;

                ScheduleList.Add(new Dictionary<string, object>
                {
                    { "Start Date", currentDate.ToString("yyyy-MM-dd") },
                    { "End Date", nextDate.ToString("yyyy-MM-dd") },
                    { "Amount", Math.Round(coupons, 2) },
                    { "Status", "Paid" }
                });

                currentDate = nextDate;
                int nextMonthDays = DateTime.DaysInMonth(nextDate.Year, nextDate.Month);
                RollDay = Math.Min(RollDay, nextMonthDays);
                nextDate = nextDate.AddMonths(1);
                nextDate = new DateTime(nextDate.Year, nextDate.Month, RollDay);
            }
        }
    }
}
