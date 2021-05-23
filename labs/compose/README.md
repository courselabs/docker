# Docker Compose

Docker Compose is a specification for describing apps which run in containers, and a command-line tool which takes those specs and runs them in Docker.

It's a desired-state approach, where you model your apps in YAML and the Compose command line creates or replaces components to get to your desired state.

## Reference

- [Docker Compose manual](https://docs.docker.com/compose/)
- [Compose specification - GitHub](https://github.com/compose-spec/compose-spec/blob/master/spec.md)
- [Docker Compose v3 syntax](https://docs.docker.com/compose/compose-file/compose-file-v3/)


## Multi-container apps

Docker can run any kind of app - the container image could be for a lightweight microservice or a legacy monolith. They all work in the same way, but containers are especially well suited to distributed applications, where each component runs in a separate container.

Try running a sample app - this is the web component for a random number generator:

```
docker run -d -p 8088:80 --name rng-web dockerfundamentals/rng-web:21.05
```

Browse to http://localhost:8088 & try to get a random number. It will fail.

📋 Check the application logs to see what's happening.

<details>
  <summary>Not sure how?</summary>

```
docker logs rng-web
```

</details><br/>

> The web app is just the front-end, it's trying to find a backend API service at http://numbers-api/rng - but there's no such service running.

You could run another container for the API, but you'd need to know the name of the image, the ports to use, and the network setup for the containers to talk to each other.

Instead you can use Docker Compose to model the whole app in one file.

## Compose App Definition

Docker Compose can define the services of your app - which run in containers - and the networks that join the services together.

You can use Compose even for simple apps - this just defines an Nginx container:

- [docker-compose.yml](/labs/compose/nginx/docker-compose.yml)

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

Compose is more useful with more components. [rng/v1.yml](/labs/compose/rng/v1.yml) defines the two parts of the random number app:

- there are two services, one for the API and one for the web
- each service defines the image to use and the ports to expose
- the web service adds an environment variable to configure logging
- both services are set to connect to the same container network
- the network is defined but it has no special options set.

Run the app using detached containers:

```
docker-compose -f ./labs/compose/rng/v1.yml up -d
```

📋 Use Compose to print the status of the containers, and the app logs.

<details>
  <summary>Not sure how?</summary>

```
# you can use some of the usual Docker command names with Compose:
docker-compose -f ./labs/compose/rng/v1.yml ps

docker-compose -f ./labs/compose/rng/v1.yml logs
```

</details><br/>

These are just standard containers - the Compose CLI sends commands to the Docker engine the same way the usual Docker CLI does.

You can manage containers created with Compose using the Docker CLI too:

```
docker ps
```

Browse to the app now at http://localhost:8088, and try to get a random number.

> Still not working! Guess we need to debug the web application.

## Container networking

Docker manages networking - containers can be attached to Docker networks, and containers on the same network can reach each other by IP address.

Docker also runs a DNS server, so containers on the same network can find each other using the container name as the DNS host name.

📋 List all the Docker networks and print the details of the RNG app network.

<details>
  <summary>Not sure how?</summary>

```
# networks are a top-level object in the Docker CLI:
docker network ls

docker network inspect rng_app-net
```

</details><br/>

> You'll see a lot of details about the network - including containers which are attached to it. The API and web containers are there, with IP addresses in the same network range so they should be able to connect.

The web container image has some networking tools installed, so we can try and find out what's going wrong:

```
# run a DNS lookup for the API service name:
docker exec rng_rng-web_1 nslookup rng-api

# ping the IP address of the API container :
docker exec rng_rng-web_1 ping -c3 <rng-api-ip>

# try calling the API inside the web container:
docker exec rng_rng-web_1 wget -qO- rng-api/rng
```

> That all works, so it must be a configuration issue with the web component.

## Managing Apps with Compose 

Docker Compose uses a desired-state approach. If you need to change your application, you change the YAML and run `up` again. Compose looks at what's running and what you're asking to run and it makes whatever changes it needs.

📋 Check the logs of the web container again, and see if the API domain it's using is registered with DNS.

<details>
  <summary>Not sure how?</summary>

```
docker logs rng_rng-web_1

# the web app is using the domain 'numbers-api'
docker exec rng_rng-web_1 nslookup numbers-api
```

</details><br/>

> The API domain doesn't match the service name we're using.

We could change the API service in the Compose file, but the web app supports a configuration setting for the API URL:

- [rng/v2.yml](/labs/compose/rng/v2.yml) sets that config value and also increases the logging level for the API.

📋 Deploy the updated Compose spec in `labs/compose/rng/v2.yml` and use Compose to follow all the container logs.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f ./labs/compose/rng/v2.yml up -d

docker-compose -f ./labs/compose/rng/v2.yml logs -f
```

</details><br/>

> You'll see both containers get recreated because the spec has changed. Try the app now at http://localhost:8088 and it will work - and you'll see the app logs from Compose.

## Lab

Compose is used to define apps that span multiple containers - but the services are only related through the container networks.

Add an Nginx container to the RNG app definition, and another network. Configure the Nginx service to the new network and the original network.

Deploy the updated spec. What IP address(es) does the Nginx container have? Can you connect the containers together - reaching the Nginx container from one of the original containers, even though it was created later?

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```