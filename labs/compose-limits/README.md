
## Refs

## Reliability

- v1.yml - fail behaviour enabled

```
docker-compose -f labs\compose-limits\rng\v1.yml up -d
```

Use the app - after 3 numbers, fails

```
docker logs rng_rng-api_1

docker ps

docker start rng_rng-api_1
```

- v2 - restart

docker-compose -f labs\compose-limits\rng\v2.yml up -d

Use app; downtime but API does restart

Restart Docker - app comes back online (same in server with reboot)

## Load

- v3 - scale

docker-compose -f labs\compose-limits\rng\v3.yml up -d

> Errors about port allocation

docker ps

> Index in container name


docker ps -a

> 2nd web container created, not started


docker exec -it rng_rng-web_1 nslookup rng-api

> Multiple IP addresses, order randomized

## Resources

- v4.yml - CPU & memory

docker-compose -f labs\compose-limits\rng\v4.yml up -d

> Fails as not enough CPU, but *after* removing web container

App not available:

docker ps

## Lab
