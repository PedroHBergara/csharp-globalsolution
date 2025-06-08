namespace WeatherAlertAPI.Models
{
    /// <summary>
    /// Requisição para análise de risco de calor extremo
    /// </summary>
    public class HeatRiskAnalysisRequest
    {
        /// <summary>
        /// Dados de temperatura para análise
        /// </summary>
        public List<TemperatureData> TemperatureData { get; set; } = new();
    }

    /// <summary>
    /// Dados de temperatura para um dia específico
    /// </summary>
    public class TemperatureData
    {
        /// <summary>
        /// Data da previsão
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Temperatura máxima em graus Celsius
        /// </summary>
        public decimal MaxTemperature { get; set; }

        /// <summary>
        /// Temperatura mínima em graus Celsius
        /// </summary>
        public decimal MinTemperature { get; set; }
    }

    /// <summary>
    /// Resposta da análise de risco de calor extremo
    /// </summary>
    public class HeatRiskAnalysisResponse
    {
        /// <summary>
        /// Indica se há risco extremo de calor
        /// </summary>
        public bool HasExtremeHeatRisk { get; set; }

        /// <summary>
        /// Lista de previsões processadas com nível de risco
        /// </summary>
        public List<WeatherForecast> Forecasts { get; set; } = new();
    }

    /// <summary>
    /// Resposta de recomendações de dicas
    /// </summary>
    public class TipsRecommendationResponse
    {
        /// <summary>
        /// Temperatura analisada
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// Nível de risco identificado
        /// </summary>
        public string RiskLevel { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do nível de risco
        /// </summary>
        public string RiskDescription { get; set; } = string.Empty;

        /// <summary>
        /// Lista de dicas recomendadas
        /// </summary>
        public List<string> Tips { get; set; } = new();
    }

    /// <summary>
    /// Resposta de estatísticas das dicas
    /// </summary>
    public class TipsStatisticsResponse
    {
        /// <summary>
        /// Total de dicas cadastradas
        /// </summary>
        public int TotalTips { get; set; }

        /// <summary>
        /// Quantidade de dicas por nível de risco
        /// </summary>
        public Dictionary<string, int> TipsByRiskLevel { get; set; } = new();

        /// <summary>
        /// Data da última atualização
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Alerta meteorológico
    /// </summary>
    public class WeatherAlert
    {
        /// <summary>
        /// ID da cidade
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Nome da cidade
        /// </summary>
        public string CityName { get; set; } = string.Empty;

        /// <summary>
        /// Nível de risco do alerta
        /// </summary>
        public string RiskLevel { get; set; } = string.Empty;

        /// <summary>
        /// Temperatura máxima prevista
        /// </summary>
        public decimal MaxTemperature { get; set; }

        /// <summary>
        /// Data do alerta
        /// </summary>
        public DateTime AlertDate { get; set; }

        /// <summary>
        /// Mensagem do alerta
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}

