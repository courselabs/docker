FROM dockerfun/curl:v2

COPY *.sh /scripts/

ENV TEST_DOMAIN=dockerfun.courselabs.co \
    TEST_SCHEME=https

RUN /scripts/network-test.sh

ENTRYPOINT ["/scripts/network-test.sh"]