# Lab Hints

You can't scale up the most recent deployment, because the Compose spec uses a specific IP address for the API container. Roll back to the [compose.yml](./compose.yml) spec and you can scale up now.

Follow the logs of the API containers and you should see them all being used when you get lots of random numbers from the website.

Check the details of all the API containers to see the networking setup Compose applies to get that load-balancing behaviour.

> Need more? Here's the [solution](solution.md).