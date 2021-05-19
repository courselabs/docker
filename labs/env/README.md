# Constructing the Container Environment

## CLI Reference

## Environment variables

```
docker run alpine printenv
```

```
docker run -e DOCKERFUN=env alpine printenv

docker run -e DOCKERFUN=env -e RELEASE=21.05 alpine printenv
```

```
cat labs/env/exercises.env

docker run --env-file labs/env/exercises.env alpine printenv
```

## Container filesystem

- mount
- ro
- overwrite

## Compute resources

- memory
- cpu

## Network resources

- ports
- ip
- dns

## Lab

- cmd/entrypoint