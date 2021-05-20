
## Refs

## Base images


```
$env:DOCKER_BUILDKIT=0

export DOCKER_BUILDKIT=0
```

```
docker build -t dockerfun/base ./labs/images/base
```

```
docker image ls 'dockerfun/*'
```

```
docker pull ubuntu
docker pull ubuntu:20.04

docker image ls ubuntu
```

docker pull unbuntu

> Unhelpful

## Commands and entrypoints

```
docker build -t dockerfun/curl ./labs/images/curl
```

```
docker run dockerfun/curl 

docker run dockerfun/curl dockerfun.courselabs.co

docker run dockerfun/curl curl --head dockerfun.courselabs.co
```

```
docker build -t dockerfun/curl:v2 -f labs/images/curl/Dockerfile.v2 labs/images/curl
```

```
docker run dockerfun/curl:v2 --head dockerfun.courselabs.co
```

```
docker image ls dockerfun/curl
```

## Image layers and hierarchy

```
docker build -t dockerfun/web ./labs/images/web
```

```
docker run -d -p 8090:80 dockerfun/web

curl localhost:8090
```

```
docker build -t dockerfun/network-test ./labs/images/network-test
```

```
docker run dockerfun/network-test
```

```
docker run -e TEST_DOMAIN=k8sfun.courselabs.co dockerfun/network-test
```



## Tagging and pushing

```
docker image ls dockerfun/*
```

```
$dockerId='<your-docker-hub-id>'
dockerId='<your-docker-hub-id>'
```

> `$dockerId='sixeyed'`

```
docker tag dockerfun/network-test ${dockerId}/network-test:21.05
```

docker image ls */network-test

docker login

docker push ${dockerId}/network-test:21.05

echo "https://hub.docker.com/r/${dockerId}/network-test/tags"

```

docker tag dockerfun/network-test microsoft/dotnet:3.1

docker push microsoft/dotnet:3.1


## Lab

TODO 

# EXTRA

```
docker build -t dockerfun/network-test:override ./labs/images/network-test-override
```

```
docker run dockerfun/network-test:override

docker run -e TEST_DOMAIN=k8sfun.courselabs.co dockerfun/network-test:override
```

docker image history dockerfun/network-test

docker image history dockerfun/network-test:override