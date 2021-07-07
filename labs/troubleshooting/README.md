# Troubleshooting Containerized Apps

You'll spend a lot of your time with the Docker CLI and your Compose YAML files troubleshooting problems.

The CLIs validate commands and specs for correctness when you deploy them, but they don't check that your app will actually work.

Images can be misconfigured, the container environment could be incorrect, and components might be unable to reach each other.

## Lab

This one is all lab :) Try running this app - and make whatever changes you need to get the app running, so the containers run and the app works.

```
docker-compose -f labs/troubleshooting/compose.yml up -d
```

> Your goal is to browse to http://localhost:8090 and have a working random number generator.

Don't go straight to the solution! These are the sort of issues you will get all the time, so it's good to start working through the steps to diagnose problems.

> Stuck? Try [hints](hints.md) or check the [solution](solution.md).

___
## Cleanup

When you're done you can remove all the objects:

```
docker-compose -f labs/troubleshooting/compose.yml down
```