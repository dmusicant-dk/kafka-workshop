#!/usr/bin/env pwsh

$cmd = $args[0]
docker run --network=host --rm --entrypoint="/bin/bash" confluentinc/cp-zookeeper:7.3.0 -c "$cmd"
