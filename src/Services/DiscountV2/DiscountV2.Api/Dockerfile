#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/DiscountV2/DiscountV2.Api/DiscountV2.Api.csproj", "Services/DiscountV2/DiscountV2.Api/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "Services/DiscountV2/DiscountV2.Api/DiscountV2.Api.csproj"
COPY . .
WORKDIR "/src/Services/DiscountV2/DiscountV2.Api"
RUN dotnet build "DiscountV2.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountV2.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountV2.Api.dll"]