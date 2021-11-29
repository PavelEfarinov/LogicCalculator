# Logic Calculator

## Client
Consists only of GRPC facades to communicate with the server and options to parse user input (and program arguments)

To launch client app you have to pass 2 options `--fileName userSessionFile.txt --url http://localhost:5000`
Client stores its session in this file between launches.

Connection consists of 3 steps:

- Locate session file
- If found, pass stored session to the server.
- If not, ask the server for new Guid
- If server has no such session, it will provide a new one

## Server

Launches migrations of DB before host launch.
These migrations create user and nodes tables for future use.

Every request contains sessionId and server checks if given user has access to the nodes in question.

## Projects

- Logic.Client -- client app
- Logic.Server -- server app
- Logic.Utils -- common tools
- Logic.Dto -- DTOs to pass from controller to the service (probably, unnecessary)
- Logic.Proto -- protobuf contract definition
- Db.Connector -- db connector manager
- DB.Migrator -- db migrator and migration definition
