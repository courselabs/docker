# Lab Solution

You can run a command inside the container to list files:

```
docker exec tls ls /certs
```

The [docker cp](https://docs.docker.com/engine/reference/commandline/cp/) command copies files between containers and the local filesystem.

To copy from the container called `tls`, use:

```
docker cp tls:/certs/server-cert.pem .

docker cp tls:/certs/server-key.pem .
```

You can also copy from the local machine into the container, but the target path needs to exist:

```
docker exec tls mkdir /certs-backup

docker cp server-cert.pem tls:/certs-backup/

docker cp server-key.pem tls:/certs-backup/

docker exec tls ls /certs-backup
```

> Back to the [exercises](README.md).