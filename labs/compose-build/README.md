

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

## Docker Compose for CI Builds

- args & env 

```
docker image inspect dockerfundamentals/rng-web:21.05-0
```

- GH workflow


## 