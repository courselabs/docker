

# Set up Kubernetes

Install [k3d](https://k3d.io/#installation):

```
# Mac
brew install k3d

# Windows
choco install k3d

# Linux
curl -s https://raw.githubusercontent.com/rancher/k3d/main/install.sh | bash
```

Create a cluster:

```
k3d cluster create dockerfun -p "30000:30000@server[0]"
```

```
kubectl get nodes
```

# Simple Pod

- labs\kubernetes\pods\sleep.yaml

kubectl apply -f labs\kubernetes\pods\sleep.yaml

kubectl get pods

> ContainerCreating -> Running

kubectl describe po sleep

> container id

kubectl exec sleep -- hostname

kubectl exec sleep -- kill 1

kubectl get pods

> Restart count

kubectl describe po sleep

> New container ID 


## Services

- labs\kubernetes\services\whoami-nodeport.yaml

kubectl apply -f labs\kubernetes\services\

kubectl get services

kubectl describe svc whoami-np

> IP address; endpoints `<none>`

curl http://localhost:30000

kubectl get pods -l app=whoami


## Services over Pods

- labs\kubernetes\pods\whoami.yaml

kubectl apply -f labs\kubernetes\pods\

> sleep unchanged; whoami created

kubectl get pods -l app=whoami -o wide

kubectl get endpoints whoami-np

> Pod IP in endpoint list

curl http://localhost:30000


## Cleanup

k3d cluster delete dockerfun