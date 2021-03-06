﻿using Autofac;
using AutoMapper;
using ProductivityTools.CalculateEmails.Autofac;
using ProductivityTools.CalculateEmails.Contract.DataContract;
using ProductivityTools.CalculateEmails.DALContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductivityTools.CalculateEmails.Service.Application
{
    public class BaseManager
    {

        // private DateTime Now { get { return DateTime.Now; } }
        private IDBManager DBManager { get; set; }
        // public CalculationDayDB TodayCalculationDetails { get; set; }

        private MapperConfiguration mapperConfiguration;

        public BaseManager()
        {

            //FillTodaysCalculationDetails();

            DBManager = AutofacContainer.Container.Resolve<IDBManager>();
            mapperConfiguration = new MapperConfiguration(config => config.CreateMap<CalculationDayDB, CalculationDay>());

        }

        private void WriteToEventLog(string message)
        {
            string sSource;
            //string sLog;
            //string sEvent;
            string applicationName = "CalculateEmails";
            sSource = "MailManager";
            //sLog = "Application";
            //sEvent = "Sample Event";

            message = message + " " + DateTime.Now.ToLongTimeString();

            if (!EventLog.SourceExists(applicationName))
            {
                EventLog.CreateEventSource(sSource, applicationName);
            }

            EventLog.WriteEntry(sSource, message);
            //EventLog.WriteEntry(sSource, message, EventLogEntryType.Warning, 234);
        }

        public static void WriteToLog(string message)
        {
            int x = Thread.CurrentThread.ManagedThreadId;
            string messagec = $"Thread:{x}; {message}";

            Debug.WriteLine(messagec);
            Console.WriteLine(messagec);
            File.AppendAllText("D:\\perls.txt", messagec + Environment.NewLine);
        }

        private static readonly object padlock = new object();

        protected void PerformChange(DateTime date, Action<CalculationDayDB> a)
        {
            lock (padlock)
            {
                // DBManager.UpdateCalculationDay(a, Now);
                var CalculationDayDB = FillTodaysCalculationDetails(date);
                //WriteToLog($"PerformChange Over A= TodayCalculationDetails.MailCountAdd: {CalculationDayDB.MailCountAdd}");
                a(CalculationDayDB);
                //WriteToLog($"PerformChange Under A= TodayCalculationDetails.MailCountAdd: {CalculationDayDB.MailCountAdd}");
                SaveDetailList(CalculationDayDB);
                // UpdateLabel();
            }
        }

        public CalculationDay GetLastCalculationDay(DateTime date)
        {
            var mapper = mapperConfiguration.CreateMapper();
            CalculationDayDB resultDB = FillTodaysCalculationDetails(date);
            CalculationDay result = mapper.Map<CalculationDayDB, CalculationDay>(resultDB);
            return result;
        }

        public List<CalculationDay> GetCalcuationDays(DateTime startDay, DateTime endDay)
        {
            var list = DBManager.GetCalculationDays(startDay, endDay);
            var mapper = mapperConfiguration.CreateMapper();
            var result = mapper.Map<List<CalculationDayDB>, List<CalculationDay>>(list);
            return result;
        }

        private CalculationDayDB FillTodaysCalculationDetails(DateTime date)
        {
            var list = DBManager.GetCalculationDays(date, date);
            if (list.Any())
            {
                return list.Single();
            }
            else
            {
                return new CalculationDayDB() { Date = date };
            }
        }

        //private void FillDetailList()
        //{
        //    Outlook.NoteItem noteItem = CheckCounterContainer();
        //    string body = noteItem.Body.Replace(CalculateEmails, "").Trim();
        //    Details = body.Deserialize<List<CalculationDetails>>();

        //    if (Details == null)
        //    {
        //        Details = new List<CalculationDetails>();
        //    }
        //    var currentDay = Details.FirstOrDefault(x => x.Date == CurrentDay);
        //    if (currentDay == null)
        //    {
        //        Details.Add(new CalculationDetails() { Date = DateTime.Today, MailCountAdd = 0 });
        //    }
        //}

        //private void SaveDetailList()
        //{
        //    Outlook.NoteItem noteItem = CheckCounterContainer();
        //    noteItem.Body = CalculateEmails + Environment.NewLine + Details.Serialize();
        //    noteItem.Save();
        //}

        private static readonly object padloc = new object();

        private void SaveDetailList(CalculationDayDB TodayCalculationDetails)
        {
            lock (padloc)
            {
                //  WriteToLog($"TodayCalculationDetails.MailCountAdd: {TodayCalculationDetails.MailCountAdd}");
                DBManager.SaveTodayCalculationDay(TodayCalculationDetails);
            }
        }

    }
}
