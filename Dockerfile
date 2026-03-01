FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

ENV TESTCONTAINERS_HOST_OVERRIDE=host.docker.internal

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
COPY PokerBunchApi.sln ./
COPY Core/*.csproj ./Core/
COPY Infrastructure.Cache/*.csproj ./Infrastructure.Cache/
COPY Infrastructure.Email/*.csproj ./Infrastructure.Email/
COPY Infrastructure.Sql/*.csproj ./Infrastructure.Sql/
COPY Api/*.csproj ./Api/
RUN dotnet restore Api

COPY . ./
RUN dotnet publish Api -c Release -o /out

FROM base AS final
WORKDIR /app
COPY --from=build /out .
CMD ["dotnet", "Api.dll"]

#docker build --t pokerbunch-api .
#docker run -d -p 8080:80 pokerbunch-api
