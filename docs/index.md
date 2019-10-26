## Micro.KeyStore
KeyStorage micro service. Simply POST data to this service and get a ID which you can use for querying that key.

Simply deploy this to your cluster, there's really no need for any modification, if you do need anything, you can create a issue to see if it's really needed, if it's a generic thing, we can add the feature to this project.

## Is this production ready?
Yes! I'd deploy this to production, but please do your own evaluation. The following are included to make sure everything's of quality

- [x] docker support with docker-compose
- [x] background worker support
- [x] health check support
- [x] `ViewModel` to ensure communication is completely TypeSafe.
- [x] Controller Response type is documented so it can be consumed through swagger
- [x] Swagger Docs (`spec.json` endpoint and docs)
- [x] Docker image generation after doing CI testing
- [x] Health Checks
- [x] Monitoring and reporting has been setup (influx db for storage, and grafana is pre-included)

It doesn't include caching yet, one for the lack of resources, and other because you can setup a caching layer in front of your API or on your client, key is supposed to be queried once per one instance of micro services, unless you have millions of services, this should be fine, because when you consume key, most like it's gonna be cached.
