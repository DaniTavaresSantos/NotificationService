## Rate-Limited Notification Service
This is a project focused on protect recipients from getting too many emails, either due to system errors or due to abuse

## 💻 Solution Proposal

This project uses DDD with Clean Architecture.

The backEnd Programming language is:
- C# .NET Core 8

It was used some tools, Technologies and Patterns to allow the solution to work:
- Docker
- SwaggerUI
- Redis
- xUnit
- K6
- Factory Pattern
- Strategy Pattern
- Result Pattern


## How to Run the App

1. Run a Docker App as Docker Desktop or Rancher Desktop
2. Open the command prompt and type: "docker-compose build" 
3. Type "docker-compose up -d" to start the application containers.
4. navigate to http://localhost:8082/swagger and use the Endpoints.

to stop containers please type "docker-compose down"

## Automated Tests

For this solution I decided to implement Unit Testings for most of my Application. Achieving the coverage of 87%.
I implemented as well LoadTests using K6 tool.
To run the load tests you should follow these steps:
1. Intall K6 tool using the following link as guide: https://k6.io/docs/get-started/installation/
2. Once you are with k6 installed, open the command prompt on the project folder and type: "cd LoadTest"
3. Then you should type "K6 run -d 15s -u 2 ./loadtest_script.js"
4. Finally you will see a dashboard showing how many successful requests we had with 15 seconds of constant requests using 2 virtual machines.
5. The project is configured to use a mocked request with the Request Type = Status, so It will allow only 3 requests per minute.

##  Testing with Swagger or Postman

1. 


