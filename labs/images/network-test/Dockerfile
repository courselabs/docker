FROM dockerfun/curl:v2

COPY *.sh /scripts/

ENV TEST_DOMAIN=dockerfun.courselabs.co \
    TEST_SCHEME=https
    
RUN chmod +x /scripts/network-test.sh && \
    /scripts/network-test.sh

ENTRYPOINT ["/scripts/network-test.sh"]