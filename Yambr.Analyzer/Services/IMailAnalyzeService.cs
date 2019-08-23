using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Services
{
    public interface IMailAnalyzeService
    {
         ICollection<IMailReferent> CommonHeaders(string text);
    }
}
