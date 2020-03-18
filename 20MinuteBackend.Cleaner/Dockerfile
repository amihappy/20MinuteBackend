#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["20MinuteBackend.Cleaner/20MinuteBackend.Cleaner.csproj", "20MinuteBackend.Cleaner/"]
COPY ["20MinuteBackend.Infrastructure/20MinuteBackend.Infrastructure.csproj", "20MinuteBackend.Infrastructure/"]
COPY ["20MinuteBackend.Domain/20MinuteBackend.Domain.csproj", "20MinuteBackend.Domain/"]
RUN dotnet restore "20MinuteBackend.Cleaner/20MinuteBackend.Cleaner.csproj"
COPY . .
WORKDIR "/src/20MinuteBackend.Cleaner"
RUN dotnet build "20MinuteBackend.Cleaner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "20MinuteBackend.Cleaner.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "20MinuteBackend.Cleaner.dll"]