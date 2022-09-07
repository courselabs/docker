# Lab Solution

Roll back to the earlier deployment:

```
docker-compose -f labs/networking/compose.yml up -d
```

And you can scale up with the Compose CLI:

```
docker-compose -f labs/networking/compose.yml up -d --scale rng-api=3
```

> You'll see two new API containers created.

Follow the logs from all API containers:

```
docker-compose -f labs/networking/compose.yml logs -f rng-api
```

> Use the app at http://localhost:8090 and you'll see different API containers generating numbers.

Inspect the API containers and you'll see compose sets the same network alias for each of them:

```
docker inspect networking_rng-api_1
```

- includes the container's hostname and the Compose service name in the .NetworkSettings.Networks section, e.g.

```
"Aliases": [
  "rng-api",
  "b382ce0e8ffc"
]
```

Any containers with the same alias will get returned in the DNS response for that domain name.


> Back to the [exercises](README.md).