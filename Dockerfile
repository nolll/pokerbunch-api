FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
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

COPY . ./
RUN dotnet test Tests.Core -c Release
RUN dotnet test Tests.Integration -c Release
RUN dotnet publish Api -c Release -o /out

FROM base AS final
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Api.dll"]

#docker build --t pokerbunch-api .
#docker run -d -p 8080:80 pokerbunch-api
