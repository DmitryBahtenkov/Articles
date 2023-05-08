FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AutofacApi/AutofacApi.csproj", "AutofacApi/"]
RUN dotnet restore "AutofacApi/AutofacApi.csproj"
COPY . .
WORKDIR "/src/AutofacApi"
RUN dotnet build "AutofacApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutofacApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutofacApi.dll"]
