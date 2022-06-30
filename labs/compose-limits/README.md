# Limitations of Compose

Docker Compose is a client-side tool - it gives you an easy way to model and deploy multi-container apps, but it is not a container orchestrator. 

The main limitation of Compose is that it runs on a single server. There's no way to manage multiple servers with Compose, so you can't make your apps fully scalable and reliable.

## References

- [Compose in production](https://docs.docker.com/compose/production/)
- [Understanding container orchestration - YouTube](https://youtu.be/F7rORInGvc4)

## Adding reliability

Applications fail and that can cause containers to exit. In production you would want your platform to see the container has exited and start a replacement, but that's not the default behaviour in Compose.

- [rng/v1.yml](./rng/v1.yml) is the same random number app we've used already, but with a configuration which causes the API to fail after 3 calls

📋 Run the app from the file `labs/compose-limits/rng/v1.yml`.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f labs/compose-limits/rng/v1.yml up -d
```

</details><br/>

You'll see the two containers start. Browse to the app at http://localhost:8088 - click the button 3 times and you'll get a random number. After that it fails and you'll see the `RNG service unavailable!` error message.

📋 Look at the API logs and then check the status of the containers.

<details>
  <summary>Not sure how?</summary>

```
docker logs rng_rng-api_1

docker ps -a
```

</details><br/>

> The API container has stopped because the API server exited.

You can restart a stopped container manually:

```
docker start rng_rng-api_1
```

> Now the app will work again, but only for three calls, then the API container will exit and you'll need to start it again.

You can configure containers to automatically restart if they exit:

- [rng/v2.yml](./rng/v2.yml) adds the `restart: always` setting to both services, so the containers restart if the application process exits.

📋 Update your deployment to the spec in `labs/compose-limits/rng/v2.yml`.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f labs/compose-limits/rng/v2.yml up -d
```

</details><br/>

> Both containers are recreated. The old web container was running fine but it didn't match the new spec.

Now when you use the app it will still fail after 3 numbers, but then Docker restarts the API container. The app won't work while the container starts up, but then it will work again:

```
# try the app until it fails, then check the container status:
docker ps -a
```

> The status for the API container shows a time difference between when it was created and how long it's been up - that's the most recent start time.

The restart flag also starts containers when the Docker engine starts. You can restart Docker Desktop and your app will come back online, but it will be unavailable while Docker is starting.

## Running at scale

You can increase reliability and the amount of load your apps can handle by running multiple containers:

- [rng/v3.yml](./rng/v3.yml) adds the `scale` setting to both services, so Compose will try to run 3 API containers and 2 web containers.

Deploy the update and check the output from the CLI:

```
docker-compose -f labs/compose-limits/rng/v3.yml up -d
```

> You'll see errors about port allocation. The web service publishes a port - only one container can use a specific port, so you can't run public components at scale with Compose.

📋 List the running containers and then all containers. Is the app working?

<details>
  <summary>Not sure how?</summary>

```
docker ps

docker ps -a
```

</details><br/>

> You'll see multiple API containers running, and an additional web container which is stuck in the `Created` status. The app is working, and you can get a lot more numbers before it fails while API containers restart.

There are multiple API containers, so Docker adds all their IP addresses to the DNS response. When the web container makes a call to the API domain name, it gets load-balanced between the API containers.

You can see all the API container IPs in the DNS response in the web container:

```
docker exec -it rng_rng-web_1 nslookup rng-api
```

> If you repeat that call you'll see the order of the addresses is randomized, which helps spread the load if the client just uses the first address in the list.

## Allocating compute resources

The final limitation with Compose is that you can only use the resources available on one server. If you have a power-hungry app you might not be able to run it at all:

- [rng/v4.yml](./rng/v4.yml) adds CPU and memory allocations to the containers. It requests 4 CPU cores and 8GB memory for each API container and 32 cores and 64GB memory for the web container.

📋 Deploy the v4 update and check the containers. Is the app working?

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f labs/compose-limits/rng/v4.yml up -d

docker ps -a
```

</details><br/>

> Unless you have a seriously capable machine, you'll get errors saying there are not enough CPUs available. 

Compose removes the working web container to replace it, then finds it can't create the replacement - so now there's no web container and the update broke your app.

## Lab

This is just a thinking lab :) We've seen the limitations of Docker Compose - what practical problems would you have if you tried to use Compose in production?

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```