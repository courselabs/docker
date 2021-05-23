# Building and modelling apps with Compose

You can think of Compose as documentation which replaces `docker run` - all the options you would put in the run commands are specified in the Compose file which becomes living documentation.

Compose can also record build-time details, so you can build multiple container images using one Docker Compose command. And you can split your Compose spec across multiple files to make them easy to manage, and merge them together at runtime.

## Reference

- [Compose build spec](https://docs.docker.com/compose/compose-file/compose-file-v3/#build)
- [Merging multiple Compose files](https://docs.docker.com/compose/extends/)
- [Environment variables in Compose](https://docs.docker.com/compose/environment-variables/)

## Building with Compose

A single Compose file to build and run your app is very appealing, but run and build options are very different and it's usually easier to split them to keep your Compose specs easier to read and maintain:

- [docker-compose.yml](/labs/compose-build/rng/docker-compose.yml) - defines the services and networks for the random number app

- [docker-compose-build.yml](/labs/compose-build/rng/docker-compose-build.yml) - defines the build options for the services, including the path to the build context and the path to the Dockerfile

All the source code for the random number app is in the `rng` folder, along with the Compose files. Switch to that folder and build the images:

```
cd labs/compose-build/rng

docker-compose -f ./docker-compose.yml -f ./docker-compose-build.yml build 
```

- you merge Compose files with multiple `-f` flags - the files to the right can override settings from files to the left
- `build` tells Compose to build all the images with a `build` section in the Compose file

> You'll see the output from your configured build engine - the original or BuildKit

Now you can run the RNG app using your own images:

- [docker-compose-local.yml](/labs/compose-build/rng/docker-compose-local.yml) - sets configuration details and ports for the services

📋 Run the app in the local configuration by merging the Compose files.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f ./docker-compose.yml -f ./docker-compose-local.yml up -d
```

</details><br/>

> You'll see the containers being created.

Compose creates containers and networks with the `rng` prefix, which is the name of the folder the Compose files are in. As long as you're in the same folder you can manage the containers without specifying file names:

```
docker-compose ps

docker-compose logs
```

## Running multiple environments

Compose adds the prefix so it can identify the containers it created - but you can use the same approach to run multiple copies of an application.

You could do that to run different test environments on a single machine:

- [docker-compose-test.yml](/labs/compose-build/rng/docker-compose-test.yml) - sets up a test environment, using different ports and application config

You can override the default prefix by specifying a *project name* in Compose commands, so instead of updating the existing app, Compose will deploy a new one.

📋 Run the app in the test configuration by merging the Compose files and using the project name `rng-test`

<details>
  <summary>Not sure how?</summary>

```
docker-compose -p rng-test -f ./docker-compose.yml -f ./docker-compose-test.yml up -d
```

</details><br/>

> If you don't add a project name, Compose uses the folder name - which is already being used for the local deployment. Adding a project name lets you run another copy of the app, and you'll see Compose creating new containers.

You can work with a named project without referencing the Compose files:

```
docker-compose -p rng-test ps

docker-compose -p rng-test logs
```

## Modelling configuration

Compose supports the same application configuration options you can use with Docker. We've used environment variables already, the "production" spec adds the other options:

- [docker-compose-prod.yml](/labs/compose-build/rng/docker-compose-prod.yml) - uses an env file for common settings for the components, and a volume mount to load a local folder with a JSON config file into the API container filesystem

This spec also uses an environment variable for the image tag. The syntax `${RELEASE:-21.05}` will use the value of the `RELEASE` environment variable - on the machine running Docker Compose - as the image tag, but use the default value `21.05` if there's no environment variable set.

📋 Run the app in the prod configuration by merging the Compose files and using the project name `rng-prod`

<details>
  <summary>Not sure how?</summary>

```
docker-compose -p rng-prod -f ./docker-compose.yml -f ./docker-compose-prod.yml up -d
```

</details><br/>

> Another new project name, so this deployment creates new containers.


Check that you have all three environments running in containers, available on different ports:

```
docker ps
```

And check the environment variables and config file have been loaded into the prod API container:

```
docker exec rng-prod_rng-api_1 printenv

docker exec rng-prod_rng-api_1 cat /app/config/override.json
```

> Browse to http://localhost:8000 and you'll see a bigger random number range

## Docker Compose for CI Builds

Compose is great for running lots of non-production environments on a single machine, but if you're not plannig to use it for that the build feature is perfect for Continuous Integration.

You can easily build your apps in Jenkins or GitHub Actions just by running `docker-compose build` - and with some additional config you can add some useful auditing to your images:

- the [rng API Dockerfile](/labs/compose-build/rng/docker/api/Dockerfile) uses `ARG` instructions - which are values you can set as build arguments - to add metadata to the image, using labels to record the build version and Git commit ID

- [docker-compose-build.yml](/labs/compose-build/rng/docker-compose-build.yml) sets default values for the build arguments, which can be overridden by environment variables on the machine running the build.

Inspect the image you built locally and you'll see the default arg values in the labels:

```
docker image inspect dockerfundamentals/rng-web:21.05-0
```

This repo also has a GitHub Actions workflow to build the RNG images using the same Docker Compose files:

- [rng-build.yml](/.github/workflows/rng-build.yml) - GitHub workflows use a YAML spec, but if you're not familiar with them you'll see the same `docker-compose` commands being executed.

This is a public repo so you can browse to the workflow output:

https://github.com/courselabs/docker-fundamentals/actions

📋 Drill down into the build output and you'll see an image tag being pushed. Pull that image and inspect the labels.

<details>
  <summary>Not sure how?</summary>

```
docker pull dockerfundamentals/rng-api:21.05-4

docker image inspect dockerfundamentals/rng-api:21.05-4
```

</details><br/>

You'll see the actual build details stored in the image labels:

```
"Labels": {
  "build_tag": "RNG App Docker Image Weekly Build-4-refs/heads/main",
  "commit_sha": "3f83c9128ec8d892e8864fe6a5e904bc919ec517"
}
```

> This is from the same Dockerfiles and Compose files you use for a local build.

## Lab

Look closely at the GitHub workflow and you'll see it runs two sets of builds and pushes - the second set uses different Compose files.

What is the difference in the Compose files and why does the workflow run this second build?

Pull the API image from the tag in the second build and check if it's the same as the tag `21.05-4`.

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```
