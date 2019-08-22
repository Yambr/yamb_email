FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Yambr.Email.Consumer/Yambr.Email.Consumer.csproj", "Yambr.Email.Consumer/"]
RUN dotnet restore "Yambr.Email.Consumer/Yambr.Email.Consumer.csproj"
COPY . .
WORKDIR "/src/Yambr.Email.Consumer"
RUN dotnet build "Yambr.Email.Consumer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Yambr.Email.Consumer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Yambr.Email.Consumer.dll"]