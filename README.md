# Fortitude

Is a .NET Core toolkit or library for .NET 7 for building low latency, high throughput 
applications with protocols optimised for market trading applications.

## Goals
1. Make it easy to build eTrading applications in .NET 
2. Minimise publish latency between servers communicating
3. Minimise garbage generated on market updates
4. Take advantage of mechanical synchronization of x64 architecture and multithreading
5. Support Windows and Linux as equal deployment options
6. Provide reliability and updatability through comprehensive testing

# Why this exists
.NET and .NET core is a great platform for building eTrading applications thanks to it's availability of 
structs and high precision decimal type that unlike Java's BigDecimal generates garbage when handling numbers.
However unlike Java, .NET library of low latency class is lacking and this library hopes to provide those missing
tools to allow .NET developers the same ease of building trading applications.

## Current State 
This project is in Beta v0.1 release and is under heavy development.  It is expected to change frequently 
and with backwards compatibility support until it make v1.0.

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
* Low Garbage Socket Send Receive
  * Fast TCP
  * Fast UDP / UDP Multicast 
* Compact Object Serialization Deserialization
* Network Clients And Server
* Pricing Implementation
  * Library Compression 
  * Level 0, 1, 2 Quotes
  * Recently Traded Info
  * Efficent Delta UDP mutlicast updates
  * Snapshot Request  
  * Support 20+ Book Depth with many attributes
* Order and Execution transmission
  
## Whats going to be in future releases
* Actor/Rules platform for launching rules/actors
* Lowest Garbage and Latency event bus
* Topic/Channel request response and pub sub messaging
* Example client server implementations
* Execution results on standard cloud machines
* wiki how to guides
* CI/CD

# Who is this for
This library exists for anyone who cares about speed and latency and not just eTrading systems.  
Those who care about latency know how much work goes into reducing Garbage Collection in your code.
Code that generates lots of garbage frequently pauses while .NET can remove unreference objects and
consoliate memory so that allocations can be performed faster.
If you need to send data and rapidly receive responses then this library could be for you.
The eventual goal would be to remove Garbage in the pricing related classes it is not yet totally garbage free.  
Work will be done in the future to get Garbage Collection in pricing flow as low as possible it is not yet
the most important goal of this project.  Work will be done to minimise trading Garbage but it might be done
if zero garbage pricing can be achieved.

# Notes for developing and testing
When building and running the tests on windows please ensure you have installed "Microsoft KM-TEST Loopback Adapter"
and have it as the highest priority adapter via
* Network Adapters 
  * Microsoft KM-TEST Loopback Adapter 
    * Properties 
      * Internet Protocol Version 4 (TCP/IPv4)
        * Properties
          * Advanced
            * Untick "Automatic Metric"
              * Lower number has higher priority 

Then ensure any adapter that appears above "Microsoft KM-TEST Loopback Adapter" in ipconfig has a higher priority.
Also ensure the IP address in FortitudeTests -> TestEnvironment -> TestMachineConfig matches that of the 
"Microsoft KM-TEST Loopback Adapter"