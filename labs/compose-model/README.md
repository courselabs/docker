# Modelling apps with Compose

You can think of Compose as documentation which replaces `docker run` - all the options you would put in the run commands are specified in the Compose file which becomes living documentation. That includes setting up the container environment with variables and voume mounts.

The Compose spec also includes higher-level modelling for configuration files, using `config` and `secret` objects. That helps to make it clear that you're modelling configuration instead of storage. Compose also has features for modelling and running the same app in different configurations on a single machine.

## Reference

- [Environment variables in Compose](https://docs.docker.com/compose/environment-variables/)
- [Compose configs](https://docs.docker.com/compose/compose-file/compose-file-v3/#configs)
- [Merging multiple Compose files](https://docs.docker.com/compose/extends/)


## Modelling configuration in Compose


docker-compose -f labs/compose-model/rng/v1.yml up -d


docker-compose -f labs/compose-model/rng/v2.yml up -d

> Up-to-date, same container def (env files expanded)


Hack - secrets

docker-compose -f labs/compose-model/rng/v3.yml up -d

docker inspect rng_rng-web_1

> Mounts



> You'll see the containers being created.

Compose creates containers and networks with the `rng` prefix, which is the name of the folder the Compose files are in. As long as you're in the same folder you can manage the containers without specifying file names:

```
docker-compose ps

docker-compose logs
```




## Running multiple environments


- override files

docker-compose -p rng-dev -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/dev.yml up -d

- you merge Compose files with multiple `-f` flags - the files to the right can override settings from files to the left

localhost:8090, 0-50

docker-compose -p rng-dev -f labs/compose-model/rng/core.yml  logs

docker-compose -p rng-dev -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/dev.yml logs

> dev debugging


docker-compose -p rng-test -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/test.yml up -d

localhost:8190, 0-500

docker-compose -p rng-test -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/test.yml logs

> info debugging


docker-compose -p rng-prod -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/prod.yml up -d

localhost:8290, 0-5000

docker-compose -p rng-prod -f labs/compose-model/rng/core.yml -f labs/compose-model/rng/prod.yml logs

> warn debugging



Compose adds the prefix so it can identify the containers it created - but you can use the same approach to run multiple copies of an application.

You could do that to run different test environments on a single machine:


You can override the default prefix by specifying a *project name* in Compose commands, so instead of updating the existing app, Compose will deploy a new one.

📋 Run the app in the test configuration by merging the Compose files and using the project name `rng-test`

<details>
  <summary>Not sure how?</summary>

```
docker-compose -p rng-test -f ./docker-compose.yml -f ./docker-compose-test.yml up -d
```

</details><br/>

> If you don't add a project name, Compose uses the folder name - which is already being used for the original v3 deployment. Adding a project name lets you run another copy of the app, and you'll see Compose creating new containers.


## Lab
 default project name and files

 from labs/compose model directory

 run with docker-compose up -d

 browse to localhost:8390

 check logs for container named rng-lab_rng-api_1

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```
