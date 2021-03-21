# Combined/Common Log Format Apllication (CRUD and Batch Upload)

Projeto full cycle (banco de dados, back end e front end) de CRUD e carregamento em lote via arquivo ".log" (aceita Combined Log Format ou Common Log Format). Seguem abaixo, os formatos aceitos.

Common Log Format
```
132.10.11.5 - gregory [20/Jan/2019:08:44:13 +0100] "POST http://things.example.com/hydrant HTTP/2.0" 204 1313
```

Combined Log Format (Common Log Format + 2 campos: 'Referer' e 'User-agent')
```
221.123.22.151 user-identifier frank [25/Jun/2019:19:32:10 -0800] "GET http://shame.example.com/bear HTTP/1.0" 200 0 "http://mom.com/cave" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"
```

Material de apoio:
https://httpd.apache.org/docs/1.3/logs.html


## Configuração

### Banco de dados (PostgreSQL)

Neste projeto, foi utilizada a porta padrão do PosgreSQL (5432). O nome utilizado para o banco de dados foi `ClfDb`. A tabela utilizada deve ter as seguintes propriedades:
```
CREATE TABLE public."Clf"
(
    "Id" UUID NOT NULL,
    "Client" VARCHAR(15) NOT NULL,
    "RfcIdentity" VARCHAR(50),
    "UserId" VARCHAR(50),
    "RequestDate" TIMESTAMP NOT NULL,
    "RequestTime" TIMETZ NOT NULL,
    "Method" VARCHAR(10) NOT NULL,
   "Request" VARCHAR(200) NOT NULL,
   "Protocol" VARCHAR(4) NOT NULL,
   "StatusCode" INT NOT NULL,
   "ResponseSize" INT,
   "Referrer" VARCHAR(200),
   "UserAgent" VARCHAR(200),
   CONSTRAINT "Clf_pkey" PRIMARY KEY ("Id")
)
```

Scripts para criar o DB e a tabela utilizada:
```
Create_Database_Script.sql
Create_Table_Script.sql
```

### Back End (ClfApi)

As dependências utilizadas no projeto são:
```
Microsoft.AspNetCore.Mvc.NewtonsoftJson (3.1.13)
Microsoft.EntityFrameworkCore (5.0.2)
Microsoft.EntityFrameworkCore.Design (5.0.2)
Microsoft.EntityFrameworkCore.Tools (5.0.2)
Npgsql.EntityFrameworkCore.PostgreSQL (5.0.2)
```

Deve-se certificar que todas as dependências estão instaladas no projeto e nas versões acima.

Caso ao clonar o projeto, ocorram erros relacionados a banco de dados, recomendável que atualize os modelos e contextos com o Banco de Dados para não haver inconsistências de mapeamento. Para isso, deve-se:

Executar o comando abaixo no Package Manager Console
`Scaffold-DbContext -Connection Name=ClfDb Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir "Models" -ContextDir "Context" -force`

Após exeutar o Scaffold, deve-se criar o construtor da classe ClfApi.Models.Clf:
```
public Clf()
{
    Id = Guid.NewGuid();
}
```
Além disso, renomear o `DbSet<Clf>` de `Clves` para `Clfs`.

Conferir se há a injeção de dependência abaixo, no `Startup.ConfigureServices(IServiceCollection services)`:
`services.AddDbContext<ClfDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClfDb")));`

Para que não ocorra erros de conversão do JSON para o modelo, deve-se certificar que a injeção de dependência `services.AddControllers()` seja trocada por `services.AddControllers().AddNewtonsoftJson();`. Isso pois o serializador padrão do .NET tem alguns problemas na conversão de propriedades de tipos relacionados a data, como `DateTimeOffset` ou `DateTimeOffset`.

### Observação

Npgsql converte o `DateTimeOffset` pro UTC local (do servidor) quando mapeado a um `timestamptz` (PostgreSQL), antes de enviar ao DB:
https://www.npgsql.org/doc/types/datetime.html

Por isso, utilizei a abordagem de armazenar o `RequestDate` como `timestamp` (convertendo para UTCDateTime global) e o `RequestTime` como `timetz` (`DateTimeOffset`) para manter a informação do time zone original.

### Front End (ClfClient)

O front end do projeto foi feito com Angular (com Typescript) e para o desenvolvimento, foi necessária a instalação do pacote `moment`.
Seguem as configurações utilizadas no front end:

```
     _                      _                 ____ _     ___ 
    / \   _ __   __ _ _   _| | __ _ _ __     / ___| |   |_ _|
   / △ \ | '_ \ / _` | | | | |/ _` | '__|   | |   | |    | | 
  / ___ \| | | | (_| | |_| | | (_| | |      | |___| |___ | | 
 /_/   \_\_| |_|\__, |\__,_|_|\__,_|_|       \____|_____|___|
                |___/
    

Angular CLI: 8.3.29
Node: 12.17.0      
OS: win32 x64
Angular: 8.2.14
... animations, common, compiler, compiler-cli, core, forms
... language-service, platform-browser, platform-browser-dynamic
... router

Package                            Version
------------------------------------------------------------
@angular-devkit/architect          0.803.29
@angular-devkit/build-angular      0.803.29
@angular-devkit/build-optimizer    0.803.29
@angular-devkit/build-webpack      0.803.29
@angular-devkit/core               8.3.29
@angular-devkit/schematics         8.3.29
@angular/cdk                       8.2.3
@angular/cli                       8.3.29
@angular/material                  8.2.3
@angular/material-moment-adapter   11.2.5
@ngtools/webpack                   8.3.29
@schematics/angular                8.3.29
@schematics/update                 0.803.29
rxjs                               6.4.0
typescript                         3.5.3
webpack                            4.39.2
```
