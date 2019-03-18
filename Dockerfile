FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY DynamicConfiguration.UI/DynamicConfiguration.UI.csproj DynamicConfiguration.UI/
COPY DynamicConfiguration.Data/DynamicConfiguration.Data.csproj DynamicConfiguration.Data/
COPY DynamicConfiguration.Data.Model/DynamicConfiguration.Data.Model.csproj DynamicConfiguration.Data.Model/
RUN dotnet restore DynamicConfiguration.UI/DynamicConfiguration.UI.csproj
COPY . .
WORKDIR /src/DynamicConfiguration.UI

RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install

RUN dotnet build DynamicConfiguration.UI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DynamicConfiguration.UI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DynamicConfiguration.UI.dll"]
