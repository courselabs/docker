#!/bin/sh

mvn -B dependency:go-offline
mvn package
