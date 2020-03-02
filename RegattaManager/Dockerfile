#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY [".", "RegattaManager/"]
RUN dotnet restore "RegattaManager/RegattaManager.csproj"
#COPY . .
WORKDIR "/src/RegattaManager"
RUN dotnet build "RegattaManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegattaManager.csproj" -c Release -o /app/publish

FROM base AS final

# Set the locale
#RUN locale-gen de_DE.UTF-8 
ENV LANG de_DE.UTF-8
ENV LANGUAGE de_DE:de
ENV LC_ALL de_DE.UTF-8

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegattaManager.dll"]
