# Lab Solution

The sample solution [lab/compose.yaml](./lab/compose.yml) adds:

- a network called `front-end` with no options, so it will be created with the Docker defaults
- a service called `nginx` which uses the Nginx image and connects to the `front-end` and `app-net` networks.

Compose uses the directory name of the YAML file to identify the application - so to update the app, the Compose file needs to be copied to the `rng` folder.

Copy the Compose file and deploy the update:

```
cp ./labs/compose/lab/compose.yml ./labs/compose/rng/lab.yml

docker-compose -f ./labs/compose/rng/lab.yml up -d
```

> You'll see the new network and container created, but the RNG web and API containers will be left unchanged. The spec hasn't changed for those services, so the containers match the desired state.

Inspect the new container to show the network details:

```
docker inspect rng_nginx_1
```

> You'll see it has two IP addresses in the network section at the end of the output. This is one IP from each network - like a machine with two network cards.

Test connectivity from the Nginx container to the web container:

```
docker exec rng_nginx_1 nslookup rng-web
```

> The new container can resolve IP addresses for the original container.


And from the web container to Nginx:

```
docker exec rng_rng-web_1 ping -c3 nginx
```

> The old containers can reach the new one.

> Back to the [exercises](README.md).