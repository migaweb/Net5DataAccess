#! /bin/bash

docker container run -d -p 27017:27017 --name mgdb mongo:4.0.10-xenial
