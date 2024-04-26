# Pokedex

A basic pokedex API

## Getting started

Before getting started with your pokedex you will need a couple of things

- Install the .NET 8 sdk you can find it [here](https://dotnet.microsoft.com/en-us/download)
- Install docker and docker compose, installing [Docker Desktop](https://www.docker.com/get-started/) for your preferred OS should give you everything you need.

Once all dependencies are install you can either run the application using your IDE of choice or run it as a container.
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
