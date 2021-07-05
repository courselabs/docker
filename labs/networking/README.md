# Container Networking

Docker manages networking - containers can be attached to Docker networks, and containers on the same network can reach each other by IP address.

Docker also runs a DNS server, so containers on the same network can find each other using the container name as the DNS host name.

## Reference

<details>
  <summary>CLI overview</summary>


```

```

</details><br/>


## Container networking

Docker networks are first-class citizens. We've been using them already, and Docker Compose creates them when we deploy an app:

- []compose.yml - defines a network with no special configuration

📋 Run the app, list all networks and print the details of the new application network.

<details>
  <summary>Not sure how?</summary>

```
docker-compose -f labs/networking/compose.yml up -d

# networks are a top-level object in the Docker CLI:
docker network ls

docker network inspect networking_app-net
```

</details><br/>

> You'll see a lot of details about the network - including containers which are attached to it. The API and web containers are there, with IP addresses in the same network range so they should be able to connect.

The web container image has some networking tools installed, so we can try and find out what's going wrong:

- `nslookup` to query the DNS server and get an IP address
- `ping` to test the network connection to a remote machine
- `wget` to make HTTP requests

Use them to check the network setup:

```
# run a DNS lookup for the API service name:
docker exec networking_rng-web_1 nslookup rng-api

# ping the IP address of the API container :
docker exec networking_rng-web_1 ping -c3 <rng-api-ip>

# try calling the API inside the web container:
docker exec networking_rng-web_1 wget -qO- rng-api/rng
```

> The Docker DNS server returns the IP address for container names. The web container can access the API container on the standard HTTP port 80.

## Publishing ports

Containers connected to the same network can access each other by IP address or name. There's no restriction to the traffic - any ports can be used between containers, they don't need to be published.

Traffic from outside the container network is restricted, and the only way to send traffic into a container is by publishing ports. Docker listens on the published port and sends the traffic into the target container port.

Try running a container with a simple web server:

```
docker run -d -p 8080:80 sixeyed/whoami:21.04

curl localhost:8080
```

The `-p` flag - lowercase p - publishes a specific port, so here Docker listens on port 8080 and sends traffic into the container on port 80.

Ports are single-use resources, only one process can listen on them. If you repeat the command you'll get a `port is already allocated` failure message.

📋 Run some more containers from the same image, using different ports (`8081`, `8082` and `8083` should be free).

<details>
  <summary>Not sure how?</summary>

```
docker run -d -p 8081:80 sixeyed/whoami:21.04
docker run -d -p 8082:80 sixeyed/whoami:21.04
docker run -d -p 8083:80 sixeyed/whoami:21.04
```

</details><br/>

Port mapping lets you listen on any free port on your machine, even though multiple containers are all listening on port 80 internally. Check the running containers: 

```
docker ps
```

They each publish to different ports, so you can see the response from each container:

```
curl localhost:8081
curl localhost:8082
curl localhost:8083
```


## Customizing container networks

Docker sets the IP address, DNS server and other network options for a container - and you can configure those too.

There's a script we can mount in a container to print the network settings - [network-info.sh](). Save the path to the script in a variable:

```
# on macOS/Linux:
scriptsPath="${PWD}/labs/networking/scripts"

# OR with PowerShell:
$scriptsPath="${PWD}/labs/networking/scripts"
```

Now run a basic Alpine container to run that script:

```
docker run -v ${scriptsPath}:/scripts alpine sh /scripts/network-info.sh
```

> These network settings are all built by Docker

📋 Repeat the run command with extra settings to specify a DNS server of `1.1.1.1` and hostname of `alpine1`.

<details>
  <summary>Not sure how?</summary>

```
docker run --dns 1.1.1.1 --hostname alpine1 -v ${scriptsPath}:/scripts alpine sh /scripts/network-info.sh
```

</details><br/>

> This is useful for apps which need a particular configuration.

You can also set an IP address - but first you need to create a network with specific IP address range: 

```
docker network create --subnet=10.10.0.0/16 dockerfun

docker run --network dockerfun --ip 10.10.0.100 -v ${scriptsPath}:/scripts alpine sh /scripts/network-info.sh
```

> IP addresses are set at random by Docker - you'll need to use a custom subnet range if Docker's IP address collides with your network or VPN.

</details><br/>

You can configure custom networking in Docker Compose too:

- labs\networking\compose-network.yml - the RNG app using a custom subnet range and IP addresses

docker-compose -f labs\networking\compose-network.yml up -d

> localhost:8090, works

docker logs networking_rng-web_1

docker exec networking_rng-api_1 /scripts/network-info.sh

## Lab 

TODO

___
## Cleanup

Cleanup by removing all containers:

```
docker rm -f $(docker ps -aq)
```