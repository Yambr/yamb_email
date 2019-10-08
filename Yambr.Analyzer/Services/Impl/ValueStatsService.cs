using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yambr.DistributedCache.Services;
using Yambr.SDK.ComponentModel;

namespace Yambr.Analyzer.Services.Impl
{
    [Service]
    class ValueStatsService : IValueStatsService
    {
        public const string StatsRegion = "ValueStats";

        private readonly ICacheService _cacheService;

        public ValueStatsService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        private const long RussianBall = 1000;
        private const long DefaultBall = 5;
        private const long PartOfEmailBall = 1;

        public async Task UpdateStatsAsync<T>(T contract, string keyParamValue)
            where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
           
            var stats =
                await _cacheService.GetAsync<Dictionary<string, Dictionary<string, long>>>(keyParamValue, StatsRegion) ??
                new Dictionary<string, Dictionary<string, long>>();

            foreach (var property in properties)
            {
                var value = property.GetValue(contract);
              
                if (value != null)
                {
                    UpdateValueStat(stats, property.Name, value, keyParamValue);
                }

                if (stats.TryGetValue(property.Name, out var actualStat))
                {
                    var valuePair = actualStat.OrderByDescending(c => c.Value)
                        .FirstOrDefault();
                    var valuePairKey = Convert.ChangeType(valuePair.Key, property.PropertyType);
                    property.SetValue(contract, valuePairKey);
                }
            }

            await _cacheService.InsertAsync(keyParamValue, stats, StatsRegion);
        }

        private void UpdateValueStat(Dictionary<string, Dictionary<string, long>> stats, string propertyName, object getValue, string keyParamValue)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (getValue == null) throw new ArgumentNullException(nameof(getValue));
            if (!stats.TryGetValue(propertyName, out var paramStats))
            {
                paramStats = new Dictionary<string, long>();
                stats.Add(propertyName, paramStats);
            }

            var stringVal = getValue.ToString();
            if (string.IsNullOrWhiteSpace(stringVal)) return;

            if (!paramStats.TryGetValue(stringVal, out var ball))
            {
                ball = 0;
            }

            if (HasRussian(stringVal))
            {
                ball += RussianBall;
            }
            else if (keyParamValue.Contains(stringVal))
            {
                ball += PartOfEmailBall;
            }
            else
            {
                ball += DefaultBall;
            }

            paramStats[stringVal] = ball;
        }

        private static bool HasRussian(string name)
        {
            return name != null &&
                   name.Any(c => (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё');
        }
    
    }
}
