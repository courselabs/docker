# Lab Solution

cd labs/compose-model

cp ./lab/.env .
cp ./lab/lab.yml ./rng

docker-compose up -d

> http://localhost:8390

docker logs rng-lab_rng-api_1

> Entry for each request