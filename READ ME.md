# Weather Alert API

## Visão Geral

Este projeto implementa uma API em C# e ASP.NET Core para consulta de previsão de temperatura e detecção de risco de calor extremo. Com base na previsão para os próximos sete dias, a API identifica dias com temperaturas elevadas e, em caso de risco, fornece dicas preventivas personalizadas para os usuários. O objetivo principal não é apenas oferecer dados meteorológicos, mas atuar como uma ferramenta de orientação para a saúde pública, alertando sobre condições climáticas perigosas e promovendo a segurança dos usuários.

## Propósito

Em um cenário de mudanças climáticas e eventos extremos cada vez mais frequentes, a exposição ao calor excessivo representa um risco significativo à saúde. Esta API visa mitigar esses riscos fornecendo informações proativas e recomendações de segurança. Ao integrar dados de previsão do tempo com uma lógica de detecção de risco baseada em thresholds configuráveis (ex: 35°C para calor extremo), o sistema pode alertar os usuários sobre dias críticos e sugerir ações preventivas como hidratação, evitar exposição solar em horários de pico e procurar ambientes ventilados. A personalização das dicas torna a orientação mais relevante e eficaz para o bem-estar dos indivíduos.



## Funcionalidades

A API oferece as seguintes funcionalidades principais:

*   **Previsão de Temperatura:** Consulta a previsão de temperatura máxima e mínima para os próximos sete dias em uma cidade específica, utilizando a API Open-Meteo.
*   **Detecção de Risco de Calor Extremo:** Analisa os dados da previsão para identificar dias em que a temperatura máxima excede um limite predefinido (por exemplo, 35°C), classificando-os como dias de 'risco' ou 'alerta'.
*   **Dicas Preventivas Personalizadas:** Com base no nível de risco detectado, a API fornece um conjunto de dicas preventivas personalizadas, como:
    *   Beber mais água e manter-se hidratado.
    *   Evitar exposição direta ao sol, especialmente entre 10h e 16h.
    *   Procurar locais frescos e ventilados.
    *   Não praticar atividades físicas intensas nos horários de pico de calor.
    *   Usar roupas leves e claras.
*   **Gerenciamento de Cidades:** Permite cadastrar, consultar e buscar cidades, associando-as a coordenadas geográficas para a consulta da previsão do tempo.
*   **Gerenciamento de Dicas:** Possibilita a manutenção de um banco de dados de dicas preventivas, categorizadas por nível de risco (normal, alerta, risco).
*   **Alertas Ativos:** Fornece uma lista de alertas de calor extremo ativos para todas as cidades cadastradas que apresentem risco nos próximos dias.



## Arquitetura e Tecnologias

O projeto é construído utilizando as seguintes tecnologias e padrões:

*   **C# e ASP.NET Core 8:** Framework robusto e de alto desempenho para a construção da API web.
*   **Entity Framework Core:** ORM (Object-Relational Mapper) para interação com o banco de dados, permitindo o uso de SQLite para desenvolvimento e Oracle para produção.
*   **Open-Meteo API:** Serviço externo utilizado para obter dados de previsão do tempo. A integração é feita através de um `HttpClient` configurado para consumir a API.
*   **Swagger/OpenAPI:** Documentação interativa da API, gerada automaticamente, facilitando o entendimento e o teste dos endpoints.
*   **Injeção de Dependência:** Utilizada para gerenciar as dependências entre os serviços e controladores, promovendo um código mais modular e testável.
*   **Padrão de Repositório e Serviço:** A lógica de negócio é separada em camadas de serviço, que interagem com os repositórios (neste caso, o `DbContext` do Entity Framework) para acesso aos dados.
*   **CORS (Cross-Origin Resource Sharing):** Configurado para permitir requisições de diferentes origens, facilitando a integração com aplicações frontend.

### Estrutura do Projeto

O projeto segue uma estrutura de API RESTful, organizada em camadas para clareza e manutenção:

*   **Controllers:** Contêm os endpoints da API, responsáveis por receber as requisições HTTP, chamar os serviços apropriados e retornar as respostas.
    *   `CitiesController.cs`: Gerencia operações CRUD para cidades.
    *   `TipsController.cs`: Gerencia operações para dicas preventivas.
    *   `WeatherController.cs`: Lida com a previsão do tempo, alertas de calor e análise de risco.
*   **Services:** Contêm a lógica de negócio da aplicação, orquestrando as operações e interagindo com fontes de dados ou APIs externas.
    *   `CityService.cs`: Lógica de negócio para cidades.
    *   `HeatRiskService.cs`: Lógica para determinar o nível de risco de calor extremo.
    *   `OpenMeteoService.cs`: Cliente para a API Open-Meteo.
    *   `TipsService.cs`: Lógica de negócio para dicas preventivas.
    *   `WeatherService.cs`: Orquestra a obtenção da previsão, análise de risco e dicas.
*   **Models:** Define as estruturas de dados (POCOs) utilizadas na aplicação, incluindo entidades de banco de dados e modelos de requisição/resposta da API.
    *   `Cidade.cs`: Modelo para a entidade Cidade.
    *   `Dica.cs`: Modelo para a entidade Dica.
    *   `ApiModels.cs`: Modelos de requisição e resposta específicos da API (ex: `HeatRiskAnalysisRequest`, `TipsRecommendationResponse`).
    *   `OpenMeteoModels.cs`: Modelos para mapear as respostas da API Open-Meteo.
    *   `WeatherModels.cs`: Modelos para a previsão do tempo e alertas (ex: `WeatherForecast`, `WeatherResponse`).
*   **Data:** Contém o `DbContext` do Entity Framework (`WeatherDbContext.cs`) e as configurações de migração, além da classe `SeedData.cs` para popular o banco de dados inicial.
*   **Program.cs:** Arquivo de inicialização da aplicação, onde são configurados os serviços, o pipeline de requisições HTTP e o Entity Framework.



## Como Configurar e Rodar o Projeto Localmente

Para configurar e executar a Weather Alert API em seu ambiente local, siga os passos abaixo:

### Pré-requisitos

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
*   Um editor de código como [Visual Studio Code](https://code.visualstudio.com/) com a extensão C# ou [Visual Studio](https://visualstudio.microsoft.com/vs/).
*   Opcional: Uma ferramenta para testar APIs, como [Postman](https://www.postman.com/) ou [Insomnia](https://insomnia.rest/).

### Passos para Configuração

1.  **Clone o Repositório:**
    ```bash
    git clone https://github.com/PedroHBergara/csharp-globalsolution.git
    cd csharp-globalsolution/WeatherAlertAPI_code
    ```

2.  **Restaure as Dependências:**
    Navegue até o diretório `WeatherAlertAPI_code` e restaure as dependências do projeto:
    ```bash
    dotnet restore
    ```

3.  **Aplique as Migrações do Banco de Dados:**
    O projeto utiliza SQLite para desenvolvimento. As migrações serão aplicadas automaticamente na primeira execução, mas você pode forçar a criação do banco de dados e a aplicação das migrações manualmente, se necessário:
    ```bash
    dotnet ef database update
    ```
    *Nota: O `SeedData.cs` popula o banco de dados com algumas cidades e dicas iniciais se o banco estiver vazio.*

4.  **Execute a Aplicação:**
    Você pode executar a aplicação diretamente da linha de comando:
    ```bash
    dotnet run
    ```
    A API será iniciada, geralmente em `https://localhost:7042` (a porta pode variar). O console exibirá a URL onde a API está rodando.

5.  **Acesse a Documentação Swagger:**
    Após iniciar a aplicação, você pode acessar a documentação interativa da API via Swagger em `https://localhost:7042/swagger` (ajuste a porta conforme necessário). Lá você encontrará todos os endpoints disponíveis e poderá testá-los diretamente.



## Exemplos de Uso da API

Esta seção demonstra como interagir com os principais endpoints da Weather Alert API.

### 1. Obter Previsão do Tempo e Dicas para uma Cidade

**Endpoint:** `GET /api/Weather/forecast/{cityId}`

Retorna a previsão do tempo para os próximos 7 dias para uma cidade específica, incluindo o nível de risco de calor extremo e dicas preventivas personalizadas.

**Exemplo de Requisição:**

```http
GET /api/Weather/forecast/1 HTTP/1.1
Host: localhost:7042
```

*(Assumindo que `cityId` 1 corresponde a uma cidade cadastrada, por exemplo, São Paulo)*

**Exemplo de Resposta (200 OK):**

```json
{
  "city": {
    "id": 1,
    "nome": "São Paulo",
    "latitude": -23.55,
    "longitude": -46.63
  },
  "forecasts": [
    {
      "date": "2025-06-08T00:00:00",
      "temperatureMax": 36.5,
      "temperatureMin": 25.1,
      "riskLevel": "risco",
      "tips": []
    },
    {
      "date": "2025-06-09T00:00:00",
      "temperatureMax": 32.0,
      "temperatureMin": 22.5,
      "riskLevel": "alerta",
      "tips": []
    }
  ],
  "hasExtremeHeatRisk": true,
  "tips": [
    {
      "id": 3,
      "nivelRisco": "risco",
      "mensagem": "Beba bastante água, mesmo que não sinta sede."
    },
    {
      "id": 4,
      "nivelRisco": "risco",
      "mensagem": "Evite atividades físicas intensas ao ar livre entre 10h e 16h."
    }
  ],
  "lastUpdated": "2025-06-08T12:00:00Z"
}
```

### 2. Obter Todas as Cidades Cadastradas

**Endpoint:** `GET /api/Cities`

Retorna uma lista de todas as cidades cadastradas no sistema.

**Exemplo de Requisição:**

```http
GET /api/Cities HTTP/1.1
Host: localhost:7042
```

**Exemplo de Resposta (200 OK):**

```json
[
  {
    "id": 1,
    "nome": "São Paulo",
    "latitude": -23.55,
    "longitude": -46.63
  },
  {
    "id": 2,
    "nome": "Rio de Janeiro",
    "latitude": -22.90,
    "longitude": -43.20
  }
]
```

### 3. Obter Dicas Preventivas por Nível de Risco

**Endpoint:** `GET /api/Tips/by-risk-level/{riskLevel}`

Retorna dicas preventivas baseadas em um nível de risco específico (`normal`, `alerta`, `risco`).

**Exemplo de Requisição:**

```http
GET /api/Tips/by-risk-level/risco HTTP/1.1
Host: localhost:7042
```

**Exemplo de Resposta (200 OK):**

```json
[
  {
    "id": 3,
    "nivelRisco": "risco",
    "mensagem": "Beba bastante água, mesmo que não sinta sede."
  },
  {
    "id": 4,
    "nivelRisco": "risco",
    "mensagem": "Evite atividades físicas intensas ao ar livre entre 10h e 16h."
  }
]
```



## Contribuição

Contribuições são bem-vindas! Se você tiver sugestões, melhorias ou encontrar algum bug, sinta-se à vontade para abrir uma *issue* ou enviar um *pull request* no repositório do GitHub.

## Licença

Este projeto está licenciado sob a [MIT License](https://opensource.org/licenses/MIT).

