using Microsoft.EntityFrameworkCore;
using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(WeatherDbContext context)
        {
            // Garantir que o banco de dados foi criado
            context.Database.EnsureCreated();

            // Verificar se já existem dados
            try
            {
                if (context.Cidades.Any() || context.Dicas.Any())
                {
                    return; // Banco já foi populado
                }
            }
            catch
            {
                // Continuar com a criação dos dados
            }

            // Adicionar cidades de exemplo
            var cidades = new[]
            {
                new Cidade { Nome = "São Paulo", Latitude = -23.5505m, Longitude = -46.6333m },
                new Cidade { Nome = "Rio de Janeiro", Latitude = -22.9068m, Longitude = -43.1729m },
                new Cidade { Nome = "Brasília", Latitude = -15.7942m, Longitude = -47.8822m },
                new Cidade { Nome = "Salvador", Latitude = -12.9714m, Longitude = -38.5014m },
                new Cidade { Nome = "Fortaleza", Latitude = -3.7319m, Longitude = -38.5267m },
                new Cidade { Nome = "Belo Horizonte", Latitude = -19.9167m, Longitude = -43.9345m },
                new Cidade { Nome = "Manaus", Latitude = -3.1190m, Longitude = -60.0217m },
                new Cidade { Nome = "Curitiba", Latitude = -25.4284m, Longitude = -49.2733m }
            };

            context.Cidades.AddRange(cidades);

            // Adicionar dicas preventivas
            var dicas = new[]
            {
                // Dicas para nível normal
                new Dica { NivelRisco = "normal", Mensagem = "Mantenha-se hidratado bebendo água regularmente." },
                new Dica { NivelRisco = "normal", Mensagem = "Use protetor solar ao sair de casa." },
                new Dica { NivelRisco = "normal", Mensagem = "Vista roupas leves e confortáveis." },

                // Dicas para nível alerta
                new Dica { NivelRisco = "alerta", Mensagem = "Aumente a ingestão de líquidos, especialmente água." },
                new Dica { NivelRisco = "alerta", Mensagem = "Evite exposição prolongada ao sol entre 10h e 16h." },
                new Dica { NivelRisco = "alerta", Mensagem = "Procure locais com sombra ou ar condicionado." },
                new Dica { NivelRisco = "alerta", Mensagem = "Use chapéu e óculos de sol ao sair." },
                new Dica { NivelRisco = "alerta", Mensagem = "Reduza atividades físicas intensas ao ar livre." },

                // Dicas para nível risco
                new Dica { NivelRisco = "risco", Mensagem = "ATENÇÃO: Beba água constantemente, mesmo sem sede." },
                new Dica { NivelRisco = "risco", Mensagem = "EVITE sair de casa nos horários de pico de calor (10h às 16h)." },
                new Dica { NivelRisco = "risco", Mensagem = "Permaneça em locais com ar condicionado ou ventilação adequada." },
                new Dica { NivelRisco = "risco", Mensagem = "NÃO pratique atividades físicas ao ar livre." },
                new Dica { NivelRisco = "risco", Mensagem = "Use roupas leves, de cores claras e tecidos naturais." },
                new Dica { NivelRisco = "risco", Mensagem = "Molhe o corpo com água fresca para se refrescar." },
                new Dica { NivelRisco = "risco", Mensagem = "Procure atendimento médico se sentir mal-estar, tontura ou náusea." },
                new Dica { NivelRisco = "risco", Mensagem = "Evite bebidas alcoólicas e com cafeína que podem causar desidratação." }
            };

            context.Dicas.AddRange(dicas);

            // Salvar as alterações
            context.SaveChanges();
        }
    }
}

