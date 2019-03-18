FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY SERVICE-B/SERVICE-B.csproj SERVICE-B/
RUN dotnet restore SERVICE-B/SERVICE-B.csproj
COPY . .
WORKDIR /src/SERVICE-B
RUN dotnet build SERVICE-B.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SERVICE-B.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SERVICE-B.dll"]
