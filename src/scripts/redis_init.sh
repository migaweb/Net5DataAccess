#! /bin/bash

docker container run -d --name redis-cache -p 6379:6379 redis:5.0-alpine
