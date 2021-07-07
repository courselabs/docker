# Lab Hints

Fixing apps is a process of checking the status of running objects and seeing what's wrong, then fixing up the YAML and redeploying.

You'll find different classes of problem with this app:

- validation failures which stop containers being created
- startup failures which mean containers can't run
- runtime failures where the container is running but the app doesn't work
- networking failures where containers can't reach each other

Use the normal command lines to deploy and check the output carefully. 

Once the containers are running you can print the logs and run commands inside the containers to check the filesystem.

> Need more? Here's the [solution](solution.md).