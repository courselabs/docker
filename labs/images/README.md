# Building Container Images

*Images* are the packages which containers run from. You'll build an image for each component of your application, and the image has all the pre-requisites installed and configured, ready to run.

You can think of images as the filesystem for the container, plus some metadata which tells Docker which command to run when the container starts.

## Reference

- [Dockerfile syntax](https://docs.docker.com/engine/reference/builder/)
- [Image build command](https://docs.docker.com/engine/reference/commandline/image_build/)
- [Pulling images](https://docs.docker.com/engine/reference/commandline/image_pull/)
- [Pushing images](https://docs.docker.com/engine/reference/commandline/image_push/)

<details>
  <summary>CLI overview</summary>

You use the `image` commands to work with images. The most popular commands also have aliases:

```
docker image --help

docker build --help

docker pull --help

docker push --help
```

</details><br/>


## Base images

Images can be built in a hierarchy - you may start with an OS image which sets up a particular Linux distro, then build on top of that to add your application runtime.

Before we build any images we'll set the Docker to use the original build engine:

```
# on macOS or Linux:
export DOCKER_BUILDKIT=0

# OR with PowerShell:
$env:DOCKER_BUILDKIT=0
```

> [BuildKit](https://docs.docker.com/develop/develop-images/build_enhancements/) is a more efficient build engine; it produces the same images but the printed output is less clear, so it's better to start with the old engine.

We'll build a really simple base image:

- [base image Dockerfile](/labs/images/base/Dockerfile)

```
docker build -t dockerfun/base ./labs/images/base
```

- `-t` or `--tag` gives the image a name
- you end the build command with the path to the Dockerfile folder


📋 List all the images you have - then filter them for images starting with 'dockerfun'.

<details>
  <summary>Not sure how?</summary>

```
# list all local images:
docker image ls

# and filter for the dockerfun images:
docker image ls 'dockerfun/*'
```

</details><br/>

> These are the images stored in your local Docker engine cache.

The new base image doesn't add anything to the [official Ubuntu image](https://hub.docker.com/_/ubuntu).


📋 Pull the main Ubuntu image, then pull the image for Ubuntu version 20.04.


<details>
  <summary>Not sure how?</summary>

```
docker pull ubuntu

# image versions are set in the tag name:
docker pull ubuntu:20.04
```

</details><br/>

List all your Ubuntu images and your own base image:

```
docker image ls --filter reference=ubuntu --filter reference=dockerfun/base
```

> You'll see they all have the same ID - they're actually aliases of one image

## Commands and entrypoints

The Dockerfile syntax is straightforward to learn:

- every image needs to start `FROM` another image
- you use `RUN` to execute commands as part of the build
- and `CMD` sets the command to run when the container starts

Here's a simple example which installs the curl tool:

- [curl Dockerfile](/labs/images/curl/Dockerfile)

📋 Build an image called `dockerfun/curl` from the `labs/images/curl` Dockerfile.

<details>
  <summary>Not sure how?</summary>

```
docker build -t dockerfun/curl ./labs/images/curl
```

</details><br/>

Now you can run a container from the image, but it might not behave as you expect:

```
# just runs curl:
docker run dockerfun/curl 

# doesn't pass the URL to curl:
docker run dockerfun/curl dockerfun.courselabs.co

# to browse you need to specify the curl command:
docker run dockerfun/curl curl --head dockerfun.courselabs.co
```

> The `CMD` instruction sets the exact command for a container to run. You can't pass options to the container command - but you can override it completely.

This updated Dockerfile makes a more usable image:

- [curl Dockerfile - v2](/labs/images/curl/Dockerfile.v2)

Build a v2 image from that Dockerfile:

```
docker build -t dockerfun/curl:v2 -f labs/images/curl/Dockerfile.v2 labs/images/curl
```

- the `-f` flag specifies the Dockerfile name - you need it if you're not using the standard name

You can run containers from this image with more logical syntax:

```
docker run dockerfun/curl:v2 --head dockerfun.courselabs.co
```

> The `--head` argument in the container command gets passed to the entrypoint

📋 List all the `dockerfun/curl` images to compare sizes.

<details>
  <summary>Not sure how?</summary>

```
docker image ls dockerfun/curl
```

</details><br/>

> The v2 image is smaller - which means it has less stuff in the filesystem and a smaller attack surface.


## Image hierarchy

You don't typically use OS images as the base in your `FROM` image. You want to get as many of your app's pre-requisites already installed for you.

You should use [official images](https://hub.docker.com/search?q=&type=image&image_filter=official&category=languages), which are application and runtime images which are maintained by the project teams.

This Dockerfile bundles some custom HTML content on top of the official Nginx image:

- [web Dockerfile](/labs/images/web/Dockerfile)

```
docker build -t dockerfun/web ./labs/images/web
```

- the folder path is called the *context* - it contains the Dockerfile and any files it references, the `index.html` file in this case

📋 Run a container from your new image, publishing port 80, and browse to it.

<details>
  <summary>Not sure how?</summary>

```
docker run -d -p 8090:80 dockerfun/web

curl localhost:8090
```

</details><br/>

> The container serves your HTML document, using the Nginx setup configured in the official image 

## Tagging and pushing

All the images you've built are only available on your machine so far.

To share images you need to push them to a *registry* - like Docker Hub. The image name needs to include your username, which Docker Hub uses to identify ownership.

Start by saving your Docker ID in a variable:

```
# on Linux or macOS:
dockerId='<your-docker-hub-id>'

# OR with PowerShell:
$dockerId='<your-docker-hub-id>'
```

> This is your Hub username *not* your email address. For mine I use: `$dockerId='sixeyed'`

Now you can *tag* one of your images with another name:

```
docker tag dockerfun/curl:v2 ${dockerId}/curl:21.05

docker image ls '*/curl'
```

📋 Now push your `curl:21.05` image to Docker Hub.

<details>
  <summary>Not sure how?</summary>

```
# log in if you haven't already:
docker login -u ${dockerId}

# push your image:
docker push ${dockerId}/curl:21.05
```

</details><br/>

Docker Hub images are publicly available (you can create private images too). Run this command and browse to your image on Docker Hub:

```
echo "https://hub.docker.com/r/${dockerId}/curl/tags"
```

## Lab

Image names (properly called *references*) are built from three parts:

- the domain of the container registry
- the repository name - which identifies the app and the owner
- the tag - which can be anything but is usually used for versioning

Docker uses defaults for the registry and the tag. What are those defaults? What is the full reference for the image `kiamol/ch05-pi`?

Not all official images are on Docker Hub. Microsoft uses its own image registry *MCR* at `mcr.microsoft.com`. What command would you use to pull version `5.0` of the `dotnet/runtime` image from MCR?

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## **EXTRA** Overriding image content


```
docker build -t dockerfun/network-test ./labs/images/network-test
```

```
docker run dockerfun/network-test
```

```
docker run -e TEST_DOMAIN=k8sfun.courselabs.co dockerfun/network-test
```

```
docker build -t dockerfun/network-test:override ./labs/images/network-test-override
```

```
docker run dockerfun/network-test:override

docker run -e TEST_DOMAIN=k8sfun.courselabs.co dockerfun/network-test:override
```

docker image history dockerfun/network-test

docker image history dockerfun/network-test:override

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```