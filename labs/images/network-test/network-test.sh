#!/bin/sh

echo ''

echo "** nslookup: $TEST_DOMAIN **"
nslookup $TEST_DOMAIN
echo ''

echo "** curl: $TEST_SCHEME://$TEST_DOMAIN **"
curl --head $TEST_SCHEME://$TEST_DOMAIN
echo ''