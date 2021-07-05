# Building Apps with Compose

Compose can also record build-time details, so you can build multiple container images using one Docker Compose command. And you can split your Compose spec across multiple files to make them easy to manage, and merge them together at runtime.

## Reference

- [Compose build spec](https://docs.docker.com/compose/compose-file/compose-file-v3/#build)

- env in compose files (image tag)

## Building with Compose

- docker-compose.yml

All the source code for the random number app is in the `rng` folder, along with the Compose files. Switch to that folder and build the images:

```
cd labs/compose-build/rng

docker-compose build 
```

- `build` tells Compose to build all the images with a `build` section in the Compose file

> You'll see the output from your configured build engine - you can use the original or BuildKit for this lab

These images have the tag `21.05-local`. You can use the same Compose file to run the app from your local images.

📋 Run the app in the local configuration.

<details>
  <summary>Not sure how?</summary>

```
docker-compose up -d
```

</details><br/>

http://localhost:8090

## Build arguments and image labels

A single Compose file to build and run your app is very appealing, but run and build options are very different and it's usually easier to split them to keep your Compose specs easier to read and maintain:

- [core.yml](./rng/core.yml) - defines the core services and networks for the random number app

- [build.yml](./rng/build.yml) - defines the build options for the services, including the path to the build context and the path to the Dockerfile

And with some additional config you can add some useful auditing to your images:

- the [rng API Dockerfile](./rng/docker/api/Dockerfile) uses `ARG` instructions - which are values you can set as build arguments - to add metadata to the image, using labels to record the build version and Git commit ID

- [args.yml](./rng/args.yml) sets default values for the build arguments, which can be overridden by environment variables on the machine running the build.

Build with audit details by combining all the Compose files, then inspect the new API image to see the labels.

```
docker-compose -f core.yml -f build.yml -f args.yml build

docker image inspect dockerfundamentals/rng-api:21.05-0
```


env settings on machine used in image tag; try changing:

```
# linux
export RELEASE=2021.07
export BUILD_NUMBER=121

# windows
$env:RELEASE='2021.07'
$env:BUILD_NUMBER='121'
```

> repeat build; args in Dockerfile RUN so breaks cache on change

check tags

docker image ls dockerfundamentals/rng-api

docker image inspect dockerfundamentals/rng-api:2-2021.07-121

## Docker Compose for CI Builds

Compose is great for running lots of non-production environments on a single machine, but if you're not planning to use it for that the build feature is perfect for Continuous Integration. You can easily build your apps in Jenkins or GitHub Actions just by running `docker-compose build`.

This repo also has a GitHub Actions workflow to build the RNG images using the same Docker Compose files you've been using locally:

- [rng-build.yml](../../.github/workflows/rng-build.yml) - GitHub workflows use a YAML spec, but if you're not familiar with them you'll see the same `docker-compose` commands being executed.

This is a public repo so you can browse to the workflow output:

https://github.com/courselabs/docker-fundamentals/actions/workflows/rng-build.yml

📋 Drill down into the latest build output and you'll see an image tag being pushed. Pull that image and inspect the labels.

<details>
  <summary>Not sure how?</summary>

```
docker pull dockerfundamentals/rng-api:21.05-14

docker image inspect dockerfundamentals/rng-api:21.05-14
```

</details><br/>

You'll see the actual build details stored in the image labels, something like this:

```
"Labels": {
  "build_tag": "RNG App Docker Image Weekly Build-14-refs/heads/main",
  "commit_sha": "2a5f404fd4de49e91bec19d38c02cb8ba218d295"
}
```

> This is from the same Dockerfiles and Compose files you use for a local build.

## Lab

Look closely at the GitHub workflow and you'll see it runs two sets of builds and pushes - the second set uses one more Compose override file.

What is the difference when you use the extra Compose file and why does the workflow run this second build?

Pull the API image from the tag in the second build and check if it's the same as the tag `21.05-14`.

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```
---