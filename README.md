# Introduction 
This is a .NET Core virtual KVM tool that allows you to use your keyboard and mouse on another computer as-if it was connected directly.
This is achieved by implementing low-level OS hooks into events, tracking mouse deltas, and keeping track of a calculated virtual coordinate to determine your current screen.
Input is captured, potentially canceled, and replayed on the relevant client. 
Client-server is handled over HTTP with messagepack and SignalR.
UDP would obviously be more performant than HTTP, but it seems good enough on my network...

# Getting Started
This app is developed against a preview build of .NET Core 2.1 and SignalRCore. https://github.com/aspnet/Home 
1.	Follow the instructions for setting up a preview dotnet core SDK. 
2.	Run Server. You may need to run as Admin for Kestrel to bind? Also may need to do some netsh stuff?
3.	Run Clients. Must be as Admin to hook low-level. IP address of server should be autodiscovered using UDP broadcast messages.

# Future Plans
I have a list of issues in the client project that i'm working through. Also need SSL.
