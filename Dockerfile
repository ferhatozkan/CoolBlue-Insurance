FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
COPY . .
WORKDIR /app/src/Insurance.Api
RUN dotnet restore
RUN dotnet publish -o /app/insurance

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/insurance /app
ENTRYPOINT [ "dotnet", "/app/Insurance.Api.dll" ]