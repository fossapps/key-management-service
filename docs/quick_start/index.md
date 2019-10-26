## Getting Started

Micro.KeyStore is an opinionated keystore for authentication service.

When you create a key pair and want to save public key to a key storage service.

You can simply deploy this to your cluster and start using as well.

### Quick Start
```bash
git clone git@github.com:fossapps/Micro.KeyStore
cd Micro.KeyStore
dotnet restore
dotnet run --project ./Micro.KeyStore.Api/Micro.KeyStore.Api.csproj
```
App should start listening on `http://localhost:5000`
