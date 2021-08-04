FROM golang:1.15.14-alpine3.14 AS builder
ENV CGO_ENABLED=0

WORKDIR /go/stock-api
COPY . .

RUN chmod +x restore.sh && ./restore.sh
RUN chmod +x build.sh  && ./build.sh

# app
FROM alpine:3.14

ENV POSTGRES_CONNECTION_STRING="host=products-db port=5432 user=postgres password=widgetario dbname=postgres sslmode=disable" \
    CACHE_EXPIRY_SECONDS="45" \
    GOLANG_VERSION="1.15.14" \
    APP_VERSION="0.4.0"

EXPOSE 8080
CMD ["/app/server"]

WORKDIR /cache
WORKDIR /app
COPY --from=builder /server .