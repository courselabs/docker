# Lab Solution

The second part of the GitHub build uses the `latest` Docker Compose file:

- [docker-compose-latest.yml](/labs/compose-build/rng/docker-compose-latest.yml) uses a different image tag, with the `RELEASE` environment variable but not the `BUILD_NUMBER` variable the main Compose file uses

When you merge in the latest file it will build images with the tag `21.05`, which is the release version for this app.

Consumers can use `21.05` to get the current build for this release, or `21.05-4` to get a specific build:

```
docker pull dockerfundamentals/rng-api:21.05

docker image ls dockerfundamentals/rng-api
```

Those two tags are aliases of the same image now, but with the next release the `21.05` tag will advance and will be an alias of the `21.05-5` build.