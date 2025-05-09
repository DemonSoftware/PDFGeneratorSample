FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PDFGeneratorSample/PDFGeneratorSample.csproj", "PDFGeneratorSample/"]
RUN dotnet restore "PDFGeneratorSample/PDFGeneratorSample.csproj"
COPY . .
WORKDIR "/src/PDFGeneratorSample"
RUN dotnet build "PDFGeneratorSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PDFGeneratorSample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PDFGeneratorSample.dll"]
