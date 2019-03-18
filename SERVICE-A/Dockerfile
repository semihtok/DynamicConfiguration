FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY SERVICE-A/SERVICE-A.csproj SERVICE-A/
COPY DynamicConfiguration.Core/DynamicConfiguration.Core.csproj DynamicConfiguration.Core/
COPY DynamicConfiguration.Data.Model/DynamicConfiguration.Data.Model.csproj DynamicConfiguration.Data.Model/
COPY DynamicConfiguration.Data/DynamicConfiguration.Data.csproj DynamicConfiguration.Data/
RUN dotnet restore SERVICE-A/SERVICE-A.csproj
COPY . .
WORKDIR /src/SERVICE-A
RUN dotnet build SERVICE-A.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SERVICE-A.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SERVICE-A.dll"]
