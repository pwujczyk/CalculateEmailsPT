﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.CalculateEmails.Server.JsonFormatter
{
    public class ClientJsonDateFormatterBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            // throw new NotImplementedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Formatter = new ResponseJsonFormatter(operationDescription, dispatchOperation.Formatter);
        }

        public void Validate(OperationDescription operationDescription)
        {
            // throw new NotImplementedException();
        }
    }
}
