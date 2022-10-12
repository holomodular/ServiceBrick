![SERVICE BRICK Logo](https://github.com/holomodular/ServiceBrick/blob/main/Logo.png)  

# SERVICE BRICK EXAMPLES

This page contains examples using the SERVICE BRICK platform.

## Before You Begin

### Storage Providers

We provide several startup files so you can use choose to use different storage providers.
Modify the Program.cs file to use the startup class you desire as well as any appsettings.json configuration settings or connection strings.
By default, all examples use the InMemory storage provider initially so all data will be destroyed when the web application stops.

### Security

Each client console application that calls its associated web application uses a default email and password, currently set to UnitTest@ServiceBrick.com and UnitTest123!@#
When starting the web application, a register admin screen will appear with these credentials filled in.
Just click the submit button to use the default values so the client application can connect with default values.
Otherwise, change the client appsettings.json file to use your values.

## WebAppStarterTemplate

This is a basic, barebone web application with a few common development configurations setup for use with Swagger.
It is a generic web app that is used with all of our web app examples.


## SingleHostSingleDatabase

This example demonstrates how to host the SERVICE BRICK platform in a single web application
and all Service Brick Modules are configured to store their data into the same database (same provider).
This example uses the InMemory Service Bus Brick for asynchronous communication for all microservices.

### Running the Application

The SingleHostSingleDatabase.sln is setup to start the web application on port 7000.
When the first page is deplayed to create an administrative user, click the Submit button to use the default values.
Open the client.sln solution to open the client console application. 
The client application demonstrates how to create, update, get, query and delete objects for each microservice.

## SingleHostMultipleDatabases

This example demonstrates how to host the SERVICE BRICK platform in a single web application
and all Service Brick Modules are configured to store their data into their own independent database.
This example uses the InMemory Service Bus Brick for asynchronous communication for all microservices.

### Running the Application

The SingleHostMultipleDatabases.sln is setup to start the web application on port 7000.
When the first page is deplayed to create an administrative user, click the Submit button to use the default values.
Open the client.sln solution to open the client console application. 
The client application demonstrates how to create, update, get, query and delete objects for each microservice.

## DistributedDeployment

This example demonstrates how to host the SERVICE BRICK platform in a distributed deployment.
Each Microservice is contained within its own web application and its own port.
* Security Service - port 7000
* Logging Service - port 7001
* Cache Service - port 7002
* Notification Service - port 7003

### Service Bus Required
This example requires the use of a Service Bus provider to communicate asynchronous messages between microservices.
By default, this example is setup to use Azure Service Bus (Basic - using Queues).
Change the appsettings.json file for the ServiceBrick:ServiceBus:Azure:ConnectionString 
config value with your value. Additionally, you can also use Azure Standard/Advanced.
To use topics and subscriptions, change the (3) three startup methods for AddBrickServiceBusAzure, RegisterBrickServiceBusAzure, and StartBrickServiceBusAzure
to be the following: AddBrickServiceBusAzureAdvanced, RegisterBrickServiceBusAzureAdvanced, StartBrickServiceBusAzureAdvanced

### Running the Application
The solution is setup to start multiple web applications at the same time.
Go to the Security Service browser (port 7000) and click the Submit button to create an ADMIN user.
Open the client.sln solution to open the client console application. 
The client application demonstrates how to create, update, get, query and delete objects for each microservice.


