

## Refs

## Multi-container apps

- manual run

## Compose App Definition

- [](labs\compose\nginx\docker-compose.yml)

```
docker-compose -f .\labs\compose\nginx\docker-compose.yml up
```

localhost:8082

- [](labs\compose\rng\v1.yml)

```
docker-compose -f .\labs\compose\rng\v1.yml up -d
```

docker-compose -f .\labs\compose\rng\v1.yml ps

docker ps

curl localhost:8087/rng

http://localhost:8088

## Container networking

docker network ls

docker network inspect rng_app-net

docker exec rng_rng-web_1 ping -c3 <rng-api-ip>

docker exec rng_rng-web_1 nslookup rng-api

docker exec rng_rng-web_1 wget -qO- rng-api/rng

## Managing Apps with Compose 

docker logs rng_rng-web_1

docker exec rng_rng-web_1 nslookup numbers-api

- [](labs\compose\rng\v2.yml)

docker-compose -f .\labs\compose\rng\v2.yml up -d

docker-compose -f .\labs\compose\rng\v2.yml logs -f

## Lab

