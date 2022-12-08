using System;

namespace LanDocs.Indexing.IntegrationTests.Configuration
{
    public class IndexingIntegrationTestsApiSettings
    {
        /// <summary>
        /// Адрес api интеграционных тестов индексации.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт api интеграционных тестов индексации.
        /// </summary>
        public int Port { get; set; }
    }
}
