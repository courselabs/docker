

## Switch to Swarm mode


docker swarm init

docker node ls



## Store configuration files

docker config create rng-logging labs\orchestration\config\logging.json

docker config ls

docker config inspect --pretty rng-logging


docker secret create rng-api labs\orchestration\config\api.json

docker secret create rng-web labs\orchestration\config\web.json

docker secret ls

docker secret inspect --pretty rng-api

## Deploy application

docker stack deploy -c labs\orchestration\rng-v1.yml rng

docker service ls

> http://localhost:8090; fails after 10 and restarts

Manage

docker service ls

docker service logs rng_rng-api

Scale

docker stack deploy -c labs\orchestration\rng-v2.yml rng

> Waits until all started; try app - load-balanced

docker rm -f $(docker ps -aq)

> Wait a few seconds...

docker ps

> All replaced, app online again

___
## Cleanup

Cleanup by exiting Swarm mode - that removes all containers:

```
docker swarm leave -f
```