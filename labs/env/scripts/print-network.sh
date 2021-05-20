#!/bin/sh

echo ''
echo '** Hostname **'
hostname
echo ''

echo '** IP **'
ip a s eth0 | grep 'inet ' | cut -d' ' -f6| cut -d/ -f1
echo ''

echo '** DNS **'
cat /etc/resolv.conf | grep nameserver | cut -c 12-
echo ''