# Docker Compose

Docker Compose is a specification for describing apps which run in containers, and a command-line tool which takes those specs and runs them in Docker.

It's a _desired-state_ approach, where you model your apps in YAML and the Compose command line creates or replaces components to get to your desired state.

## Reference

- [Docker Compose manual](https://docs.docker.com/compose/)
- [Compose specification - GitHub](https://github.com/compose-spec/compose-spec/blob/master/spec.md)
- [Docker Compose v3 syntax](https://docs.docker.com/compose/compose-file/compose-file-v3/)


<details>
  <summary>CLI overview</summary>

The original Docker Compose CLI is a separate tool:

```
docker-compose --help

docker-compose up --help
```

The latest versions of Docker have the Compose command built-in. The commands are the same, minus the hyphen so `docker-compose` becomes `docker compose`:

```
docker compose --help

docker compose up --help
```

> This is new functionality and it's not 100% compatible with the original Compose CLI. For this lab you should be able to use either, but if you have any issues stick with `docker-compose`.

</details><br/>


## Multi-container apps

Docker can run any kind of app - the container image could be for a lightweight microservice or a legacy monolith. They all work in the same way, but containers are especially well suited to distributed applications, where each component runs in a separate container.

Try running a sample app - this is the web component for a random number generator:

```
docker run -d -p 8088:80 --name rng-web courselabs/rng-web:21.05
```

Browse to http://localhost:8088 and try to get a random number. After a few seconds it will fail and show the error message `RNG service unavailable!`.

📋 Check the application logs to see what's happening.

<details>
  <summary>Not sure how?</summary>

```
docker logs rng-web
```

</details><br/>

> The web app is just the front-end, it's trying to find a backend API service at http://numbers-api/rng - but there's no such service running.

You could start an API container with `docker run`, but you'd need to know the name of the image, the ports to use, and the network setup for the containers to talk to each other.

Instead you can use Docker Compose to model both containers.

## Compose app definition

Docker Compose can define the services of your app - which run in containers - and the networks that join the containers together.

You can use Compose even for simple apps - this just defines an Nginx container:

- [docker-compose.yml](./nginx/docker-compose.yml)

> Why bother putting this in a Compose file? It specifies an image version and the ports to use; it acts as project documentation, as well as being an executable spec for the app.

Docker Compose has its own command line - this tells you the available commands:

```
docker-compose
```

📋 Run this application using the `docker-compose` CLI.

<details>
  <summary>Not sure how?</summary>

```
# run 'up' to start the app, pointing to the Compose file
docker-compose -f ./labs/compose/nginx/docker-compose.yml up
```

</details><br/>

> The Nginx container starts in interactive mode; you can browse to http://localhost:8082 to check it's working.

Use Ctrl-C / Cmd-C to exit - that stops the container.

## Multi-container apps in Compose

Compose is more useful with more components. [rng/v1.yml](./rng/v1.yml) defines the two parts of the random number app:

- there are two services, one for the API and one for the web
- each service defines the image to use and the ports to expose
- the web service adds an environment variable to configure logging
- both services are set to connect to the same container network
- the network is defined but it has no special options set.

📋 Run the app using detached containers and use Compose to print the container status and logs.

<details>
  <summary>Not sure how?</summary>

```
# run the app:
docker-compose -f ./labs/compose/rng/v1.yml up -d

# use compose to show just this app's containers:
docker-compose -f ./labs/compose/rng/v1.yml ps

# and this app's logs:
docker-compose -f ./labs/compose/rng/v1.yml logs
```

</details><br/>

These are just standard containers - the Compose CLI sends commands to the Docker engine in the same way that the usual Docker CLI does.

You can manage containers created with Compose using the Docker CLI too:

```
docker ps
```

Browse to the new web app at http://localhost:8090, and try to get a random number.

> Still not working! Guess we need to debug the web application.

## Managing Apps with Compose 

The web app uses the API to get random numbers. There are only two reasons why that might fail:

1. The API isn't working
2. The web app can't connect to the API

The API publishes a port, so we can check it independently.

📋 Find the published port for the API and browse to the `/rng` endpoint.

<details>
  <summary>Not sure how?</summary>

```
# the API is listening on port 8089 - you can see that in the Compose file or use the CLI:
docker-compose -f ./labs/compose/rng/v1.yml port rng-api 80

curl localhost:8089/rng
```

</details><br/>

> You should get a random number back, so the API is working correctly.

Looks like the web app isn't connecting to the API. The web container is packaged with the `nslookup` tool which we can use to check DNS to see if the API domain is accessible.

📋 Check the logs of the web container to see the API address it's using, and use nslookup to get the IP address of that domain.

<details>
  <summary>Not sure how?</summary>

```
docker logs rng_rng-web_1

# the web app is using the domain 'numbers-api'

# run the nslookup command in the container:
docker exec rng_rng-web_1 nslookup numbers-api
```

</details><br/>

> There's a DNS error - the API domain isn't accessible.

The name of a service in the Compose file becomes the DNS name containers can use to access that service. The API service name is `rng-api`, not `numbers-api`.

We could change the API service in the Compose file, but the web app supports a configuration setting for the API URL:

- [rng/v2.yml](./rng/v2.yml) sets that config value and also increases the logging level for the API.

Here we'll see the desired-state approach. If you need to change your application, you change the YAML and run `up` again. Compose looks at what's running and what you're asking to run and it makes whatever changes it needs.

📋 Deploy the updated Compose spec in `labs/compose/rng/v2.yml` and use Compose to follow all the container logs.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f ./labs/compose/rng/v2.yml up -d

docker-compose -f ./labs/compose/rng/v2.yml logs -f
```

</details><br/>

> You'll see the web container get recreated because the spec has changed. Try the app now at http://localhost:8090 and it will work - and you'll see the app logs from Compose.

## Lab

Compose is used to define apps that span multiple containers, but the services are only related through container networks.

Add an Nginx container and another network to the RNG app definition. Configure the Nginx service to the new network and the original network.

Deploy the updated spec. What IP address(es) does the Nginx container have? Can you connect the containers together - is the Nginx container accessible from the RNG web container, even though it was created afterwards?

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```