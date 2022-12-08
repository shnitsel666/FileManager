using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanDocs.Indexing.IntegrationTests.Configuration
{
    public class AppSettings
    {
        /// <summary>
        /// Параметры настройки тестов Индексации.
        /// </summary>
        public IndexingIntegrationTestsApiSettings IndexingIntegrationTestsApiSettings { get; set; } = new();
    }
}
