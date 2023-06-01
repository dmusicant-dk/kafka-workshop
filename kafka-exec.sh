#!/usr/bin/env bash

docker run --network=host --rm --entrypoint="/bin/bash" confluentinc/cp-zookeeper:7.3.0 -c "$@"
