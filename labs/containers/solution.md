# Lab Solution

Search for `java` on Docker Hub and the official image is the top hit.

You might start by downloading the Java package:

```
docker pull java
```

Run a container to check the Java version:

```
docker run java java -version
```

And then the JRE build for Alpine is the smallest:

```
docker pull java:8-jre-alpine
```

```
docker image ls java
```

> Latest is 640MB; 8 JRE is 100MB *but* both 5 years old...

## Using OpenJDK

The official Java page on Docker Hub is actually a redirect to the OpenJDK package, which is an open-source Java runtime.

But the `java` package is not the same as `openjdk` - and you should use `openjdk` because it's more up-to-date:

```
docker pull openjdk
docker pull openjdk:8-jre-alpine
```

The default image tag is much more recent, and both images are smaller:

```
docker image ls openjdk
```

> Back to the [exercises](README.md).