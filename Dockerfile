FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY PokerBunchApi.sln ./
COPY Core/*.csproj ./Core/
COPY Api/*.csproj ./Api/
COPY Infrastructure.Cache/*.csproj ./Infrastructure.Cache/
COPY Infrastructure.Email/*.csproj ./Infrastructure.Email/
COPY Infrastructure.Sql/*.csproj ./Infrastructure.Sql
COPY Tests.Common/*.csproj ./Tests.Common/
COPY Tests.Core/*.csproj ./Tests.Core/

RUN dotnet restore .
COPY . .
WORKDIR /Core
RUN dotnet build -c Release -o /app

WORKDIR /Api
RUN dotnet build -c Release -o /app

WORKDIR /Tests.Common
RUN dotnet build -c Release -o /app

WORKDIR /Core.Tests
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.dll"]

#docker build --t pokerbunch-api .
#docker run -d -p 8080:80 pokerbunch-api