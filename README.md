## Rate-Limited Notification Service
This is a project focused on protect recipients from getting too many emails, either due to system errors or due to abuse

## 💻 Solution Proposal

This project uses Clean Architecture.

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
4. Finally, you will see a dashboard showing how many successful requests we had with 15 seconds of constant requests using 2 virtual machines.
5. The project is configured to use a mocked request with the Request Type = Status, so It will allow only 2 requests per minute.

##  Testing with Swagger or Postman

1. Once you navigated to http://localhost:8082/swagger, you will see the Post Endpoint called Notification:
    ![image](https://github.com/user-attachments/assets/a6ff145c-7fb8-40fe-bc93-968741205871)
2. If you want to use the RateLimit functionality you should select the value of the header field: LimitType to: RateLimited:
     ![image](https://github.com/user-attachments/assets/d1c0bff9-fa9f-44f1-b1f0-aa2158e25dd9)
   - If you change the value of the header LimitType to Unlimited, there won't be limit rules applied to the request, so it will work everytime.
4. For the body, I suggest you to use this json:
```json
    {
      "type": "Status",
      "recipient": {
        "name": "daniel",
        "emailAdress": "dani.tav99@hotmail.com"
      },
      "message": {
        "title": "Important message",
        "body": "This message is Important"
      }
    }
```
4.The field: "type" defined what will be the type of the Request: Status, News or Marketing. 
- Their Rate Limits values are:
```json
"RateLimits": {
      "Status": {
        "Limit": 2,
        "TimePeriod": "00:01:00" // 1 minute
      },
      "News": {
        "Limit": 1,
        "TimePeriod": "1.00:00:00" // 1 day
      },
      "Marketing": {
        "Limit": 3,
        "TimePeriod": "01:00:00" // 1 hour
      }
    }
```

5. Once we're testing using the type: #Status on the json example above, it will be possible to send 2 requests successfully on the Time Period of 1 minute for the emailAdress and type provided.
- If you change any of these values (emailAdress or Type) the identity of the request will be changed, and It will be possible to send more requests on the same Time Period.

# Data Storing choices
On this project I needed to choose two data storing tools, one to store the RateLimits information, and other to store the number of requests made by the user.

## 1. RateLimits information - AppSettings:
  - To store the RateLimits information I chose to use the AppSettings file. 
  - AppSettings file is a Dictionary type file that is responsible to provide Key-value informations that can be used while the application is running. 
  - Once this file is treated as a Dictionary, we have constant time to Search for values on this file, if we have the key of them.
  - And another reason for this approach, is that is really easy to add new RateLimits information to the project, if we just add a new RateLimit called "Weather" the only change that is going to be made is that:
  
```json
"RateLimits": {
      "Status": {
        "Limit": 2,
        "TimePeriod": "00:01:00" // 1 minute
      },
      "News": {
        "Limit": 1,
        "TimePeriod": "1.00:00:00" // 1 day
      },
      "Marketing": {
        "Limit": 3,
        "TimePeriod": "01:00:00" // 1 hour
      },
      "Weather": {
        "Limit": 5,
        "TimePeriod": "00:05:00" // 5 minutes.
      }
    }
```
## 2. Number of Already made Requests - Redis Distributed Cache:
  - To store the number of requests of the user, and update them when its needed, I chose Redis.
  - Redis has a few usabilities, and one of them is using it as a Distributed Cache.
  - For this solution I'm imagining that this Application is going to have thousands of concurrent requests per hour, so a robust database is needed.
  - So, If we have a horizontal scalling of the application, with multiple instances, we will still use the same Database for all the pods, ensuring that If a user make a request on the pod 1 and other request on pod 2, We will count both requests, 
  and the application won't allow a new Request in any other pod, if the limit for that type of Request is 2.
  - I chose Redis over MemoryCache of .NET because with MemoryCache we would have a new Cache for each instance of the app, enabling possible consistency errors.






