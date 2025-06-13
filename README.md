# 📦 HospisimAPI

---

## ✅ Pré-requisitos

Certifique-se de ter os seguintes itens instalados:

	- .NET SDK 8.0 ou superior
	- Visual Studio 2022 ou Visual Studio Code
	- SQL Server (LocalDB ou instância completa)
---
### 🔁 Clonar o repositório

sh
git clone https://github.com/bessaemily/hospisim.git
cd hospisim

---

## Configuração do banco de dados

Edite o arquivo appsettings.json para configurar a conexão com o banco de dados:

```json 
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HospiSim;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

## Aplique as migrações

sh 
dotnet ef database update

## Execução da API

sh
dotnet run

-------------

## Usando o Visual Studio

1.	Abra o arquivo hospisim.sln no Visual Studio
2.	Pressione F5 ou clique no botão "Executar" para iniciar a aplicação
Por padrão, a API será executada em:
•	https://localhost:5001
•	http://localhost:5000

## Documentação Swagger
Acesse a interface Swagger para testar os endpoints:

https://localhost:5001/swagger
