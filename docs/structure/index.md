## File Structure
```
$ tree -I 'docs|*bin*|*obj*'
.
├── docker-compose.ci.yml
├── docker-compose.yml
├── Dockerfile
├── extras
│   └── grafana
│       └── Key.json
├── hooks
│   └── commit-msg
├── Micro.KeyStore.Api
│   ├── appsettings.json
│   ├── Archive
│   │   ├── ArchiveKeysConfig.cs
│   │   ├── drivers
│   │   │   ├── Noop.cs
│   │   │   └── Webhook.cs
│   │   └── IDriver.cs
│   ├── Configs
│   │   ├── DatabaseConfig.cs
│   │   └── SlackLoggingConfig.cs
│   ├── Controllers
│   │   ├── WeatherForecastController.cs
│   │   └── WeatherForecast.cs
│   ├── HealthCheck
│   │   ├── HealthCheckController.cs
│   │   └── HealthData.cs
│   ├── Keys
│   │   ├── KeysController.cs
│   │   ├── Models
│   │   │   ├── Key.cs
│   │   │   └── ViewModels.cs
│   │   ├── Repositories
│   │   │   ├── IKeyRepository.cs
│   │   │   └── KeyRepository.cs
│   │   ├── Services
│   │   │   ├── ConflictingKeyConflictException.cs
│   │   │   ├── IKeyService.cs
│   │   │   └── KeyService.cs
│   │   └── Sha256.cs
│   ├── Measurements
│   │   ├── ShortShaKey.cs
│   │   └── Timer.cs
│   ├── Micro.KeyStore.Api.csproj
│   ├── Migrations
│   │   └── ApplicationContextModelSnapshot.cs
│   ├── Models
│   │   ├── ApplicationContext.cs
│   │   └── Weather.cs
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   ├── Startup.cs
│   ├── Uuid
│   │   ├── IUuidService.cs
│   │   └── UuidService.cs
│   └── Workers
│       ├── CleanupKeysWorker.cs
│       └── Key.cs
├── Micro.KeyStore.IntegrationTest
│   ├── ExternalTests
│   │   └── postman_tests.sh
│   ├── Keys
│   │   └── KeyRepositoryTest.cs
│   └── Micro.KeyStore.IntegrationTest.csproj
├── Micro.KeyStore.sln
├── Micro.KeyStore.UnitTest
│   ├── ExternalTests
│   │   └── postman_tests.sh
│   ├── Keys
│   │   └── KeyServiceTest.cs
│   ├── Micro.KeyStore.UnitTest.csproj
│   └── UnitTest1.cs
├── readme.md
└── release.config.js
```

## Micro.KeyStore.Api
This is the project which is actually booted, once it boots, it configures and starts listening for incoming requests.
`Controllers` are where requests will land in, they're not supposed to contain any business logic,
but rather extract data from requests and pass in to other services.

## Micro.KeyStore.UnitTest
This project contains unit tests and postman tests for Micro.Starter.Api

## Micro.KeyStore.IntegrationTest
This project contains integration tests for Repository layer.
