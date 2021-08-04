FROM maven:3.6.3-jdk-11 AS builder

WORKDIR /usr/src/api
COPY . .

RUN chmod +x restore.sh && ./restore.sh
RUN chmod +x build.sh  && ./build.sh

# app
FROM openjdk:11.0.12-jre-slim-buster

WORKDIR /app
COPY --from=builder /usr/src/api/target/products-api-0.1.0.jar .

EXPOSE 80
ENTRYPOINT ["java", "-jar", "/app/products-api-0.1.0.jar"]

ENV JRE_VERSION="11.0.12" \
    APP_VERSION="0.4.0"