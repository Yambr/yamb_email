﻿using System.Collections.Generic;
using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Services
{
    public interface IMailAnalyzeService
    {
         ICollection<IMailReferent> CommonHeaders(string text);

         ICollection<IPersonReferrent> Persons(string text);
    }
}
