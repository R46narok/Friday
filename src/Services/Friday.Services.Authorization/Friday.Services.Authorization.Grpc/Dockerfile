FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Friday.Services.Authorization/Friday.Services.Authorization.Grpc/Friday.Services.Authorization.Grpc.csproj", "Friday.Services.Authorization.Grpc/"]
RUN dotnet restore "src/Services/Friday.Services.Authorization/Friday.Services.Authorization.Grpc/Friday.Services.Authorization.Grpc.csproj"
COPY . .
WORKDIR "/src/Friday.Services.Authorization.Grpc"
RUN dotnet build "Friday.Services.Authorization.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Friday.Services.Authorization.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Friday.Services.Authorization.Grpc.dll"]
