## KeyStore

Started from Micro.Starter Project, the goal of this project is to have a reusable Key Storage for different purposes.

Consider a authentication system which uses JWT, and uses asymmetric keys to sign keys, public keys can be stored on this storage with some retention policy.

Once a key expires, it's simply removed after being sent to a long term storage for historical purposes.

## Save removed keys
Once a key is no longer in use (defined by retention policy), it needs to be removed from database for few reasons.
- We can provide shortest key because keys won't collide
- Removing keys will ensure tables are small
- Ability to backup or restore quick
- Ability to read the whole table in memory and serve from there if needed

But before permanently removing a key, we'll store it somewhere based using driver pattern,
so new deployment of this service can be done without writing any more code.

Initially I'll include one driver which does nothing. But I'll be accepting more drivers which will save to database, S3, azure, firebase etc.
If there's a special case, then you'd need to clone this repo and write your own driver. (help wanted: if you can find a way to add config and driver stuff separated, I'd appreciate it, it'd be also awesome if I could use a certain driver during deployment, like deploy this service and driver separately and point to the driver on runtime)

## Used packages

## Getting Started
You can start by cloning this repository, or better yet, just use docker to run this on your own.

## Contribution
Please follow the existing coding standards which is being followed, no trailing whitespaces, edge cases goes to if conditions,
follow line of sight rule. Happy path is always straight down, only short circuit (early exits) the error path unless there's a strong reason not to.

## More Documentation
Before a feature is worked on, I'll try to document on what needs to be done, after the feature is ready,
I'll try to finalize the documentation. If there's something missing, please contribute.
