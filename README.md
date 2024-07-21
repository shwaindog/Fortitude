# Fortitude

Is a .NET Core toolkit or library for .NET 8 for building low latency, high throughput 
applications with protocols optimised for market trading applications.

## Goals
1. Make it easy to build eTrading applications in .NET 
2. Minimise publish latency between servers communicating
3. Minimise garbage generated on market updates
4. Take advantage of mechanical sympathy of x64 architecture and multithreading
5. Support Windows and Linux as equal deployment options
6. Provide reliability and updatability through comprehensive testing

# Why this exists
.NET and .NET core is a great platform for building eTrading applications thanks to it's availability of 
structs and high precision decimal type that unlike Java's BigDecimal generates garbage when handling numbers.
However unlike Java, .NET lacks a pleothra of low latency libraries and this library hopes to provide those missing
tools to allow .NET developers the same ease of building trading applications.

## Current State 
This project is in Beta v0.1 release and is under heavy development.  It is expected to change frequently 
and with NO backwards compatibility support until it reaches v1.0.

## Whats in the Box Now
The following are a list of features found in the Fortitude Libraries
* Low Latency Event Rings
  * Disruption Ring 
* High Performance Collections
  * Collection Extensions Methods
  * Doubly Linked Lists
  * Garbage Free Maps (Dictionary)
  * Reusable Enumerator
  * Recyclable and Reusable Ref Counted Objects and Pools 
* OS Abstractions
  * Networking
  * Multi-threading
  * Virtual Memory
* Low Garbage Metric Collection
  * Tracing
  * StopWatch
  * Latency Trigger Logs
  * Low latency logging 
* File System Hierarchical Time Series Storage
  * Manage data from your terminal
  * Copy data locally for rapid backtesting
  * Memory Mapped Indexed Buckets
    * Sliding (If VM allocation allows) Memory Mapped File View
  * Constrainable Result Set Generation
  * Batched Retrieval
  * Throttled Retrieval
  * LZMA (7Zip) bucket compression
* Indicator Generation
  * Time Weighted Moving Average
  * Candles  
* Low Garbage Actor Framework
  * Non-Locking and Blocking Disruption Message Queues
  * Task Parallel Enabled
  * Paritition Queues Types
  * Address Inteceptors
  * Target / Deploy to Queues based on Load
  * ValueTask Native (Low Garbage)
  * Integrated with Pricing Client And Server
  * Supporting Request/Response, Pub/Sub and Private Channels
  * Auto Object InterQueue Marshalling  
* Reflection Helpers
* Reusable/Mutable String Alternative
* Low Garbage Socket Send Receive
  * Fast TCP
  * Fast UDP / UDP Multicast 
* Compact Object Serialization Deserialization
* Network Clients And Server
* Pricing Implementation
  * Library Compression 
  * Single value tick updates
  * Level 1, 2 & 3 Quotes
  * Recently Traded Info
  * Efficent Delta UDP mutlicast updates
  * Snapshot Request  
  * Support 20+ Book Depth with many attributes
  * Price Period Summaries (Candles)
  * Time series storage and retrieval
* Order and Execution transmission
  
## Whats going to be in future releases
* Add Low Garbage IO library to Actor library
  * Connect different Actor library together as a cluster 
  * Lowest Garbage and Latency event bus
* Sample Venue Adapter
* Sample Pricing and Trading Client  
* Indicator Signals
  * Moving Average Velocity Threshold
  * Volatility Calculation
  * Aggregated Instrument Trend
  * Support and Resistance Tracking
  * Peak and Trough
* Example client server implementations
* Execution results on standard cloud machines
* wiki how to guides

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

# Notes for developing and testing
When building and running the tests on windows please ensure you have installed "Microsoft KM-TEST Loopback Adapter".
Component Tests assume your loop back IP address is 169.254.224.238 on windows.
