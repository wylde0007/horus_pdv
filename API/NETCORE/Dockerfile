FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HORUSPDV-API.csproj ./
RUN dotnet restore HORUSPDV-API.csproj

COPY . ./
RUN dotnet publish HORUSPDV-API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet HORUSPDV-API.dll --urls http://0.0.0.0:${PORT}"]
