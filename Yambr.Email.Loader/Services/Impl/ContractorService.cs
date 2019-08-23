using System.Threading.Tasks;
using Yambr.Email.Common.Models;

namespace Yambr.Email.Loader.Services.Impl
{
    /// <summary>
    /// Сервис работы с контрагентами
    /// </summary>
    public class ContractorService : IContractorService
    {
        /// <summary>
        /// Источник яндекс
        /// </summary>
        public const string YandexSource = "yandex";
        /// <summary>
        /// Источник дадата
        /// </summary>
        public const string DadataSource = "dadata";
        /*
        private readonly IRecordCollection<Contractor> _contractorRecordCollection;
        private readonly IPublicDomainService _publicDomainService;
        private readonly ISuggestClient _suggestClient;
        private readonly IBusinessClient _businessClient;
        private readonly ILanguageIdentifierService _languageIdentifierService;


        public ContractorService(
            IRecordCollection<Contractor> contractorRecordCollection,
            IPublicDomainService publicDomainService,
            ISuggestClient suggestClient,
            IBusinessClient businessClient,
            ILanguageIdentifierService languageIdentifierService)
        {
            _contractorRecordCollection = contractorRecordCollection;
            _publicDomainService = publicDomainService;
            _suggestClient = suggestClient;
            _businessClient = businessClient;
            _languageIdentifierService = languageIdentifierService;
        }

        #region Получение и обновление

        */

        /// <summary>
        /// Получить или создать контрагента по домену
        /// </summary>
        /// <param name="domainString"></param>
        /// <returns></returns>
        public async Task<ContractorSummary> GetOrCreateContractorSummaryByDomainAsync(string domainString)
        {
            /*
             TODO Cache
            var contractor = await GetOrCreateContractorByDomainAsync(domainString);
            if (contractor == null) return null;
            await CreateOrUpdateContractorByDomain(domainString, contractor);
            */
            return new ContractorSummary()
            {
                Name = domainString
            };
        }
        /*
        private async Task CreateOrUpdateContractorByDomain(string domainString, Contractor contractor)
        {
            var filterDefinitionBuilder = new FilterDefinitionBuilder<Contractor>();
            await _contractorRecordCollection.CreateOrUpdateOneAsync(contractor, filterDefinition: filterDefinitionBuilder.ElemMatch(c => c.Domains, domain => domain.DomainString == domainString));
        }

        /// <summary>
        /// Получить или создать контрагента по домену
        /// </summary>
        /// <param name="domainString"></param>
        /// <returns></returns>
        public async Task<Contractor> GetOrCreateContractorByDomainAsync(string domainString)
        {
            if (string.IsNullOrWhiteSpace(domainString)) return null;
            // если домен публичный то сразу вернем пусто
            if (await _publicDomainService.IsPublicDomainAsync(domainString)) return null;
            //если нет сначала поищем у себя в бд по домену

            return
                await GetContractorByDomainAsync(domainString) ?? // Попробуем получить по домену
                await CreateContractorByDomainAsync(domainString);//не получится создадим по домену
        }

        /// <summary>
        /// Создать контрагента по домену
        /// </summary>
        /// <param name="domainString"></param>
        /// <returns></returns>
        public async Task<Contractor> CreateContractorByDomainAsync(string domainString)
        {
            //запросим в яндекс
            var businessModel = await _businessClient.QueryAsync(domainString);//не ограничиваем ответ
            //если ответ есть 
            var contractorResponse = businessModel?.features?.FirstOrDefault(c =>
            !string.IsNullOrWhiteSpace(c.properties?.CompanyMetaData?.Url) &&
            c.properties.CompanyMetaData.Url.Contains(domainString));//берем первую у которой такой сайт установлен
            var contractor = contractorResponse?.properties?.CompanyMetaData != null ?
                await GetOrCreateContractorByBusinessModelAsync(contractorResponse) ://создадим на основе того что вернул яндекс и подгрузим из того что у нас есть
                new Contractor
                {
                    Name = domainString
                };
            //добавим домен
            AddDomain(contractor, domainString);
            return contractor;
        }

        /// <summary>
        /// Получить или создать контрагента по результату яндекса
        /// </summary>
        /// <param name="businessModel"></param>
        /// <returns></returns>
        public async Task<Contractor> GetOrCreateContractorByBusinessModelAsync(Feature businessModel)
        {
            var contractor =
                await GetContractorByYandexIdAsync(businessModel.properties.CompanyMetaData.id) ??//попроуем получить по результату из яндекса
                new Contractor // если не нашли то создадим
                {
                    Name = businessModel.properties.name,
                    Address = businessModel.properties.CompanyMetaData.Address,
                    YandexId = businessModel.properties.CompanyMetaData.id
                };
            AddSource(contractor, YandexSource);
            FillByFeature(businessModel, contractor);
            await UpdateDadataAsync(contractor);

            return contractor;
        }
        /// <summary>
        /// Обновить данные на основе дадаты
        /// </summary>
        /// <param name="contractor"></param>
        /// <returns></returns>
        public async Task UpdateDadataAsync(Contractor contractor)
        {
            // если название компании пустое или  = домен то нет смысла лезть в дадату
            if (string.IsNullOrWhiteSpace(contractor.Name) || contractor.Domains.Any(c => c.DomainString == contractor.Name)) return;

            if (contractor.Sources.All(c => c.Name != DadataSource) || //если в источниках нет дадаты
                contractor.LastModified < DateTime.UtcNow.AddDays(-1)) //или дата изменения больше суток то нужно запросить данные из дадаты
            {
                var suggestion = await GetSuggestionByContractorAsync(contractor);
                if (suggestion != null)
                {
                    //заполним источник
                    AddSource(contractor, DadataSource);
                    FillByDadata(contractor, suggestion.data);
                }
            }
        }

        private async Task<SuggestPartyResponse.Suggestions> GetSuggestionByContractorAsync(IContractor contractor)
        {
            var partyResponse = await GetMostRelevantResopnse(contractor);
            // если есть хотя бы один ответ то сохраним в компанию
            var suggestion = partyResponse?.suggestions?.FirstOrDefault();
            return suggestion;
        }

        private async Task<SuggestPartyResponse> GetMostRelevantResopnse(IContractor contractor)
        {
            var partySuggestQuery = new PartySuggestQuery(contractor.Name) { count = 2 };//два потому что иначе дадата ничего не пришлет

            #region если есть инн и огрн

            //если есть иннн и огрн
            if (!string.IsNullOrWhiteSpace(contractor.INN) || !string.IsNullOrWhiteSpace(contractor.OGRN))
            {
                //то ищем по ним
                partySuggestQuery.query = $"{contractor.Name} {contractor.INN} {contractor.OGRN}";
                return await _suggestClient.QueryPartyAsync(partySuggestQuery);
            }
            #endregion

            #region если нет инн или огрн запросим сначала в дадату как есть вдруг там 1 вариант

            var suggestPartyResponse = await _suggestClient.QueryPartyAsync(partySuggestQuery);
            //если вариант ответа нам не подходит
            if (IsMostRelevantResult(suggestPartyResponse))
            {
                //если один или пусто вернем как есть
                return suggestPartyResponse;
            }

                #endregion

            #region Будем орагничивать по адресу если он еть

            //для запроса с ограниением по адресу этого апдейтим из дадаты адрес
            await UpdateContractorAddressDataAsync(contractor);
            // ограничим поиск по адресу дадаты если он есть
            if (contractor.AddressData == null) return null;
            var locationBounds = new List<AddressData>
            {
                new AddressData
                {
                    kladr_id = contractor.AddressData.region_kladr_id
                }, //ограничиваем поиск по региону (по кладр)
                new AddressData
                {
                    kladr_id = contractor.AddressData.area_kladr_id
                }, //ограничиваем поиск по району в регионе (по кладр)
                new AddressData
                {
                    kladr_id = contractor.AddressData.city_kladr_id
                }, //ограничиваем поиск по городу в районе (по кладр)
                new AddressData
                {
                    kladr_id = contractor.AddressData.settlement_kladr_id
                }, //ограничиваем поиск по населенному пункту в городе (по кладр)
                new AddressData
                {
                    kladr_id = contractor.AddressData.street_kladr_id
                }, //ограничиваем поиск по улиеце в населенном пункте (по кладр)
                new AddressData
                {
                    kladr_id = contractor.AddressData.house_kladr_id
                } //ограничиваем поиск по дому на улице (по кладр)
            };
            locationBounds.RemoveAll(c => c.kladr_id == null);
            foreach (var addressData in locationBounds)
            {
                if (addressData.kladr_id == null) continue;
                var localSuggestPartyResponse = await QueryPartyBoundedByKladr(addressData, partySuggestQuery);
                //проверяем по каждому из ограничений 
                if (IsMostRelevantResult(localSuggestPartyResponse))
                {
                    return localSuggestPartyResponse;
                }
                if (localSuggestPartyResponse?.suggestions != null &&
                    localSuggestPartyResponse.suggestions.Count > 0)
                {
                    //передадим запрос
                    suggestPartyResponse = localSuggestPartyResponse;
                }
            }
            //отдадим последний успешный запрос
            return suggestPartyResponse;
            //если прошли по всем и вполь до офиса ответов два то вернем первый вариант
            #endregion
            //если нет то вернем что ничего не нашли (или т.е. не получили релевантный ответ (т.е. 1))
        }

       private async Task<SuggestPartyResponse> QueryPartyBoundedByKladr(AddressData addressData, PartySuggestQuery partySuggestQuery)
        {
            partySuggestQuery.locations = new[] { addressData };
            return await _suggestClient.QueryPartyAsync(partySuggestQuery);
        }
        /// <summary>
        /// релевантный ответ если получили пусто или 1
        /// </summary>
        /// <param name="suggestPartyResponse"></param>
        /// <returns></returns>
        private static bool IsMostRelevantResult(SuggestPartyResponse suggestPartyResponse)
        {
            return 
                suggestPartyResponse?.suggestions == null || suggestPartyResponse.suggestions.Count <= 1;
        }

        public async Task UpdateContractorAddressDataAsync(IContractor contractor)
        {
            if (!string.IsNullOrWhiteSpace(contractor.Address) &&
                _languageIdentifierService.IsRus(contractor.Address).GetValueOrDefault(false)) 
            {
                var addressSuggestQuery = new AddressSuggestQuery(contractor.Address)
                {
                    count = 2 //два потому что иначе дадата ничего не пришлет
                };
                //сначала получим кладр адреса чтобы ограничить поиск
                var addressResponse = await _suggestClient.QueryAddressAsync(addressSuggestQuery);
                var firstAddressResult = addressResponse?.suggestions?.FirstOrDefault();
                if (firstAddressResult?.data != null) //если удалось получить один результат
                {
                    //заодно заполним в контрагенте адрес полный 
                    contractor.AddressData = firstAddressResult.data;
                }
            }
        }

        #endregion

        #region Заполнение контрагента

        /// <summary>
        /// Добавить домен в контрагента (без сохранения)
        /// </summary>
        /// <param name="contractor"></param>
        /// <param name="domainString"></param>
        public void AddDomain(Contractor contractor, string domainString)
        {
            if (contractor == null) throw new ArgumentNullException(nameof(contractor));
            if (domainString == null) throw new ArgumentNullException(nameof(domainString));
            if (contractor.Domains.All(c => c.DomainString != domainString))
            {
                contractor.Domains.Add(new Domain
                {
                    DomainString = domainString
                });
            }
        }
        /// <summary>
        /// Добавляет источник без сохранения
        /// </summary>
        /// <param name="contractor"></param>
        /// <param name="source"></param>
        public void AddSource(Contractor contractor, string source)
        {
            if (contractor.Sources.All(c => c.Name != source))
            {
                contractor.Sources.Add(new DataSource(source));
            }
        }
        /// <summary>
        /// Заполнить на основе результата яндекса
        /// </summary>
        /// <param name="businessModel"></param>
        /// <param name="contractor"></param>
        private static void FillByFeature(Feature businessModel, IContractor contractor)
        {
            contractor.Categories = businessModel.properties.CompanyMetaData.Categories;
            contractor.Features = businessModel.properties.CompanyMetaData.Features;
            contractor.Geometry = businessModel.geometry;
            contractor.Hours = businessModel.properties.CompanyMetaData.Hours;
            contractor.Links = businessModel.properties.CompanyMetaData.Links;
            contractor.Site = businessModel.properties.CompanyMetaData.Url;
            //заполним телефоны если есть
            if (businessModel.properties.CompanyMetaData.Phones == null) return;
            foreach (var phone in businessModel.properties.CompanyMetaData.Phones)
            {
                AddPhone(contractor, phone);
            }
        }

        public static void AddPhone(IContractor contractor, Yandex.Business.Models.Phone phone)
        {
            if (contractor.Phones.All(c => c.PhoneString != phone.PhoneString))
            {
                contractor.Phones.Add(new Phone(phone.PhoneString));
            }
        }

        /// <summary>
        ///  Заполнить на основе результата дадаты
        /// </summary>
        /// <param name="contractor"></param>
        /// <param name="partyData"></param>
        public static void FillByDadata(IContractor contractor, IPartyData partyData)
        {
            if (partyData == null) return;
            if (partyData.Address?.data != null)
            {
                contractor.AddressData = partyData.Address.data;
            }
            contractor.BranchCount = partyData.BranchCount;
            contractor.BranchType = partyData.BranchType;
            contractor.INN = partyData.INN;
            contractor.KPP = partyData.KPP;
            contractor.OGRN = partyData.OGRN;
            contractor.Okpo = partyData.Okpo;
            contractor.Okved = partyData.Okved;
            contractor.Opf = partyData.Opf;
            contractor.Management = partyData.Management;
            contractor.PartyName = partyData.Name;
            contractor.State = partyData.State;
            contractor.Type = partyData.Type;
        }

        #endregion

        #region Загрузка из БД

        /// <summary>
        /// Получить контрагента по id яндекса (из бд)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Contractor> GetContractorByYandexIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            var cursor =
                await _contractorRecordCollection.FindAsync(
                    record => record.YandexId == id,
                    new FindOptions<Contractor>
                    {
                        Limit = 1,
                        BatchSize = 1
                    });
            return !cursor.MoveNext() ? null : cursor.Current.FirstOrDefault();
        }

        /// <summary>
        /// получить контрагента по домену (из бд)
        /// </summary>
        /// <param name="domainString"></param>
        /// <returns></returns>
        public async Task<Contractor> GetContractorByDomainAsync(string domainString)
        {
            if (string.IsNullOrWhiteSpace(domainString)) return null;
            var cursor =
                await _contractorRecordCollection.FindAsync(
                    record => record.Domains.Any(d => d.DomainString == domainString),
                    new FindOptions<Contractor>
                    {
                        Limit = 1,
                        BatchSize = 1
                    });
            return !cursor.MoveNext() ? null : cursor.Current.FirstOrDefault();
        }

        #endregion
        */
    }
}
