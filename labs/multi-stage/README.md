
## Multi-stage Dockerfiles

```
$env:DOCKER_BUILDKIT=0

export DOCKER_BUILDKIT=0
```

```
docker build -t simple ./labs/multi-stage/simple/
```

docker run simple


# BuildKit and targets

```
$env:DOCKER_BUILDKIT=1

export DOCKER_BUILDKIT=1
```


```
docker build -t simple ./labs/multi-stage/simple/
```

docker run simple

> Test stage not built

```
docker build -t simple:test --target test ./labs/multi-stage/simple/
```

> Final stage not built...

```
docker run simple:test

docker run simple:test cat /build.txt
```



## Simple Go application


```
docker build -t whoami ./labs/multi-stage/whoami/
```

```
docker run -d -P --name whoami1 whoami

docker port whoami1
```

curl localhost:port

```
docker run -d -P --name whoami2 whoami -port 5000

docker logs whoami2

docker port whoami2
```

curl localhost:port

```
docker run -it --entrypoint sh whoami

docker image ls -f reference=whoami -f reference=golang
```