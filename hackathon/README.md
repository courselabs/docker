# Hackathon!

The hackathon is your chance to spend some decent time modelling and deploying a Kubernetes app on your own.

You'll use all the key skills you've learned in the course, and:

- 😣 you will get stuck
- 💥 you will have errors and broken apps
- 📑 you will need to research and troubleshoot

**That's why the hackathon is so useful!** 

It will help you understand which areas you're comfortable with and where you need to spend some more time.

And it will give you an app that you modelled yourself, which you can use as a reference next time you model a new app.

> ℹ There are three parts to the hackathon - you're not expected to complete them all. Just get as far as you can in the time, it's all great experience.

## Part 1 - Welcome to Widgetario

Widgetario is a company which sells gadgets. They want to run their public web app in containers. 

They've made a start - all the components are packaged into container images and published on Docker Hub. Your job is to get it running in Docker Compose so they can see how it works in a test environment.

Use this architecture diagram as the basis to model your YAML. It has the Docker image tags and the scale for each component:

![](/img/widgetario-architecture.png)

It's not much to go on, but it has all the information you need for the first stage.

<details>
  <summary>Hints</summary>

The component names in the diagram are the DNS names the app expects to use.

</details><br/>

When you're done you should be able to browse to port 8080 on your cluster and see this:

![](/img/widgetario-solution-1.png)

<details>
  <summary>Solution</summary>

If you didn't get part 1 finished, you can check out the sample solution from `hackathon/solution-part-1`. 

Deploy the sample solution and you can continue to part 2:

```
docker-compose -p hackathon -f hackathon/solution-part-1/docker-compose.yml up -d
```

</details><br/>

## Part 2 - Configuration

Well done! Seems pretty straightforward when you look at the YAML, but now we need to go to the next stage and stop using the default configuraion in the Docker images.


Also the front-end team are experimenting with a new dark mode, and they want to quickly turn it on and off with a config setting.



* products api price factor

* .net logging

* That feature flag for the UI can be set with an environment variable - `Widgetario__Theme` = `dark`.

<details>
  <summary>Hints</summary>

You have the app working from part 1, so you can investigate the current configuration by running commands in the Pods (`printenv`, `ls` and `cat` will be useful).

</details><br/>

When you've rolled out your update, the UI will be updated but the products and stock details should be the same:

![](/img/widgetario-solution-2.png)

<details>
  <summary>Solution</summary>

If you didn't get part 2 finished, you can check out the specs in the sample solution from `hackathon/solution-part-2`. 

Deploy the sample solution and you can continue to part 3:

```
docker-compose -p hackathon -f hackathon/solution-part-2/docker-compose.yml up -d
```

</details><br/>

## Part 3 - Reiliability & scale


- dependency map
- scale - 2x & 3x API
- restart
- resources 


<details>
  <summary>Hints</summary>
  
- can't scale with published ports

</details><br/>

The app won't look any different if you get your update right. If not, you'll need to dig into the logs.

<details>
  <summary>Solution</summary>

If you didn't get part 3 finished, you can check out the specs in the sample solution from `hackathon/solution-part-3`. 

Deploy the sample solution:

```
docker-compose -p hackathon -f hackathon/solution-part-3/docker-compose.yml up -d
```

</details><br/>

___ 

## Cleanup

```
docker-compose -p hackathon -f hackathon/solution-part-3/docker-compose.yml down
```