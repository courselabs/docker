# Lab Solution

The sample solution [rng/lab.yaml](/labs/compose/rng/lab.yml) adds:

- a network called `front-end` with no options, so it will be created with the Docker defaults
- a service called `nginx` which uses the Nginx image and connects to the `front-end` and `app-net` networks.

Deploy the update:

```
docker-compose -f ./labs/compose/rng/lab.yml up -d
```

> You'll see the new network and container created, but the RNG web and API containers will be left unchanged. The spec hasn't changed for those services, so the containers match the desired state.

Check the network of the new container:

```
docker inspect rng_nginx_1
```

> You'll see it has two IP addresses, one from each network - like a machine with two network cards.

Test connectivity:

```
docker exec rng_nginx_1 nslookup rng-api

docker exec rng_nginx_1 nslookup rng-web
```

> The new container can resolve IP addresses for the original containers.

```
docker exec rng_rng-web_1 ping -c3 nginx
```

> And the old containers can reach the new one.