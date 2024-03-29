﻿# Distributed Systems Final Project

[![Build Status](https://dev.azure.com/iust-msc/distributed-system-final/_apis/build/status/distributed-system-final?branchName=master)](https://dev.azure.com/iust-msc/distributed-system-final/_build/latest?definitionId=1&branchName=master)

**NOTE**: Query 2 ran for a limited number of events, therefore the result is not complete. Query1 ran completely for all of the 2 million events.

## Getup and Running

### Prerequesties

- [WSO2 Streaming Integrator Tooling Edition](https://wso2.com/integration/streaming-integrator)
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)

### Run Project

1- Go to `bin` folder of WSO2 SI Tooling and run the launcher.

```powershell
cd "C:\WSO2\Streaming Integrator Tooling\1.0.0\bin"
.\launcher_tooling.bat
```

Now you should be able to see the WSO2 SI Tooling Editor at `http://127.0.0.1:9390/editor`.

2- From menu select **File** > **New** and create two queries for query one and two, and name them `DebsQuery1.siddhi` and `DebsQuery2.siddhi` with contents from `"src\GrandChallange\SiddhiApps"`.

3- Run the siddhi script for the query you want to test using the WSO2 dashboard.

4- Run both "Grandchallange" client and "Grandchallange.EventWebService".

In *Visual Studio*: You can use "Multiple Startup Projects" from **Solution** > **Properties** > **Startup Project**.

-- or --

In *Command line*: Run `dotnet run` for both projects.

```powershell
cd "src\GrandChallange.EventWebService"
dotnet run

cd "src\GrandChallange"
dotnet run
```

5- Use client app guides to start project. Results are stored for Query1 and Query2 in files named "Query1_res.txt" and "Query2_res.txt" respectively.
