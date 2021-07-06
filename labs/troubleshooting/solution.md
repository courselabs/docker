
cp labs\troubleshooting\lab\solution.yml labs\troubleshooting\

docker-compose -f labs\troubleshooting\solution.yml up -d

http://localhost:8090


## Validation failures

- network name
- not enough CPUs

## Startup failures

- image name
- duplicate port mapping
- entrypoint command

> Starts but web app shows failure

# Runtime failures

Check API:

```
docker ps -a

docker logs troubleshooting_rng-api_1
```

> `the application was not found`

- volume overwrite app folder


