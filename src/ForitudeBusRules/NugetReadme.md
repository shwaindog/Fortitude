# Fortitude Bus Rules

Is a .NET Core library for .NET 7+ for building low latency, high throughput 
applications using an actor model that handles synchronisation between blocks of
code similar how the Windows Message Pump or Java's Vertx framework.
It allows you to deploy and communicate with "Rules" or Actors that can communicate
with each other in a thread safe manner by synchronizing all communication via a
disruptor like event queue.  It also provides facilities to clone and recycle objects
passed between "Rules" by providing a refeerence counting and recycling framework to
minimise or eliminate garbage collection.

## Release
v0.1.1
under the MIT License

### Release Notes 
This project is in Beta v0.1.1 release and is under heavy development.    
It is planned over 2024 to get this project to v1.0.

## Goals
1. Make it easy to build highly responsive applications in .NET 
3. Minimise garbage generated on market updates
4. Take advantage of mechanical sympathy of x64 architecture and multithreading
5. Support Windows and Linux as equal deployment options
6. Provide reliability and updatability through comprehensive testing

# Why this exists
If you've use Akka, Vertx or any other actor based framework and would like a .NET reinterruptation of
these that takes advantage of the .NET Task Parallel library to allow you to communicate with other actors
in an async await fashion and control and reuse messages to minimise garbage collection.

## Whats in the Box Now
The following are a list of features found in the Fortitude Libraries
* Low Latency Event Rings
  * Disruption Ring 
* High Performance Collections
  * Collection Extensions Methods
  * Doubly Linked Lists
  * Garbage Free Maps (Dictionary)
  * Reusable Enumerator 
* OS Abstractions
  * Networking
  * Multi-threading
* Low Garbage Metric Collection
  * Tracing
  * StopWatch
  * Latency Trigger Logs
  * Low latency logging 
* Reflection Helpers
* Reusable String Replacement

  
## Whats going to be in future releases
* Connect instances of Bus Rules Message Bus to other instances to allow sending messages on different servers
* Provide a sensible serialization interface to allow developers to register serializers and deserializes for messages
* Improve unit test coverage
* Add msbuild test and release pipelines

# Who is this for
This library exists for anyone who cares about speed and latency and not just eTrading systems.  
Those who care about latency know how much work goes into reducing Garbage Collection in your code.
Code that generates lots of garbage frequently pauses while .NET can remove unreachable objects and
consoliate memory so that allocations can be performed faster.
If you need to send data and rapidly receive responses then this library could be for you.
The eventual goal would be to remove Garbage in the pricing related classes it is not yet totally garbage free.  
Work will be done in the future to get Garbage Collection in pricing flow as low as possible it is not yet
the most important goal of this project.  Work will be done to minimise trading Garbage but it might be done
if zero garbage pricing can be achieved.

