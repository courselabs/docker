FROM alpine:3.13 AS base
RUN echo 'Adding deps...' > /deps.txt

FROM base AS build
RUN echo 'Building...' > /build.txt

FROM build AS test
COPY --from=build /build.txt /build.txt
RUN echo 'Testing...' >> /build.txt

FROM base
COPY --from=build /build.txt /build.txt
CMD cat /deps.txt && cat /build.txt