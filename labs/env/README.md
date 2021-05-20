# Constructing the Container Environment

## CLI Reference

- [](https://docs.docker.com/engine/reference/commandline/run/#set-environment-variables--e---env---env-file)
- [](https://docs.docker.com/engine/reference/commandline/run/#mount-volume--v---read-only)
- [](https://docs.docker.com/config/containers/resource_constraints/)

## Environment variables

```
docker run alpine printenv

docker run java:8-jdk-alpine printenv
```

```
docker run -e DOCKERFUN=env alpine printenv
```

> Can't be edited, lifetime of container

```
docker run -e DOCKERFUN=env -e RELEASE=21.05 alpine printenv
```

```
cat labs/env/exercises.env

docker run --env-file labs/env/exercises.env alpine printenv
```

## Container filesystem

```
docker run -d -P --name nginx nginx:alpine
```

```
docker exec -it nginx sh

ls /usr/share/nginx/html/

cat /usr/share/nginx/html/index.html
```

```
$htmlPath="${PWD}/labs/env/html"
htmlPath="${PWD}/labs/env/html"

docker run -d -p 8081:80 -v ${htmlPath}:/usr/share/nginx/html --name nginx2 nginx:alpine
```

> Change index.html and check again - doesn't alter image or other containers unless they have same share

```
docker exec nginx2 which nginx

docker run -d -p 8081:80 -v ${htmlPath}:/usr/sbin --name nginx3 nginx:alpine

docker ps -a

docker logs nginx3
```

## Compute resources

```
docker run -d -p 8031:80 --name pi kiamol/ch05-pi -m web 
```

Browse to http://localhost:8031/pi?dp=50000, takes ~3s

> Can't change CPU resources

```
docker inspect pi
```
> All CPU and memory quotas at 0, meaning unlimited


```
docker rm -f pi

docker run -d -p 8031:80 --name pi --memory 200m --cpus 0.25 kiamol/ch05-pi -m web 
```

> Refresh http://localhost:8031/pi?dp=50000, now takes >13s

```
docker container inspect --format='Memory: {{.HostConfig.Memory}}b, CPU: {{.HostConfig.NanoCpus}}n' pi
```

## Network resources

TODO - make an extra?

$scriptsPath="${PWD}/labs/env/scripts"
scriptsPath="${PWD}/labs/env/scripts"

```
docker run -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

> All set by Docker

- hostname & dns

```
docker run --dns 1.1.1.1 --hostname alpine1 -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

> IP set by Docker

- network & ip 

```
docker network create --subnet=10.10.0.0/16 dockerfun

docker run --network dockerfun --ip 10.10.0.100 -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

## Lab

- cmd/entrypoint

container fs - copy files from tls generator; 

```
docker cp af00:/certs/server-cert.pem .
docker cp af00:/certs/server-cert.key .
```

then tidy all up

```
docker rm -f $(docker ps -aq)
```