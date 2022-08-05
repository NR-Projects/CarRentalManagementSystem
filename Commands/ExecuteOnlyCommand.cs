﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Commands
{
    public class ExecuteOnlyCommand : CommandBase
    {
        private readonly Action<object> _ActionToExecute;

        public ExecuteOnlyCommand(Action<object> action)
        {
            _ActionToExecute = action;
        }

        public override void Execute(object parameter) => _ActionToExecute(parameter);
    }
}
