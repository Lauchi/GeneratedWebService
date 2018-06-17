#!/usr/bin/env bash
rm -f Host/Eventstore.db
rm -f Host/Hangfire.db
cd SqlAdapter/

dotnet ef migrations remove -s ../Host/ --context EventStoreContext
dotnet ef migrations add InitialMigration -s ../Host/ --context EventStoreContext
dotnet ef database update -s ../Host/ --context EventStoreContext

dotnet ef migrations remove -s ../Host/ --context HangfireContext
dotnet ef migrations add InitialMigration -s ../Host/ --context HangfireContext
dotnet ef database update -s ../Host/ --context HangfireContext
cd ..