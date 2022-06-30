# Lab Solution

The lab model for the app just sets new ports and uses the existing dev config files:

- [lab/compose.yml](./lab/compose.yml)

The Compose env file sets a project name and default files - using the core and the lab override:

- [lab/.env](./lab/.env)

Switch to the lab folder:

```
cd labs/compose-model
```

Copy in the sample solution:

```
cp ./lab/.env .
cp ./lab/compose.yml ./rng/lab.yml
```

Start the app:

```
docker-compose up -d
```

> Try it out at http://localhost:8390

Check API logs:

```
docker logs rng-lab_rng-api_1
```

> Shows the info and debug level logs

> Back to the [exercises](README.md).