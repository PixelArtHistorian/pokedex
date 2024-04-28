# Pokedex

A basic pokedex API

## Getting started

Before getting started with your pokedex you will need a couple of things

- Install and configure [git](https://git-scm.com/downloads) on your machine.
- Install the .NET 8 sdk, you can find it [here](https://dotnet.microsoft.com/en-us/download).
- Install docker and docker compose, installing [Docker Desktop](https://www.docker.com/get-started/) for your preferred OS should give you everything you need.

Once all dependencies are installed, clone the repository or download the zip file containing the source code.
You can either run the application using your IDE of choice or run it as a container.
The repository contains a docker compose file that builds the image and runs it inside a container called buzzwole.
To run it navigate to the same directory that hosts the docker-compose.yml file and run the following command:

```console
docker-compose up -d
```
the application should start on localhost:1025.

To test that everything is working fine you can try the following commands using either httpie or curl:

HTTPIE

```console
    http GET http://localhost:1025/pokemon/jigglypuff
```

Expected output:
```bash 
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Fri, 26 Apr 2024 14:08:22 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
    "description": "When its huge eyes light up, it sings a mysteriously soothing melody that lulls its enemies to sleep.",
    "habitat": "grassland",
    "isLegendary": false,
    "name": "jigglypuff"
}
```

CURL
```console
curl http://localhost:1025/pokemon/jigglypuff
```
```json
{"name":"jigglypuff","description":"When its huge eyes light up, it sings a mysteriously soothing melody that lulls its enemies to sleep.","habitat":"grassland","isLegendary":false}
```
## Production readiness

This API is not production ready and is missing some key features.

### Monitoring and obsvervability

The API in its current state does not offer any support for monitoring metrics and log analytics, logs should be enriched with metadata to perform effective analysys on them.
A telemetry solution should be chosen and integrated in the application together with an health check system to have live information on the status of the deployed service and its dependencies.
Dashboards and alerts should be set up to monitor the health of the system.

### Security 

Currently the API does not require any authentication or authorization features, a production service would likely require a proper authentication system to be set up before going live.

### Resiliency

Currently the API does not have any rate limiting capabilities, this feature should be included in a production ready system that relies on rate limited resources.

### Distributed Caching

The application offers very limited caching capabilities, it will cache any successful response to a request for 30 minutes using an in memory cache. 
A more robust and distributed option should be considered for production to allow the service to scale. 

### Testing and CI

Currently the solution includes only unit tests and a very basic suite of integration tests.
The test suite should be expanded to include acceptance, performance and load tests.
In addition to the testing improvements some environments should be set up to test deploys and test the application.
