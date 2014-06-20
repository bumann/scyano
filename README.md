What is Scyano ?
===============
Scyano is a very simple helper that provides message queuing for any class.

How do I get started ?
======================
1. 		Install **Scyano** Nuget package
2. 		Add message handler to your class and add the [MessageConsumer] attribute
3. 		Initialize Scyano with your class (Initialize(object messageConsumer))
4.		Start Scyano (Start())
5.		Enqueue your messages (Enqueue(object message))
6.		Have fun :-)   

Where can I get it ?
====================
First, install NuGet.  
Then, install Scyano from the package manager console:

`PM> Install-Package Scyano`  
