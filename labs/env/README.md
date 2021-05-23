# Constructing the Container Environment

Docker creates the virtual environment for a container. You can specify how parts of the environment should be set, which lets you prepare the container for your applications.

## CLI Reference

Container environments are static - the details are fixed for the life of the container. You configure the environment in the `docker run` command:

- [Setting environment variables](https://docs.docker.com/engine/reference/commandline/run/#set-environment-variables--e---env---env-file)
- [Mounting volumes](https://docs.docker.com/engine/reference/commandline/run/#mount-volume--v---read-only)
- [Applying resource constraints](https://docs.docker.com/config/containers/resource_constraints/)

<details>
  <summary>CLI overview</summary>

There are dozens of options for running a container:

```
docker run --help
```

The arguments we'll use here are:

- `-e` to set the value for a single environment variable
- `--env-file` to set multiple environment variables from data in a text file
- `-v` to mount a local directory into the container's filesystem
- `--cpus` and `--memory` to specify the compute resources available to the container

</details><br/>

## Environment variables

Environment variables are key-value pairs set in the OS. Applications can read them and they're often used for configuration settings.

`printenv` is the Linux command to print them all:

```
docker run alpine printenv

docker run java:8-jdk-alpine printenv
```

> You'll see variables set by the OS and in the container package.

📋 Run a new container - set your own environment variable and print all the variables.

<details>
  <summary>Not sure how?</summary>

```
# -e adds a new environment variable
docker run -e DOCKERFUN=env alpine printenv
```

</details><br/>

> Environment variables can't be edited, they're set for the lifetime of container.

You can add as many environment variables as you need, and you can also *override* default variables for the container:

```
docker run -e DOCKERFUN=env -e LANG=C.UTF-16 openjdk:8-jre-alpine printenv
```

> This overrides the default language setting in the Java container.

If you're setting lots of variables, it's easier to store them all in a file like [exercises.env](/labs/env/exercises.env) and pass that to the container as an environment file.

📋 Run container loading `labs/env/exercises.env` as an environment file.

<details>
  <summary>Not sure how?</summary>

```
# check the contents of the local file:
cat labs/env/exercises.env

# run a container loading that file as environment variables:
docker run --env-file labs/env/exercises.env alpine printenv
```

</details><br/>

> Env file contents overwrite default values, but you can overwrite them with `-e`

## Container filesystem

Containers filesystems are virtual disks, put together by Docker. Every container starts with the disk contents set up in the container package.


📋 Run a background Nginx container called `nginx`.

<details>
  <summary>Not sure how?</summary>

```
# alpine is the smallest variant but any will do:
docker run -d --name nginx nginx:alpine
```

</details><br/>

You can connect to a detached container and run commands in it - useful for exploring the filesystem:

```
docker exec -it nginx sh

ls /usr/share/nginx/html/

cat /usr/share/nginx/html/index.html

exit
```

> The container has the Nginx web server installed, and some default HTML content.

You can mount a directotry from your local machine into the container filesystem. You can use that to add new content, or to override existing files.

[index.html](/labs/env/html/index.html) is a web page you can display from Nginx when you mount the local folder as a volume:

```
# put the full local path in a variable - on macOS/Linux:
htmlPath="${PWD}/labs/env/html"

# OR with PowerShell:
$htmlPath="${PWD}/labs/env/html"

# run a container mounting the local volume to the HTML directory:
docker run -d -p 8081:80 -v ${htmlPath}:/usr/share/nginx/html --name nginx2 nginx:alpine
```

- `-v` mounts a local directory to the container - you need to use a full path for the source, the variables mean we can use the same Docker command on any OS

> Browse to http://localhost:8081 and you'll see the custom HTML response.

The container is reading data from your local machine. You can edit [index.html](/labs/env/html/index.html) and when you refresh your browser you'll see your changes straight away.

## Compute resources

You can restrict the amount of compute power containers have. By default there's no restriction, so the container sees all the CPUs and memory your machine has.

Run a compute-intensive app without restrictions:

```
docker run -d -p 8031:80 --name pi kiamol/ch05-pi -m web 
```

- the `-m` flag gets passed to the application inside the container, it's not a container configuration.


Browse to http://localhost:8031/pi?dp=50000 - this calculates Pi to 50K decimal places. On my machine that takes ~3s.

> You can't change CPU resources for a container, they're fixed like the other environment setup.

📋 Inspect the details of the container to see the complete configuration.

<details>
  <summary>Not sure how?</summary>

```
docker inspect pi
```

</details><br/>

> All CPU and memory quotas are set to 0, meaning they're not restricted.


Try running a the same app in a container with only 200Mb of memory and 1/4 of a CPU core:

```
docker rm -f pi

docker run -d -p 8031:80 --name pi --memory 200m --cpus 0.25 kiamol/ch05-pi -m web 
```

> Refresh http://localhost:8031/pi?dp=50000 and it will take much longer - over 13s on my machine

You can print specific parts of the container configuration using [formatting](https://docs.docker.com/config/formatting/):

```
docker container inspect --format='Memory: {{.HostConfig.Memory}}b, CPU: {{.HostConfig.NanoCpus}}n' pi
```

> Memory is returned in bytes, and CPU as nano-cores (1-billionth of a core!).

## Lab

We can run a container to generate TLS certificates, but the certs are created inside the container filesystem.

In this lab your job is to copy the TLS certificate and key from the container onto your local machine.

Start by generating certs in a new named container:

```
docker run --name tls kiamol/ch15-cert-generator

# Ctrl-C to exit
```

Now copy the `server-cert.pem` and `server-key.pem` files from the `/certs` folder in the container onto your machine.

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).
___
## **EXTRA** Configuring network resources

Docker sets the IP address, DNS server and other network options for a container - and you can configure those too.

There's a script we can mount in a container to print the network settings - save the path to the script in a variable:

```
# on macOS/Linux:
scriptsPath="${PWD}/labs/env/scripts"

# OR with PowerShell:
$scriptsPath="${PWD}/labs/env/scripts"
```

Now run a basic Alpine container to run that script:

```
docker run -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

> These network settings are all built by Docker

📋 Repeat the run command with extra settings to specify a custom DNS server and hostname.

<details>
  <summary>Not sure how?</summary>

```
docker run --dns 1.1.1.1 --hostname alpine1 -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

</details><br/>

> This is useful for apps which need a particular configuration.

You can also set an IP address - but first you need to create a netwotrk with specific IP address range: 

```
docker network create --subnet=10.10.0.0/16 dockerfun

docker run --network dockerfun --ip 10.10.0.100 -v ${scriptsPath}:/scripts alpine sh /scripts/print-network.sh
```

> IP addresses are set at random by Docker - you'll need to use a custom subnet range if Docker's IP address collides with your network or VPN.

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```