﻿using CalculateEmails.WindowsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateEmails.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            PSCalculateEmais service = new PSCalculateEmais();
            service.OnDebug();
            Console.WriteLine("Host started");
            Console.Read();
        }
    }
}