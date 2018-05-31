﻿using CalculateEmails.Contract;
using CalculateEmails.Contract.ServiceContract;
using CalculateEmails.WCFService;
using Configuration;
using MasterConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CalculateEmails.WindowsService
{
    public partial class PSCalculateEmais : ServiceBase
    {

        ServiceHost host;
        public PSCalculateEmais()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            StartServer();
        }

        protected override void OnStart(string[] args)
        {
            StartServer();
        }

        protected override void OnStop()
        {
            StopServer();
        }

        private void StartServer()
        {
            var binding = new NetMsmqBinding(NetMsmqSecurityMode.None);


            string queneAddress = $".\\private$\\{MConfiguration.Configuration["QueneName"]}";
            if (MessageQueue.Exists(queneAddress) == false)
            {
                MessageQueue.Create(queneAddress, true);
            }

            string address = Config.Address;

            host = new ServiceHost(typeof(CalculateEmailsWCFService));
            host.AddServiceEndpoint(typeof(ICalculateEmailsWCFMQService), binding, address);

            host.Open();
        }

        private void StopServer()
        {
            host.Close();
        }
    }
}