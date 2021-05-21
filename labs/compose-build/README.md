

## Refs

## Building with Compose

```
cd labs/compose-build/rng

docker-compose -f .\docker-compose.yml -f .\docker-compose-build.yml build --pull
```

Run local:

```
docker-compose -p rng-local -f .\docker-compose.yml -f .\docker-compose-local.yml up -d

docker-compose -p rng-local ps

docker-compose -p rng-local logs -f
```

## Running multiple environments

Run test:

```
docker-compose -p rng-test -f .\docker-compose.yml -f .\docker-compose-test.yml up -d

docker-compose -p rng-test ps

docker-compose -p rng-test logs -f
```

## Modelling configuration

- env
- env-file
- json load

Run prod:

```
docker-compose -p rng-prod -f .\docker-compose.yml -f .\docker-compose-prod.yml up -d

docker-compose -p rng-prod ps

docker exec rng-prod_rng-api_1 printenv

docker exec rng-prod_rng-api_1 cat /app/config/override.json
```

localhost:8000, bigger rng range

## Docker Compose for CI Builds

- args & env 

```
docker image inspect dockerfundamentals/rng-web:21.05-0
```

- GH workflow

https://github.com/courselabs/docker-fundamentals/actions

```
docker pull dockerfundamentals/rng-api:21.05-4

docker image inspect dockerfundamentals/rng-api:21.05-4
```


```
docker pull dockerfundamentals/rng-api:21.05

docker image ls dockerfundamentals/rng-api
```

## Lab