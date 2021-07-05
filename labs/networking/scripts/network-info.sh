#!/bin/sh

echo ''

echo "** Hostname **"
echo $(hostname)
echo ''

echo "** IP address **"
echo  $(hostname -i)
echo ''

echo "** DNS server: **"
cat /etc/resolv.conf | grep nameserver | cut -c 12-
echo ''