# Running Docker Locally

Docker runs as a background service on server operating systems, but in a local environment the easiest option is Docker Desktop.

## Docker Desktop - Mac or Windows

If you're on macOS or Windows 10, Docker Desktop is for you:

- [Install Docker Desktop](https://www.docker.com/products/docker-desktop)

The download and install takes a few minutes. When it's done, run the _Docker_ app and you'll see the Docker whale logo in your taskbar (Windows) or menu bar (macOS).

> On Windows 10 the install may need a restart before you get here.

## **OR** Docker Engine - Linux

<details>
  <summary>Running Docker on Linux</summary>

Docker Engine is the background service which runs containers. You can install it - along with the Docker command line - for lots of different Linux distros:

 - [Install Docker Engine](https://docs.docker.com/engine/install/)
 - [Install Docker Compose](https://docs.docker.com/compose/install/)

> If you're using WSL on Windows 10, it's much easier to use Docker Desktop which integrates with your WSL distro.

</details><br />

## Check your setup

When you have Docker running you should be able to run this command and get some output:

```
docker version
```

I'm using Docker Desktop on Windows and mine says:

```
Client:
 Cloud integration: 1.0.14
 Version:           20.10.6
 API version:       1.41
 Go version:        go1.16.3
 Git commit:        370c289
 Built:             Fri Apr  9 22:49:36 2021
 OS/Arch:           windows/amd64
 Context:           default
 Experimental:      true

Server: Docker Engine - Community
 Engine:
  Version:          20.10.6
  API version:      1.41 (minimum version 1.12)
  Go version:       go1.13.15
  Git commit:       8728dd2
  Built:            Fri Apr  9 22:44:56 2021
  OS/Arch:          linux/amd64
...
```

And then:

```
docker-compose version
```

My output is:

```
docker-compose version 1.29.1, build c34c88b2
docker-py version: 5.0.0
CPython version: 3.9.0
OpenSSL version: OpenSSL 1.1.1g  21 Apr 2020
```

> Your details and version numbers may be different - that's fine. If you get errors then we need to look into it, because you'll need to have Docker running for all of the exercises.

> ❗ If you're running Docker Desktop on Windows, make sure you're in _Linux container mode_. This is the default mode, but if you've changed to using Windows containers (from the whale toolbar menu), then you'll need to switch back.