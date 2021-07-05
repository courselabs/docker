

# Set up Kubernetes

Follow the instructions from the [Kubernetes Fundamentals setup](https://k8sfun.courselabs.co/setup/).

# Simple Pod

- labs\kubernetes\pods\sleep.yaml

kubectl apply -f labs\kubernetes\pods\sleep.yaml

kubectl get pods

kubectl describe po sleep

kubectl exec sleep -- sh hostname

docker ps

docker rm -f <container-id>

kubectl get pods


## Services

- labs\kubernetes\services\whoami-nodeport.yaml

kubectl apply -f labs\kubernetes\services\

kubectl get services

kubectl describe svc whoami-np

curl http://localhost:30080


## Services over Pods

- labs\kubernetes\pods\whoami.yaml

kubectl apply -f labs\kubernetes\pods\

kubectl get endpoints whoami-np

curl http://localhost:30080

docker ps

