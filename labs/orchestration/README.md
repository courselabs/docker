# Orchestration with Docker Swarm

[Swarm Mode](https://docs.docker.com/engine/swarm/) is the orchestration platform built into the Docker Engine. It's easy to get started with because it uses the Docker Compose specification to model apps. 

Swarm isn't widely used any more, but it's a fully-featured container platform so it's a good introduction to orchestration concepts.


## Reference

- [Swarm Mode introduction](https://docs.docker.com/engine/swarm/)
- [Key swarm concepts](https://docs.docker.com/engine/swarm/key-concepts/)
- [Overlay networks](https://docs.docker.com/network/overlay/)


<details>
  <summary>CLI overview</summary>

Swarm mode uses the same Docker CLI, but adds new objects to manage:

```
docker swarm --help

docker node

docker stack

docker service
```

> A set of machines in a Swarm is called a cluster, and each machine is a node. You deploy applications from Compose specs, where each application is a stack, which has one or more services.

</details><br/>


## Switching to swarm mode

There are no extra dependencies for swarm mode - you just run a command to initialize the swarm, which turns that machine into a swarm manager:

```
docker swarm init
```

> The output gives you a token to use for other servers to join the Swarm as workers. We won't add any more, but you can run Swarms with thousands of nodes.

📋 Print a list of all the nodes in the swarm.

<details>
  <summary>Not sure how?</summary>

```
# `node` commands are available on the manager:
docker node ls
```

</details><br/>

That one `swarm init` command created all the infrastructure for a scalable container platform - including a resilient database which swarm uses to store application models.

## Storing configuration files

The cluster database is replicated between manager nodes when you run at scale, so the data is always available. Its also used to store static data for configuration files.

[Config objects](https://docs.docker.com/engine/swarm/configs/) store any kind of data, which you can surface into container filesystems. You create configs from a local file and the data is stored in the cluster database.

Create a config object called `rng-logging` from the local file path `labs/orchestration/config/logging.json`:

```
docker config create rng-logging labs/orchestration/config/logging.json
```

📋 List all the config objects and print the details of the new logging config.

<details>
  <summary>Not sure how?</summary>

```
# config is a top-level object in Swarm mode:
docker config ls

# inspecting with the pretty flag prints the contents:
docker config inspect --pretty rng-logging
```

</details><br/>

> Config objects are stored in plain text and they can be seen by anyone with access to the cluster.

Some configuration settings have sensitive details - like passwords or API keys. You can store them in the swarm but keep them protected using [secrets](https://docs.docker.com/engine/swarm/secrets/).

We've got a config object with logging settings, and now we'll use secrets for the random number app's other configuration files.

📋 Create a secret called `rng-api` from the file in `labs/orchestration/config/api.json`, and one called `rng-web` from the file in `labs/orchestration/config/web.json`.

<details>
  <summary>Not sure how?</summary>

```
# secrets are top-level objects like configs:
docker secret create rng-api labs/orchestration/config/api.json

docker secret create rng-web labs/orchestration/config/web.json
```

</details><br/>

> You work with secrets with similar commands to config objects. The difference is that they are encrypted in the database and they're only decrypted inside the container filesystem.

List the secrets in the Swarm and print the details of the API secret:

```
docker secret ls

docker secret inspect --pretty rng-api
```

Even cluster administrators can't see the contents of secrets. Config and secret objects are separate from containers so they're perfect for workflows where a configuration management team creates the configs before the app is deployed.

## Deploying applications

Swarm mode uses the Compose spec to model applications:

- [rng-v1.yml](./rng-v1.yml) - is the random number app, loading the config and secret objects into the container filesystem

Applications are deployed as [stacks](https://docs.docker.com/engine/swarm/stack-deploy/) in Swarm mode. Stacks are a grouping mechanism for services (which run as containers) and networks (which span across the whole swarm).

📋 Deploy a stack called `rng` from the v1 model and list all the services.

<details>
  <summary>Not sure how?</summary>

```
# stack deploy uses a desired-state approach:
docker stack deploy -c labs/orchestration/rng-v1.yml rng

# you'll see services for the API and web components:
docker service ls
```

</details><br/>

> Try the app at http://localhost:8090

You could deploy this same Compose spec to a swarm with 100 nodes and it would work in the same way. Networking is cluster-wide, so any node can receive a request on port 8090 and direct it to a container, even if the container is running on a different node.

## Managing applications

Services are an abstraction in the Compose model - with Docker Compose they create containers, but in swarm mode services are separate objects.

You can use services to manage your application:

```
# list the containers running the API service:
docker service ps rng_rng-api

# print the API container logs:
docker service logs rng_rng-api
```

Services are the unit of scale - and unlike Docker Compose you can have multiple containers listening on the same published port. Docker swarm takes care of receiving the traffic, and directing it to one of the containers.

- [rng-v2.yml](./rng-v2.yml) - increases the scale of the app with multiple replicas

The new spec also turns on failure mode for the API, so multiple calls will cause containers to exit. We'll use that to see how swarm mode keeps the app online.

📋 Update the stack to the v2 model and verify the services are running at scale.

<details>
  <summary>Not sure how?</summary>

```
# stack deploy is for updates and new releases:
docker stack deploy -c labs/orchestration/rng-v2.yml rng

# you'll see multiple replicas for each service:
docker service ls
```

</details><br/>

> The service list shows the number of replicas - each replica is a container running on a node in the Swarm. With multiple nodes the replicas would be spread across them for reliability.

Try the app now, repeatedly getting new random numbers. API containers will fail and the swarm will replace them. The app will still show failures while API containers are restarted, but in a more complex model you would add [healthchecks](https://docs.docker.com/compose/compose-file/compose-file-v3/#healthcheck) to avoid that.

___
## Cleanup

Cleanup by exiting swarm mode - that removes the database and all containers:

```
docker swarm leave -f
```