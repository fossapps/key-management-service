## KeyStore

## Documentation
[Documentation](https://fossapps.github.io/Micro.KeyStore/) is updated on every merge to master using jekyll on github pages

Started from Micro.Starter Project, the goal of this project is to have a reusable Key Storage for different purposes.

Consider a authentication system which uses JWT, and uses asymmetric keys to sign keys, public keys can be stored on this storage with some retention policy.

Once a key expires, it's simply removed after being sent to a long term storage for historical purposes.

## Used packages

## Getting Started
You can start by cloning this repository, or better yet, just use docker to run this on your own.

## Contribution
Please follow the existing coding standards which is being followed, no trailing whitespaces, edge cases goes to if conditions,
follow line of sight rule. Happy path is always straight down, only short circuit (early exits) the error path unless there's a strong reason not to.

## More Documentation
Before a feature is worked on, I'll try to document on what needs to be done, after the feature is ready,
I'll try to finalize the documentation. If there's something missing, please contribute.
