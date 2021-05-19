# Running Containers

TODO: intro

## CLI Reference

- [Docker command line reference](https://docs.docker.com/engine/reference/commandline/cli/)
- [Container commands](https://docs.docker.com/engine/reference/commandline/container/)
- [`docker run`](https://docs.docker.com/engine/reference/commandline/run/)

```
docker

docker container --help

docker container run --help
```

## Running a one-off container

```
docker run alpine hostname
```

> pulls, runs, outputs a random ID

Repeat

> Already pulled, new ID

```
docker container ls

docker ps -a
```

Try a container which generates a TLS cert you can use for dev environments:

```
docker run kiamol/ch15-cert-generator
```

> Ctrl-C; docker ps - still running

## Running an interactive container

```
docker run -it alpine

ls /
whoami
hostname

lsb_release
cat /etc/os-release

exit
```

```
docker run -it ubuntu

curl https://dockerfun.courselabs.co

apt-get install -y curl

apt-get update

apt-get install -y curl

curl https://dockerfun.courselabs.co
```

> Only for that container

```
docker run ubuntu bash -c 'curl https://dockerfun.courselabs.co'
```

```
docker run -it sixeyed/hollywood
```


## Running a background container

```
docker run nginx:alpine
```

> Web server with no network access; Ctrl-C

```
docker run -d --name nginx1 -P nginx:alpine

docker container ls

docker container port nginx1

curl locahost:<container-port>
```

## Multiple containers

```
docker run -d -p 8080:80 sixeyed/whoami:21.04

curl localhost:8080
```

> Repeat & fails

```
docker run -d -p 8081:80 sixeyed/whoami:21.04
docker run -d -p 8082:80 sixeyed/whoami:21.04
docker run -d -p 8083:80 sixeyed/whoami:21.04
```

```
docker ps
```

```
curl localhost:8081
curl localhost:8082
curl localhost:8083
```

```
docker top <container-id>

docker stats <container-id>
```

## Lab

container fs - copy files from tls generator; 

```
docker cp af00:/certs/server-cert.pem .
docker cp af00:/certs/server-cert.key .
```

then tidy all up

```
docker rm -f $(docker ps -aq)
```