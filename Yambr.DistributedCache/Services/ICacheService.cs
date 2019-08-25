using System;
using System.Threading.Tasks;

namespace Yambr.DistributedCache.Services
{
    /// <summary>
    /// Интерфейс работы с кэшем
    /// </summary>
    public interface ICacheService
    {
        
        /// <summary>
        /// Добавить или изменить элемент кэша по ключу с указанием зависимости и времени кеширования
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="region">Регион</param>
        /// <param name="cacheDuration">Длительность хранения значения в кэше</param>
        void Insert<T>(string key, T value, string region, TimeSpan cacheDuration);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу с указанием времени кеширования
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="cacheDuration">Длительность хранения значения в кэше</param>
        void Insert<T>(string key, T value, TimeSpan cacheDuration);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу c зависимостью
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="region">Регион</param>
        void Insert<T>(string key, T value, string region);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        void Insert<T>(string key, T value);

        /// <summary>
        /// Получить элемент из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <returns>Значение</returns>
        T Get<T>(string key);

        /// <summary>
        /// Получить элемент из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="region">Регион</param>
        /// <returns>Значение</returns>
        T Get<T>(string key, string region);

        /// <summary>
        /// Удалить элемент из кэша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        void Remove(string key);

        /// <summary>
        /// Удалить элемент из кэша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="region">Регион</param>
        void Remove(string key, string region);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу с указанием зависимости и времени кеширования
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="region">Регион</param>
        /// <param name="cacheDuration">Длительность хранения значения в кэше</param>
        Task InsertAsync<T>(string key, T value, string region, TimeSpan cacheDuration);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу с указанием времени кеширования
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="cacheDuration">Длительность хранения значения в кэше</param>
        Task InsertAsync<T>(string key, T value, TimeSpan cacheDuration);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу c зависимостью
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        /// <param name="region">Регион</param>
        Task InsertAsync<T>(string key, T value, string region);

        /// <summary>
        /// Добавить или изменить элемент кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Кэшируемый элемент</param>
        Task InsertAsync<T>(string key, T value);

        /// <summary>
        /// Получить элемент из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <returns>Значение</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Получить элемент из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип кэшируемого значения в кэше</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="region">Регион</param>
        /// <returns>Значение</returns>
        Task<T> GetAsync<T>(string key, string region);

        /// <summary>
        /// Удалить элемент из кэша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Удалить элемент из кэша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="region">Регион</param>
        Task RemoveAsync(string key, string region);
    }
}
