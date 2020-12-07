﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace NRZ.Services.Interfaces
{
    public interface IHangfireJob
    {
        /// <summary>
        /// The method can be used in unit tests or can be reused when the interface is injected to other places. 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Run(IJobCancellationToken token);

        /// <summary>
        /// The method will be used in Hangfire job scheduler.
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        Task RunAtTimeOf(DateTime now);
    }
}
