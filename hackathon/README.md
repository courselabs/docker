# Hackathon!

The hackathon is your chance to spend some decent time modelling and deploying a Docker Compose app on your own.

You'll use all the key skills you've learned, and:

- 😣 you will get stuck
- 💥 you will have errors and broken apps
- 📑 you will need to research and troubleshoot

**That's why the hackathon is so useful!** 

It will help you understand which areas you're comfortable with and where you need to spend some more time.

And it will give you an app that you modelled yourself, which you can use as a reference next time you model a new app.

> ℹ There are four parts to the hackathon - you're not expected to complete them all. Just get as far as you can in the time, it's all great experience.

## Part 1 - Welcome to Widgetario

Widgetario is a company which sells gadgets. They want to run their public web app in containers - so the first thing is to build some Docker images.

There are four components to the app, each will need its own Docker image. The source code is in the `hackathon/src` folder, and each component has a Dockerfile which needs to be completed:

- Products database - a Postgres database, built with some sample data ([db/postgres/Dockerfile](./src/db/postgres/Dockerfile))

- Products API - a Java REST API which reads from the Products database ([products-api/java/Dockerfile](./src/products-api/java/Dockerfile))

- Stock API - a Go REST API which also reads from the Products database ([stock-api/golang/Dockerfile](./src/stock-api/golang/Dockerfile))

- Website- an ASP.NET Core website which reads from the Products and Stock API ([web/dotnet/Dockerfile](./src/web/dotnet/Dockerfile))

You can use `docker build` commands to check all your components, but it might be easier to add build sections to [docker-compose.yml](./src/docker-compose.yml). Then you can use `docker-compose build` to build all the images.

<details>
  <summary>💡 Hints</summary>

We have the source code for so you'll want to use multi-stage builds for the application components (except the database). The build steps are already written in scripts, so your job will be to find the right base images from Docker Hub and copy in the correct folder structure.

</details><br/>

All the components should build without any errors, and you should have four new Docker images.

<details>
  <summary>🎯 Solution</summary>

If you didn't get part 1 finished, you can check out the sample solution from `hackathon/solution-part-1`:

- Products database [db/postgres/Dockerfile](./solution-part-1/db/postgres/Dockerfile)

- Products API [products-api/java/Dockerfile](./solution-part-1/products-api/java/Dockerfile)

- Stock API [stock-api/golang/Dockerfile](./solution-part-1/stock-api/golang/Dockerfile)

- Website [web/dotnet/Dockerfile](./solution-part-1/web/dotnet/Dockerfile)

Build from the sample solution and you can continue to part 1:

```
docker-compose -f hackathon/solution-part-1/docker-compose.yml build
```

</details><br/>

## Part 2 - Application Modelling

We've made a good start - all the components are packaged into container images now. Your job is to get it running in Docker Compose so Widgetario can see how it works in a test environment.

Use this architecture diagram as the basis to model your YAML:

![](/img/widgetario-architecture.png)

> The diagram shows the Docker image tags for each component and the port(s) to publish. You can use these images from Docker Hub, or your own images from part 1.

It's not much to go on, but it has all the information you need for this stage.

<details>
  <summary>💡 Hints</summary>

The component names in the diagram are the DNS names the app expects to use. It can take 30 seconds or so for all the components to be ready, so you may have to refresh a few times before you see the website.

</details><br/>

When you're done you should be able to browse to http://localhost:8080 and see this:

![](/img/widgetario-solution-1.png)

<details>
  <summary>🎯 Solution</summary>

If you didn't get part 2 finished, you can check out the sample solution from [hackathon/solution-part-2](./solution-part-2/docker-compose.yml). 

Deploy the sample solution and you can continue to part 3:

```
docker-compose -p hackathon -f hackathon/solution-part-2/docker-compose.yml up -d
```

</details><br/>

## Part 3 - Configuration

Well done! Seems pretty straightforward when you look at the YAML, but now we need to go to the next stage and stop using the default configuration in the Docker images.

Here's what we want to do:

- in the products API component, set the `PRICE_FACTOR` configuration value to `1.5` to increase the sale prices

- in the web application, set a feature flag to run the UI in dark mode, the setting is called `Widgetario__Theme`

- the web app writes logs to a file; mount a volume so when the app writes files in `/logs` in the container, they actually get written inside the `hackathon` directory on your machine

- increase the default logging level for the web app to `Debug`. You'll need to do this by loading a JSON config file like [logging.json](./solution-part-3/config/web/logging.json) into the contaier filesystem at `/app/config`.

<details>
  <summary>💡 Hints</summary>

You have the app working from part 2, so you can investigate the current configuration by running commands in the Pods (`printenv`, `ls` and `cat` will be useful).

</details><br/>

When you've rolled out your update, the UI will be updated but the products and stock details should be the same:

![](/img/widgetario-solution-2.png)

You should be able to see the logs from the web app in your local filesystem:

```
cat ./logs/web/app.log
```

<details>
  <summary>🎯 Solution</summary>

If you didn't get part 3 finished, you can check out the specs in the sample solution from [hackathon/solution-part-3](./solution-part-3/docker-compose.yml). 

Deploy the sample solution and you can continue to part 4:

```
docker-compose -p hackathon -f hackathon/solution-part-3/docker-compose.yml up -d

# test the app and you'll be able to see the logs with:
cat ./hackathon/solution-part-3/logs/web/app.log
```

</details><br/>

## Part 4 - Reiliability & scale

The app is looking good, but we want to see if Compose is the right tool to run in production.

Add some reliability to the app:

- the database should run in 1 container with 1 CPU core and 250MB memory

- the products API can run in 2 containers, with 0.5 cores and 400MB memory

- the stock API can run in 3 containers, with 0.25 cores and 200MB memory

- the web app should run in 1 container with 0.5 cores and 300MB memory

- all services should be set to restart if the containers exit

- include a dependency map so the database container starts first, and the web container only starts when the API containers are running.

<details>
  <summary>💡 Hints</summary>
  
Remember ports are exclusive-use resources, so if you were publishing ports for components which need to scale then you need to change that.

</details><br/>

The app won't look any different if you get your update right. If not, you'll need to dig into the logs.

<details>
  <summary>🎯 Solution</summary>

If you didn't get part 4 finished, you can check out the specs in the sample solution from [hackathon/solution-part-4](./solution-part-4/docker-compose.yml). 

Deploy the sample solution:

```
docker-compose -p hackathon -f hackathon/solution-part-4/docker-compose.yml up -d
```

</details><br/>

> It's still not perfect, because the app doesn't respond immediately after a new deployment. And we really need to be able to scale the web component. Who's for learning [Kubernetes](https://k8sfun.courselabs.co) next? 

___ 

## Cleanup

```
docker-compose -p hackathon -f hackathon/solution-part-4/docker-compose.yml down
```