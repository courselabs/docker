# Lab Solution

It's all about having to run everything on a single server:

- when the server goes offline - planned or unplanned - you lose all your apps

- you'll update your container images regularly (at least monthly) to get OS and library updates, as well as new features. Updating your app means replacing containers, so there will be downtime while that happens because there are no other servers running additional containers

- same with config, any changes get deployed by replacing containers which means your app can be offline - and may be broken when it comes back if there's a config mistake

- your scaling options are limited to the CPU, memory and network ports on the machine. Most important is the port - only one container can listen on a port, so you can't run multiple copies of a public-facing component.


> Back to the [exercises](README.md).