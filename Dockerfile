FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.15 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out --self-contained --runtime linux-x64 ./GetWeed.Backend.Startup/GetWeed.Backend.Startup.csproj

FROM debian:jessie-slim

ARG VERSION
ENV APP_VERSION="${VERSION}"
WORKDIR /app
COPY --from=build /app/out/ ./
RUN chmod +x ./getweed && apt-get update && apt-get install -y --no-install-recommends libicu-dev openssl && rm -Rf /var/lib/apt/lists/* && apt-get clean
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
STOPSIGNAL SIGTERM
CMD ["./getweed", "server", "start"]
