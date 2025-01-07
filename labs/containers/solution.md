# Lab Solution

Search for `java` on Docker Hub and the official image is the top hit, but open the page at https://hub.docker.com/_/java and you will see it's been deprecated.

That means the `java` image (and the `openjdk` image which replaced it) are no longer being maintained. The images are old, so you should avoid them.

There are a few alternatives with active maintainers (e.g. Amazon and IBM). My preference is for [eclipse-temurin](https://hub.docker.com/_/eclipse-temurin) which is an open-source build of OpenJDK.

You might start by copying the command from the Docker Hub page, which will download the default image:

```
docker pull eclipse-temurin
```

Run a container to check the Java version:

```
docker run eclipse-temurin java -version
```

> The output will show the OpenJDK and JRE versions

Check the [tags page on Docker Hub](https://hub.docker.com/_/eclipse-temurin/tags) and search for `alpine` and you'll see there are JRE and IDK versions. 

The JRE build for Alpine is the smallest - the tag contains the version number, so this pulls a small JRE image for OpenJDK 23:

```
docker pull eclipse-temurin:23-jre-alpine
```

Check the sizes:

```
docker image ls eclipse-temurin
```

> Your output may be different, but mine shows the default ("latest") image is 478MB; Alpin JRE is 206MB...

> Back to the [exercises](README.md).