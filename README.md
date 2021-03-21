# clf-crud
Common Log Format Apllication (CRUD and Batch Upload)

## Configuração

As dependências utilizadas no projeto são:
`Microsoft.AspNetCore.Mvc.NewtonsoftJson (3.1.13)`
`Microsoft.EntityFrameworkCore (5.0.2)`
`Microsoft.EntityFrameworkCore.Design (5.0.2)`
`Microsoft.EntityFrameworkCore.Tools (5.0.2)`
`Npgsql.EntityFrameworkCore.PostgreSQL (5.0.2)`

Deve-se certificar que todas as dependências estão instaladas no projeto e nas versões acima.

Caso ao clonar o projeto, ocorram erros relacionados a banco de dados, recomendável que atualize os modelos e contextos com o Banco de Dados para não haver inconsistências de mapeamento. Para isso, deve-se:

Executar o comando abaixo no Package Manager Console
`Scaffold-DbContext -Connection Name=ClfDb Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir "Models" -ContextDir "Context" -force`

Após exeutar o Scaffold, deve-se criar o construtor da classe ClfApi.Models.Clf:
`public Clf()`
`{`
`    Id = Guid.NewGuid();`
`}`

Além disso, renomear o `DbSet<Clf>` de `Clves` para `Clfs`.

Conferir se há a injeção de dependência abaixo, no `Startup.ConfigureServices(IServiceCollection services)`:
`services.AddDbContext<ClfDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClfDb")));`

Para que não ocorra erros de conversão do JSON para o modelo, deve-se certificar que a injeção de dependência `services.AddControllers()` seja trocada por `services.AddControllers().AddNewtonsoftJson();`. Isso pois o serializador padrão do .NET tem alguns problemas na conversão de propriedades de tipos relacionados a data, como `DateTimeOffset` ou `DateTimeOffset`.

## Observação

Npgsql converte o `DateTimeOffset` pro UTC local (do servidor) quando mapeado a um `timestamptz` (PostgreSQL), antes de enviar ao DB:
https://www.npgsql.org/doc/types/datetime.html

Por isso, utilizei a abordagem de armazenar o `RequestDate` como `timestamp` (convertendo para UTCDateTime global) e o `RequestTime` como `timetz` (`DateTimeOffset`) para manter o time zone original.
