## Cleaning up (Archiving)
Archiving is a big part of maintaining public keys, once they expire, there's really no need for them to stay around and contribute to table size or key conflicts.

To solve that, this project includes a worker which runs on background and removes keys using a retention policy.

## Configuring
```json
{
    "ArchiveKeys": {
        "Driver": "noop",
        "WebhookUrl": "http://mockbin.org/bin/5456de1f-c02d-471d-b426-4c2388d5b9bd",
        "BatchSize": 200,
        "BatchIntervalInSeconds": 2,
        "TimeToLiveInMinutes": 15
      }
}
```
The above is the default configuration which is set, we can use environment variables to overwrite any data, Please check overwriting configuration section.

### Driver:
This project has 2 drivers: noop and webhook. Once it sends data to driver, it deletes from local table.

#### noop driver
this essentially does nothing, this is perfect when you don't want to save historical keys, simply cleans up and is fast.

#### webhook driver
this POSTs key and id to a webhook endpoint, any custom business you might want to have will have to be a separate micro-service which does what you need to do.

### WebhookUrl
this is only needed when you want to use webhook driver to configure where to send

### BatchSize
To ensure we don't spam a webhook url or do a large delete on table and cause replication problems, worker takes some items which needs to be cleaned up, cleans them up, then waits for next batch so it can reduce load.

### BatchIntervalInSeconds
Number of seconds to wait before sending another batch

### TimeToLiveInMinutes
Once a key is created, after this period it will be Archived.
