# Lab Solution

My solution is here:

- [solution.yml](./lab/solution.yml)

Copy it to the main folder so the config paths are correct, and the app will work:

```
cp labs/troubleshooting/lab/solution.yml labs/troubleshooting/

docker-compose -f labs/troubleshooting/solution.yml up -d
```

> Use the app at http://localhost:8090

## Validation failures

1. The image name is incorrect for the web component. You'll see a `manifest unknown` error.

2. Not enough CPUs - the web application request `25` CPUs, which is a typo - it should be `2.5`. Unless you have a mega powerful machine, Docker can't allocate enough CPUs to create the container.

## Startup failures

1. Incorrect entrypoint for the API. It's specifying the command to run but `dontet` is a typo. You'll see `executable file not found in $PATH` - it should be `dotnet`.

2. Duplicate port mapping - both the web app and API are trying to publish port `8089`. You'll see `port is already allocated` - the web app should be publishing to port `8090`.

## Runtime failures

1. The API container exits after starting. Check the logs and you'll see `the application was not found`. The volume mount is incorrect - it's loading the config folder into the `/app` folder, which overwrites the contents from the image so there is no application binary. The volume target should be `/app/config`.

## Networking failures

1. Try using the website and you'll get the `RNG service unavailable!` error. The web container logs show you the app is using the correct URL, but if you run `nslookup` the API container can't be found. The spec uses two different networks, so the containers aren't connected - the web container needs to attach to the `app-net` network.


> Back to the [exercises](README.md).