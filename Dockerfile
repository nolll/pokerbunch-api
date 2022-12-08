FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY PokerBunchApi.sln ./
COPY Core/*.csproj ./Core/
COPY Infrastructure.Cache/*.csproj ./Infrastructure.Cache/
COPY Infrastructure.Email/*.csproj ./Infrastructure.Email/
COPY Infrastructure.Sql/*.csproj ./Infrastructure.Sql/
COPY Api/*.csproj ./Api/
COPY Tests.Common/*.csproj ./Tests.Common/
COPY Tests.Core/*.csproj ./Tests.Core/
COPY Tests.Integration/*.csproj ./Tests.Integration/

RUN dotnet restore .
COPY . .
WORKDIR /Core
RUN dotnet build -c Release -o /app

WORKDIR /Infrastructure.Cache
RUN dotnet build -c Release -o /app

WORKDIR /Infrastructure.Email
RUN dotnet build -c Release -o /app

WORKDIR /Infrastructure.Sql
RUN dotnet build -c Release -o /app

WORKDIR /Api
RUN dotnet build -c Release -o /app

WORKDIR /Tests.Common
RUN dotnet build -c Release -o /app

WORKDIR /Tests.Core
RUN dotnet build -c Release -o /app

WORKDIR /Tests.Integration
RUN dotnet build -c Release -o /app

WORKDIR /Tests.Core
RUN dotnet test

#WORKDIR /Tests.Integration
#RUN dotnet test

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.dll"]

#docker build --t pokerbunch-api .
#docker run -d -p 8080:80 pokerbunch-api